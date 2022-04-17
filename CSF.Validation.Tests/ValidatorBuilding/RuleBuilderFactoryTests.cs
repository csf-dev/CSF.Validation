using CSF.Validation.Manifest;
using CSF.Validation.Stubs;
using NUnit.Framework;

namespace CSF.Validation.ValidatorBuilding
{
    [TestFixture,Parallelizable,Ignore("Temporarily broken, to be restored")]
    public class RuleBuilderFactoryTests
    {
        [Test,AutoMoqData]
        public void GetRuleBuilderShouldReturnBuilder(IGetsManifestRuleIdentifierFromRelativeIdentifier identifierConverter,
                                                      IGetsManifestRuleIdentifier identifierFactory,
                                                      [ManifestModel] ValidatorBuilderContext context)
        {
            var sut = new RuleBuilderFactory(() => identifierConverter, () => identifierFactory);
            Assert.That(() => sut.GetRuleBuilder<ObjectRule>(context, c => { }), Is.Not.Null);
        }

        [Test,AutoMoqData]
        public void GetRuleBuilderShouldExecuteConfigurationUponBuilder(IGetsManifestRuleIdentifierFromRelativeIdentifier identifierConverter,
                                                                        IGetsManifestRuleIdentifier identifierFactory,
                                                                        [ManifestModel] ValidatorBuilderContext context,
                                                                        string name)
        {
            var sut = new RuleBuilderFactory(() => identifierConverter, () => identifierFactory);
            var result = sut.GetRuleBuilder<ObjectRule>(context, c => c.Name = name);
            Assert.That(result.Name, Is.EqualTo(name));
        }
    }
}