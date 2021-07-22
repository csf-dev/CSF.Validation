using System;

namespace CSF.Validation.ValidatorBuilding
{
    public record RelativeRuleIdentifier(Type RuleType,
                                         string MemberName = null,
                                         string RuleName = null,
                                         int AncestorLevels = 0);
}