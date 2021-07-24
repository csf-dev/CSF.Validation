using System.Collections.Generic;

namespace CSF.Validation
{
    /// <summary>
    /// A model for the results of a validation process.
    /// </summary>
    public class ValidationResult
    {
        /// <summary>
        /// Gets a collection of the results of individual validation rules.
        /// </summary>
        IReadOnlyCollection<ValidationRuleResult> RuleResults { get; }
    }
}