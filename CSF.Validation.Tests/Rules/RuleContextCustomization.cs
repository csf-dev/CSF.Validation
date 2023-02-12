using System;
using AutoFixture;
using CSF.Validation.Manifest;

namespace CSF.Validation.Rules
{
    public class RuleContextCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            new RuleIdCustomization().Customize(fixture);
            new Manifest.ManifestModelCustomization().Customize(fixture);
            fixture.Customize<RuleContext>(c => c.FromFactory(GetRuleContextFunc));
        }
        
        Func<ManifestRule, RuleIdentifier, IValidationLogic, RuleContext> GetRuleContextFunc => GetRuleContext;

        RuleContext GetRuleContext(ManifestRule manifestRule,
                                   RuleIdentifier ruleIdentifier,
                                   IValidationLogic logic)
        {
            var valueContext = new ValueContext(Guid.NewGuid(), null, manifestRule.ManifestValue);
            return new RuleContext(manifestRule, ruleIdentifier, null, new [] { valueContext }, typeof(object));
        }

    }
}