using CSF.Validation.Stubs;
using NUnit.Framework;

namespace CSF.Validation.ValidatorBuilding
{
    [TestFixture,Parallelizable]
    public class RuleBuilderFactoryTests
    {
        [Test,AutoMoqData]
        public void GetRuleBuilderShouldReturnBuilder(IGetsManifestRuleIdentifierFromRelativeIdentifier identifierFactory,
                                                      RuleBuilderContext context)
        {
            var sut = new RuleBuilderFactory(() => identifierFactory);
            Assert.That(() => sut.GetRuleBuilder<ObjectRule>(context, c => { }), Is.Not.Null);
        }

        [Test,AutoMoqData]
        public void GetRuleBuilderShouldExecuteConfigurationUponBuilder(IGetsManifestRuleIdentifierFromRelativeIdentifier identifierFactory,
                                                                        RuleBuilderContext context,
                                                                        string name)
        {
            var sut = new RuleBuilderFactory(() => identifierFactory);
            var result = sut.GetRuleBuilder<ObjectRule>(context, c => c.Name = name);
            Assert.That(result.Name, Is.EqualTo(name));
        }
    }
}