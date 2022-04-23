using System.Collections.Generic;

namespace CSF.Validation
{
    /// <summary>
    /// A model for the results of a validation process which may also
    /// include human-readable feedback messages for any failed rules.
    /// </summary>
    public class ValidationResultWithMessages
    {
        /// <summary>
        /// Gets a collection of the results for individual validation rules.
        /// </summary>
        public IReadOnlyCollection<ValidationRuleResultWithMessage> RuleResults { get; }
    }
}