using System.Collections.Generic;

namespace CSF.ValidationMessages
{
    /// <summary>
    /// A model for the results of a validation process which may also
    /// include human-readable feedback messages for rules.
    /// </summary>
    public class ValidationResultWithMessages
    {
        /// <summary>
        /// Gets a collection of the results of individual validation rules.
        /// </summary>
        public IReadOnlyCollection<ValidationRuleResultWithMessage> RuleResults { get; }
    }
}