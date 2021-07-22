using System;
using System.Collections.Generic;
using CSF.Validation.Resources;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// Represents information about the result of running a single validation rule.
    /// Typically this is a class which implements either <see cref="IRule{TValidated}"/>
    /// or <see cref="IValueRule{TValue, TValidated}"/>.
    /// </summary>
    /// <remarks>
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
        /// Gets the outcome of the validation rule.
        /// </summary>
        public RuleOutcome Outcome { get; }
        
        /// <summary>
        /// Gets a key/value collection of arbitrary data which has been made available by the validation rule.
        /// </summary>
        public IDictionary<string,object> Data { get; }
        
        /// <summary>
        /// If the <see cref="Outcome"/> is <see cref="RuleOutcome.Errored"/> then this property may provide
        /// access to the exception which caused this error.  For all other outcomes this property will be
        /// <see langword="null"/>.
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RuleResult"/> class.
        /// </summary>
        /// <param name="outcome">The outcome of validation.</param>
        /// <param name="data">An optional collection of arbitrary key/value data which is provided by the validation rule logic.</param>
        /// <param name="exception">
        /// An optional exception which caused the validation process to error.  This parameter must be <see langword="null"/>
        /// if the <paramref name="outcome"/> is not <see cref="RuleOutcome.Errored"/>.
        /// </param>
        public RuleResult(RuleOutcome outcome, IDictionary<string,object> data = null, Exception exception = null)
        {
            if(outcome != RuleOutcome.Errored && !(exception is null))
            {
                var message = String.Format(ExceptionMessages.GetMessage("MayNotHaveExceptionIfNotErroredOutcome"),
                                            nameof(RuleResult),
                                            $"{nameof(RuleOutcome)}.{nameof(RuleOutcome.Errored)}");
                throw new ArgumentException(message, nameof(exception));
            }

            Outcome = outcome;
            Data = data ?? new Dictionary<string, object>();
            Exception = exception;
        }
    }
}