using System;
using System.Collections.Generic;
using CSF.Validation;
using CSF.Validation.Rules;

namespace CSF.ValidationMessages
{
    /// <summary>
    /// A <see cref="ValidationRuleResult"/> which may additionally have an associated human-readable feedback message.
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