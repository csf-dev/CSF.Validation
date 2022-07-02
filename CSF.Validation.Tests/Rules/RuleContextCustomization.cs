using AutoFixture;

namespace CSF.Validation.Rules
{
    public class RuleContextCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            new RuleIdCustomization().Customize(fixture);
            new Manifest.ManifestModelCustomization().Customize(fixture);
        }
    }
}