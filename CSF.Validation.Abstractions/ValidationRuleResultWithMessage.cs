using System;
using System.Collections.Generic;
using CSF.Validation;
using CSF.Validation.Rules;

namespace CSF.Validation
{
    /// <summary>
    /// A model which is a specialisation of <see cref="ValidationRuleResult"/> that may additionally have
    /// an associated human-readable feedback message.
    /// </summary>
    public class ValidationRuleResultWithMessage : ValidationRuleResult
    {
        /// <summary>
        /// Gets a human-readable feedback message to be associated with this rule result.
        /// </summary>
        public string Message { get; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationRuleResultWithMessage"/> class.
        /// </summary>
        /// <param name="identifier">A unique identifier for the rule to which this result corresponds.</param>
        /// <param name="outcome">The outcome of validation.</param>
        /// <param name="data">An optional collection of arbitrary key/value data which is provided by the validation rule logic.</param>
        /// <param name="exception">
        /// An optional exception which caused the validation process to error.  This parameter must be <see langword="null"/>
        /// if the <paramref name="outcome"/> is not <see cref="RuleOutcome.Errored"/>.
        /// </param>
        /// <param name="message">The feedback message associated with this validation rule.</param>
        /// <exception cref="ArgumentNullException">If the <paramref name="identifier"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">
        /// <list type="bullet">
        /// <item>
        /// <description>
        /// If the <paramref name="outcome"/> is not a defined enumeration constant.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// If the <paramref name="outcome"/> is not <see cref="RuleOutcome.Errored"/> but the <paramref name="exception"/>
        /// is not <see langword="null"/>.
        /// </description>
        /// </item>
        /// </list>
        /// </exception>
        public ValidationRuleResultWithMessage(RuleIdentifier identifier,
                                               RuleOutcome outcome,
                                               IDictionary<string,object> data = null,
                                               Exception exception = null,
                                               string message = null) : base(identifier, outcome, data, exception)
        {
            Message = message;
        }
    }
}