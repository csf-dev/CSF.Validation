using System;
using System.Linq;
using AutoFixture.NUnit3;
using CSF.Validation.Manifest;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.ValidatorBuilding
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class RuleBuilderTests
    {
        [Test,AutoMoqData]
        public void GetManifestRuleShouldReturnAManifestRulewithCorrectIdentifierFromService([Frozen, ManifestModel] ValidatorBuilderContext context,
                                                                                             [Frozen] IGetsManifestRuleIdentifier identifierFactory,
                                                                                             RuleBuilder<SampleRule> sut,
                                                                                             [ManifestModel] ManifestRuleIdentifier identifier)
        {
            Mock.Get(identifierFactory)
                .Setup(x => x.GetManifestRuleIdentifier(typeof(SampleRule), It.IsAny<ValidatorBuilderContext>(), It.IsAny<string>()))
                .Returns(identifier);
            Func<object, object> GetFunc(Func<object, object> input) => input;
            context.ManifestValue.IdentityAccessor = GetFunc(x => new object());
            sut.Dependencies.Clear();

            sut.Name = "Rule name";

            sut.ConfigureContext(context);

            Assert.That(context.ManifestValue.Rules.Single().Identifier, Is.SameAs(identifier));
        }

        [Test,AutoMoqData]
        public void GetManifestRuleShouldReturnAManifestRuleWithCorrectConfigurationAction([Frozen] IGetsManifestRuleIdentifier identifierFactory,
                                                                                           [Frozen, ManifestModel] ValidatorBuilderContext context,
                                                                                           RuleBuilder<SampleRule> sut,
                                                                                           [ManifestModel] ManifestRuleIdentifier identifier,
                                                                                           string stringPropValue)
        {
            Mock.Get(identifierFactory)
                .Setup(x => x.GetManifestRuleIdentifier(typeof(SampleRule), It.IsAny<ValidatorBuilderContext>(), It.IsAny<string>()))
                .Returns(identifier);
            sut.ConfigureRule(r => r.StringProp = stringPropValue);
            sut.Dependencies.Clear();

            sut.ConfigureContext(context);

            var sampleRule = new SampleRule();
            context.ManifestValue.Rules.Single().RuleConfiguration(sampleRule);
            Assert.That(sampleRule.StringProp, Is.EqualTo(stringPropValue), "Configuration action applied correctly");

        }
        [Test,AutoMoqData]
        public void GetManifestRuleShouldReturnAManifestRuleWithCorrectDependencies([Frozen] IGetsManifestRuleIdentifierFromRelativeIdentifier relativeToManifestIdentityConverter,
                                                                                    [Frozen] IGetsManifestRuleIdentifier identifierFactory,
                                                                                    [Frozen, ManifestModel] ValidatorBuilderContext context,
                                                                                    RuleBuilder<SampleRule> sut,
                                                                                    [ManifestModel] ManifestRuleIdentifier identifier,
                                                                                    RelativeRuleIdentifier relativeId1,
                                                                                    RelativeRuleIdentifier relativeId2,
                                                                                    RelativeRuleIdentifier relativeId3,
                                                                                    [ManifestModel] ManifestRuleIdentifier manifestId1,
                                                                                    [ManifestModel] ManifestRuleIdentifier manifestId2,
                                                                                    [ManifestModel] ManifestRuleIdentifier manifestId3)
        {
            Mock.Get(identifierFactory)
                .Setup(x => x.GetManifestRuleIdentifier(typeof(SampleRule), It.IsAny<ValidatorBuilderContext>(), It.IsAny<string>()))
                .Returns(identifier);
            Mock.Get(relativeToManifestIdentityConverter).Setup(x => x.GetManifestRuleIdentifier(context.ManifestValue, relativeId1)).Returns(manifestId1);
            Mock.Get(relativeToManifestIdentityConverter).Setup(x => x.GetManifestRuleIdentifier(context.ManifestValue, relativeId2)).Returns(manifestId2);
            Mock.Get(relativeToManifestIdentityConverter).Setup(x => x.GetManifestRuleIdentifier(context.ManifestValue, relativeId3)).Returns(manifestId3);

            sut.Dependencies = new[] { relativeId1, relativeId2, relativeId3 };

            sut.ConfigureContext(context);

            Assert.That(context.ManifestValue.Rules.Single(),
                        Has.Property(nameof(ManifestRule.DependencyRules)).EquivalentTo(new[] { manifestId1, manifestId2, manifestId3 }));
        }
        
        public class SampleRule
        {
            public string StringProp { get; set; }
        }
    }
}