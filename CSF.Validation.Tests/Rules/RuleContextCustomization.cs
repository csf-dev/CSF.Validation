using AutoFixture;

namespace CSF.Validation.Rules
{
    public class RuleContextCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            new RuleIdCustomization().Customize(fixture);
            fixture.Customize<RuleContext>(c => c.FromFactory((RuleIdentifier id) => new RuleContext(id, null)));
        }
    }
}