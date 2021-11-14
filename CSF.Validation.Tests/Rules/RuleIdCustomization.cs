using System;
using AutoFixture;

namespace CSF.Validation.Rules
{
    public class RuleIdCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<RuleIdentifier>(c => c.FromFactory((Type ruleType, Type validatedValueType, object objectIdentity)
                => new RuleIdentifier(ruleType, validatedValueType, objectIdentity)));
        }
    }
}