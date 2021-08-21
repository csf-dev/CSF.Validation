using System.Collections.Generic;
using AutoFixture;

namespace CSF.Validation.Rules
{
    public class RuleResultCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<RuleResult>(c => c.FromFactory((RuleOutcome outcome, Dictionary<string, object> data) => new RuleResult(outcome, data)));
        }
    }
}