using System;

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
        /// <param name="result">A validation rule result to enrich.</param>
        /// <param name="message">The feedback message associated with this validation rule.</param>
        /// <exception cref="ArgumentNullException">If the <paramref name="result"/> is <see langword="null"/>.</exception>
        public ValidationRuleResultWithMessage(ValidationRuleResult result,
                                               string message = null) : base(result)
        {
            Message = message;
        }
    }
}