using System;
using System.Collections.Generic;
using AutoFixture;
using CSF.Validation.RuleExecution;

namespace CSF.Validation.Rules
{
    public class RuleResultCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            new RuleContextCustomization().Customize(fixture);
            fixture.Customize<RuleResult>(c => c.FromFactory((RuleOutcome outcome, Dictionary<string, object> data) => new RuleResult(outcome, data)));
            fixture.Customize<ValidationRuleResult>(c => c.FromFactory(GetValidationRuleResultFunc));
        }

        Func<RuleResult, RuleContext, ExecutableRule, ValidationRuleResult> GetValidationRuleResultFunc => GetValidationRuleResult;

        static ValidationRuleResult GetValidationRuleResult(RuleResult result, RuleContext context, ExecutableRule rule)
            => new ValidationRuleResult(result, context, rule);
    }
}