using System;
using AutoFixture.NUnit3;
using CSF.Validation.Manifest;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.ValidatorBuilding
{
    [TestFixture,Parallelizable]
    public class RuleBuilderTests
    {
        [Test,AutoMoqData]
        public void GetManifestRuleShouldReturnAManifestRulewithCorrectIdentifier([Frozen] RuleBuilderContext context,
                                                                                  RuleBuilder<SampleRule> sut)
        {
            Func<object, object> GetFunc(Func<object, object> input) => input;
            context.ObjectIdentityAccessor =  GetFunc(x => new object());
            sut.Dependencies.Clear();

            sut.Name = "Rule name";

            var result = sut.GetManifestRule();

            Assert.Multiple(() =>
            {
                Assert.That(result.Identifier, Has.Property(nameof(ManifestRuleIdentifier.MemberName)).EqualTo(context.MemberName));
                Assert.That(result.Identifier, Has.Property(nameof(ManifestRuleIdentifier.ObjectIdentityAccessor)).SameAs(context.ObjectIdentityAccessor));
                Assert.That(result.Identifier, Has.Property(nameof(ManifestRuleIdentifier.RuleName)).EqualTo("Rule name"));
                Assert.That(result.Identifier, Has.Property(nameof(ManifestRuleIdentifier.RuleType)).EqualTo(typeof(SampleRule)));
            });
        }

        [Test,AutoMoqData]
        public void GetManifestRuleShouldReturnAManifestRuleWithCorrectConfigurationAction(RuleBuilder<SampleRule> sut)
        {
            sut.ConfigureRule(r => r.StringProp = "Property value");
            sut.Dependencies.Clear();

            var result = sut.GetManifestRule();

            var sampleRule = new SampleRule();
            result.RuleConfiguration(sampleRule);
            Assert.That(sampleRule.StringProp, Is.EqualTo("Property value"), "Configuration action applied correctly");

        }
        [Test,AutoMoqData]
        public void GetManifestRuleShouldReturnAManifestRuleWithCorrectDependencies([Frozen] IGetsManifestRuleIdentifierFromRelativeIdentifier relativeToManifestIdentityConverter,
                                                                                    RuleBuilder<SampleRule> sut,
                                                                                    RelativeRuleIdentifier relativeId1,
                                                                                    RelativeRuleIdentifier relativeId2,
                                                                                    RelativeRuleIdentifier relativeId3,
                                                                                    ManifestRuleIdentifier manifestId1,
                                                                                    ManifestRuleIdentifier manifestId2,
                                                                                    ManifestRuleIdentifier manifestId3)
        {
            Mock.Get(relativeToManifestIdentityConverter).Setup(x => x.GetManifestRuleIdentifier(relativeId1)).Returns(manifestId1);
            Mock.Get(relativeToManifestIdentityConverter).Setup(x => x.GetManifestRuleIdentifier(relativeId2)).Returns(manifestId2);
            Mock.Get(relativeToManifestIdentityConverter).Setup(x => x.GetManifestRuleIdentifier(relativeId3)).Returns(manifestId3);

            sut.Dependencies = new[] { relativeId1, relativeId2, relativeId3 };

            var result = sut.GetManifestRule();

            Assert.That(result, Has.Property(nameof(ManifestRule.DependencyRules)).EquivalentTo(new[] { manifestId1, manifestId2, manifestId3 }));
        }

        [Test,AutoMoqData]
        public void GetManifestRuleShouldReturnAManifestRuleWithCorrectAccessors([Frozen] RuleBuilderContext context,
                                                                                  RuleBuilder<SampleRule> sut)
        {
            Func<object, object> GetFunc(Func<object, object> input) => input;

            context.ValidatedObjectAccessor =  GetFunc(x => new object());
            context.ValueAccessor =  GetFunc(x => new object());
            context.ObjectIdentityAccessor =  GetFunc(x => new object());
            sut.Dependencies.Clear();

            var result = sut.GetManifestRule();

            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Property(nameof(ManifestRule.ValueAccessor)).SameAs(context.ValueAccessor));
                Assert.That(result, Has.Property(nameof(ManifestRule.ValidatedObjectAccessor)).SameAs(context.ValidatedObjectAccessor));
            });
        }
        
        public class SampleRule
        {
            public string StringProp { get; set; }
        }
    }
}