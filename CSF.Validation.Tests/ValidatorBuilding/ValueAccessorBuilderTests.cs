using System;
using System.Linq;
using AutoFixture.NUnit3;
using CSF.Validation.Autofixture;
using CSF.Validation.Manifest;
using CSF.Validation.Stubs;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.ValidatorBuilding
{
    [TestFixture,Parallelizable]
    public class ValueAccessorBuilderTests
    {
        [Test,AutoMoqData]
        public void GetManifestRulesShouldReturnOneRulePerRuleAdded([Frozen] IGetsRuleBuilder ruleBuilderFactory,
                                                                    ValueAccessorBuilder<ValidatedObject,string> sut,
                                                                    [ManifestModel] ManifestRule rule)
        {
            Mock.Get(ruleBuilderFactory)
                .Setup(x => x.GetRuleBuilder<StringValueRule>(It.IsAny<ValidatorBuilderContext>(), It.IsAny<Action<IConfiguresRule<StringValueRule>>>()))
                .Returns(() => {
                    var ruleBuilder = new Mock<IBuildsRule<StringValueRule>>();
                    ruleBuilder
                        .Setup(x => x.GetManifestRules())
                        .Returns(() => new[] { rule });
                    return ruleBuilder.Object;
                });

            sut.AddRule<StringValueRule>();
            sut.AddRule<StringValueRule>();

            var manifestRules = sut.GetManifestRules().ToList();

            Assert.That(manifestRules, Has.Count.EqualTo(2));
        }

        [Test,AutoMoqData]
        public void AddRuleShouldProvideConfigFunctionToRuleBuilder([Frozen] IGetsRuleBuilder ruleBuilderFactory,
                                                                    ValueAccessorBuilder<ValidatedObject,string> sut,
                                                                    IBuildsRule<StringValueRule> ruleBuilder,
                                                                    [ManifestModel] ManifestRule rule)
        {
            Mock.Get(ruleBuilderFactory)
                .Setup(x => x.GetRuleBuilder<StringValueRule>(It.IsAny<ValidatorBuilderContext>(), It.IsAny<Action<IConfiguresRule<StringValueRule>>>()))
                .Returns(ruleBuilder);
            Mock.Get(ruleBuilder)
                .Setup(x => x.GetManifestRules())
                .Returns(() => new[] { rule });

            Action<IConfiguresRule<StringValueRule>> configFunction = r => { };
            sut.AddRule<StringValueRule>(configFunction);

            Mock.Get(ruleBuilderFactory)
                .Verify(x => x.GetRuleBuilder<StringValueRule>(It.IsAny<ValidatorBuilderContext>(), configFunction), Times.Once);
        }

        [Test,AutoMoqData]
        public void AddRulesShouldAddBuilderReturnedFromManifestFactory([Frozen] IGetsValidatorManifest manifestFactory,
                                                                        [Frozen] ValidatorBuilderContext context,
                                                                        ValueAccessorBuilder<ValidatedObject,string> sut,
                                                                        IGetsManifestRules manifest,
                                                                        [ManifestModel] ManifestRule rule)
        {
            Mock.Get(manifestFactory)
                .Setup(x => x.GetValidatorManifest(typeof(StringValidator), context))
                .Returns(manifest);
            Mock.Get(manifest).Setup(x => x.GetManifestRules()).Returns(() => new[] { rule });

            sut.AddRules<StringValidator>();

            Assert.That(sut.GetManifestRules(), Is.EqualTo(new[] { rule }));
        }
    }
}