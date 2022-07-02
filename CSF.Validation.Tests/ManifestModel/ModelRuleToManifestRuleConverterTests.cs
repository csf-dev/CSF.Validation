using System;
using System.Linq;
using AutoFixture.NUnit3;
using CSF.Validation.Manifest;
using CSF.Validation.ValidatorBuilding;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.ManifestModel
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class ModelRuleToManifestRuleConverterTests
    {
        [Test,AutoMoqData]
        public void ConvertAllRulesAndAddToManifestValuesShouldSuccessfullyConvertASingleRule([Frozen] IGetsRuleConfiguration configProvider,
                                                                                              [Frozen] IResolvesRuleType ruleTypeResolver,
                                                                                              ModelRuleToManifestRuleConverter sut,
                                                                                              [ManifestModel] ModelAndManifestValuePair modelAndValue,
                                                                                              [ManifestModel] Rule rule,
                                                                                              Type ruleType,
                                                                                              Action<object> ruleConfig)
        {
            modelAndValue.ModelValue.Rules.Add(rule);
            Mock.Get(ruleTypeResolver).Setup(x => x.GetRuleType(rule.RuleTypeName)).Returns(ruleType);
            Mock.Get(configProvider).Setup(x => x.GetRuleConfigurationAction(ruleType, rule.RulePropertyValues)).Returns(ruleConfig);

            sut.ConvertAllRulesAndAddToManifestValues(new[] { modelAndValue });

            Assert.Multiple(() =>
            {
                Assert.That(modelAndValue.ManifestValue.Rules, Has.Count.EqualTo(1), "One rule has been added to the manifest value");
                var manifestRule = modelAndValue.ManifestValue.Rules.Single();
                Assert.That(manifestRule.Identifier,
                            Is.EqualTo(new ManifestRuleIdentifier(modelAndValue.ManifestValue, ruleType, rule.RuleName)),
                            "Rule has correct identifier");
                Assert.That(manifestRule.ManifestValue, Is.SameAs(modelAndValue.ManifestValue), "Rule has correct manifest value");
                Assert.That(manifestRule.RuleConfiguration, Is.SameAs(ruleConfig), "Rule has correct config action");
            });
        }

        [Test,AutoMoqData]
        public void ConvertAllRulesAndAddToManifestValuesShouldSuccessfullyConvertTheRuleDependencies([Frozen] IGetsRuleConfiguration configProvider,
                                                                                                      [Frozen] IResolvesRuleType ruleTypeResolver,
                                                                                                      [Frozen] IGetsManifestRuleIdentifierFromRelativeIdentifier relativeToManifestIdentifierConverter,
                                                                                                      ModelRuleToManifestRuleConverter sut,
                                                                                                      [ManifestModel] ModelAndManifestValuePair modelAndValue,
                                                                                                      [ManifestModel] Rule rule,
                                                                                                      Type ruleType,
                                                                                                      Action<object> ruleConfig,
                                                                                                      RelativeIdentifier dependency1,
                                                                                                      RelativeIdentifier dependency2,
                                                                                                      Type dependencyType1,
                                                                                                      Type dependencyType2,
                                                                                                      ManifestRuleIdentifier dependencyId1,
                                                                                                      ManifestRuleIdentifier dependencyId2)
        {
            modelAndValue.ModelValue.Rules.Add(rule);
            Mock.Get(ruleTypeResolver).Setup(x => x.GetRuleType(rule.RuleTypeName)).Returns(ruleType);
            Mock.Get(configProvider).Setup(x => x.GetRuleConfigurationAction(ruleType, rule.RulePropertyValues)).Returns(ruleConfig);
            rule.Dependencies.Add(dependency1);
            rule.Dependencies.Add(dependency2);
            Mock.Get(ruleTypeResolver).Setup(x => x.GetRuleType(dependency1.RuleTypeName)).Returns(dependencyType1);
            Mock.Get(ruleTypeResolver).Setup(x => x.GetRuleType(dependency2.RuleTypeName)).Returns(dependencyType2);
            Mock.Get(relativeToManifestIdentifierConverter)
                .Setup(x => x.GetManifestRuleIdentifier(modelAndValue.ManifestValue,
                                                        It.Is<RelativeRuleIdentifier>(r => r.RuleType == dependencyType1
                                                                                        && r.RuleName == dependency1.RuleName
                                                                                        && r.MemberName == dependency1.MemberName
                                                                                        && r.AncestorLevels == dependency1.AncestorLevels)))
                .Returns(dependencyId1);
            Mock.Get(relativeToManifestIdentifierConverter)
                .Setup(x => x.GetManifestRuleIdentifier(modelAndValue.ManifestValue,
                                                        It.Is<RelativeRuleIdentifier>(r => r.RuleType == dependencyType2
                                                                                        && r.RuleName == dependency2.RuleName
                                                                                        && r.MemberName == dependency2.MemberName
                                                                                        && r.AncestorLevels == dependency2.AncestorLevels)))
                .Returns(dependencyId2);


            sut.ConvertAllRulesAndAddToManifestValues(new[] { modelAndValue });

            Assert.That(modelAndValue.ManifestValue.Rules.Single().DependencyRules, Is.EquivalentTo(new[] { dependencyId1, dependencyId2 }));
        }
    }
}