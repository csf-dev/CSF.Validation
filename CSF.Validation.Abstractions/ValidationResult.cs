using System.Collections.Generic;
using CSF.Validation.Rules;

namespace CSF.Validation
{
    public class ValidationResult
    {
        IReadOnlyCollection<ValidationRuleResult> RuleResults { get; }
    }
}