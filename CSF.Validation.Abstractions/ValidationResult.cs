using System.Collections.Generic;

namespace CSF.Validation
{
    public class ValidationResult
    {
        IReadOnlyCollection<ValidationRuleResult> RuleResults { get; }
    }
}