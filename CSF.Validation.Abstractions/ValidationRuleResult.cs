using System;
using System.Collections.Generic;
using CSF.Validation.Rules;

namespace CSF.Validation
{
    /// <summary>
    /// Represents information about the result of running a single validation rule.
    /// Typically this is a class which implements either <see cref="IRule{TValidated}"/>
    /// or <see cref="IValueRule{TValue, TValidated}"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class is a more complete version of <see cref="RuleResult"/>.  Where the rule result only contains
    /// the information which would be directly output from the rule itself, this class provides more contextual
    /// information which may be 'backfilled' by the validation framework.
    /// </para>
    /// </remarks>
    public class ValidationRuleResult : RuleResult
    {
        /// <summary>
        /// Gets a unique identifier for the rule to which this result corresponds.
        /// </summary>
        public RuleIdentifier Identifier { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationRuleResult"/> class.
        /// </summary>
        /// <param name="identifier">A unique identifier for the rule to which this result corresponds.</param>
        /// <param name="outcome">The outcome of validation.</param>
        /// <param name="data">An optional collection of arbitrary key/value data which is provided by the validation rule logic.</param>
        /// <param name="exception">
        /// An optional exception which caused the validation process to error.  This parameter must be <see langword="null"/>
        /// if the <paramref name="outcome"/> is not <see cref="RuleOutcome.Errored"/>.
        /// </param>
        public ValidationRuleResult(RuleIdentifier identifier,
                                    RuleOutcome outcome,
                                    IDictionary<string,object> data = null,
                                    Exception exception = null) : base(outcome, data, exception)
        {
            Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
        }

        /// <summary>
        /// Serves as a 'copy constructor' of sorts to initialise an instance of <see cref="ValidationRuleResult"/>
        /// from an existing <see cref="RuleResult"/> as well as the additional information required by this type.
        /// </summary>
        /// <param name="result">An existing instance of <see cref="RuleResult"/>.</param>
        /// <param name="identifier">A unique identifier for the rule to which this result corresponds.</param>
        /// <returns>An instance of <see cref="ValidationRuleResult"/> created from the provided information.</returns>
        public static ValidationRuleResult FromRuleResult(RuleResult result, RuleIdentifier identifier)
        {
            if (result is null)
                throw new ArgumentNullException(nameof(result));

            return new ValidationRuleResult(identifier, result.Outcome, result.Data, result.Exception);
        }
    }
}