using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using static CSF.Validation.Resources.ExceptionMessages;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A model for information about the result of running a single validation rule.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Validation rules are typically classes which implement either <see cref="IRule{TValidated}"/>
    /// or <see cref="IRule{TValue, TValidated}"/>.
    /// </para>
    /// <para>
    /// This class deals only with the possible information which could be returned by executing a validation
    /// rule.  It does not contain any other contextual information which would be 'backfilled' by the validation
    /// framework.  Instances of this class may be 'converted up' to instances of <see cref="ValidationRuleResult"/>
    /// for the purpose of returning final results.
    /// </para>
    /// </remarks>
    public class RuleResult
    {
        /// <summary>
        /// A special value that indicates that the result represents a rule which have timed-out when executing.
        /// </summary>
        /// <remarks>
        /// <para>
        /// When a result contains a data item with this key, the value is the <see cref="TimeSpan"/> timeout which was set.
        /// The string key is "<c>Validation rule timeout</c>".
        /// </para>
        /// </remarks>
        /// <seealso cref="IHasRuleTimeout"/>
        public static string RuleTimeoutDataKey => "Validation rule timeout";

        /// <summary>
        /// Gets the outcome of the validation rule.
        /// </summary>
        public RuleOutcome Outcome { get; }

        /// <summary>
        /// Gets a value that indicates whether or not the current result instance represents passing
        /// validation or not.  This property returns <see langword="true" /> if <see cref="Outcome"/> equals
        /// <see cref="RuleOutcome.Passed"/>.  This property returns <see langword="false" /> for any other outcome.
        /// </summary>
        public bool IsPass => Outcome == RuleOutcome.Passed;

        /// <summary>
        /// Gets a key/value collection of arbitrary data which has been made available by the validation rule.
        /// </summary>
        public IReadOnlyDictionary<string,object> Data { get; }
        
        /// <summary>
        /// If the <see cref="Outcome"/> is <see cref="RuleOutcome.Errored"/> then this property may provide
        /// access to the exception which caused this error.  For all other outcomes this property will be
        /// <see langword="null"/>.
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Gets a string representation of the current instance.
        /// </summary>
        /// <returns>A string representation of the current object.</returns>
        public override string ToString() => $"[{nameof(RuleResult)}:{Outcome}]";

        /// <summary>
        /// Initializes a new instance of the <see cref="RuleResult"/> class.
        /// </summary>
        /// <param name="outcome">The outcome of validation.</param>
        /// <param name="data">An optional collection of arbitrary key/value data which is provided by the validation rule logic.</param>
        /// <param name="exception">
        /// An optional exception which caused the validation process to error.  This parameter must be <see langword="null"/>
        /// if the <paramref name="outcome"/> is not <see cref="RuleOutcome.Errored"/>.
        /// </param>
        /// <exception cref="ArgumentException">
        /// If the <paramref name="outcome"/> is not <see cref="RuleOutcome.Errored"/> but the <paramref name="exception"/>
        /// is not <see langword="null"/>.
        /// </exception>
        public RuleResult(RuleOutcome outcome,
                          IDictionary<string, object> data = null,
                          Exception exception = null)
        {
            if(outcome != RuleOutcome.Errored && !(exception is null))
            {
                var message = String.Format(GetExceptionMessage("MayNotHaveExceptionIfNotErroredOutcome"),
                                            nameof(RuleResult),
                                            $"{nameof(RuleOutcome)}.{nameof(RuleOutcome.Errored)}");
                throw new ArgumentException(message, nameof(exception));
            }

            Outcome = outcome;
            Data = new ReadOnlyDictionary<string, object>(data ?? new Dictionary<string, object>());
            Exception = exception;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RuleResult"/> class.  This is essentially a copy-constructor.
        /// </summary>
        /// <param name="result">A rule result instance to copy.</param>
        /// <exception cref="ArgumentNullException">If the <paramref name="result"/> is <see langword="null"/>.</exception>
        protected RuleResult(RuleResult result)
        {
            if (result is null)
                throw new ArgumentNullException(nameof(result));

            Outcome = result.Outcome;
            Data = result.Data;
            Exception = result.Exception;
        }
    }
}