using System;
using System.Linq;
using AutoFixture.NUnit3;
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
        public void GetManifestRulesShouldIterateOverEveryRuleAdded([Frozen, ManifestModel] ValidatorBuilderContext context,
                                                                    [Frozen] IGetsRuleBuilder ruleBuilderFactory,
                                                                    ValueAccessorBuilder<ValidatedObject,string> sut,
                                                                    [ManifestModel] ManifestValue value)
        {
            Mock.Get(ruleBuilderFactory)
                .Setup(x => x.GetRuleBuilder<StringValueRule>(It.IsAny<ValidatorBuilderContext>(), It.IsAny<Action<IConfiguresRule<StringValueRule>>>()))
                .Returns(() => {
                    var ruleBuilder = new Mock<IBuildsRule<StringValueRule>>();
                    ruleBuilder
                        .Setup(x => x.GetManifestValue())
                        .Returns(() => value);
                    return ruleBuilder.Object;
                });

            sut.AddValueRule<StringValueRule>();
            sut.AddValueRule<StringValueRule>();

            var manifestRules = sut.GetManifestValue().Rules.ToList();

            Mock.Get(ruleBuilderFactory)
                .Verify(x => x.GetRuleBuilder<StringValueRule>(context, It.IsAny<Action<IConfiguresRule<StringValueRule>>>()),
                        Times.Exactly(2));
        }

        [Test,AutoMoqData]
        public void AddRuleShouldProvideConfigFunctionToRuleBuilder([Frozen] IGetsRuleBuilder ruleBuilderFactory,
                                                                    [Frozen, ManifestModel] ValidatorBuilderContext context,
                                                                    ValueAccessorBuilder<ValidatedObject,string> sut,
                                                                    IBuildsRule<StringValueRule> ruleBuilder,
                                                                    [ManifestModel] ManifestRule rule,
                                                                    [ManifestModel] ManifestValue value)
        {
            Mock.Get(ruleBuilderFactory)
                .Setup(x => x.GetRuleBuilder<StringValueRule>(It.IsAny<ValidatorBuilderContext>(), It.IsAny<Action<IConfiguresRule<StringValueRule>>>()))
                .Returns(ruleBuilder);
            Mock.Get(ruleBuilder)
                .Setup(x => x.GetManifestValue())
                .Returns(() => value);

            Action<IConfiguresRule<StringValueRule>> configFunction = r => { };
            sut.AddValueRule<StringValueRule>(configFunction);

            Mock.Get(ruleBuilderFactory)
                .Verify(x => x.GetRuleBuilder<StringValueRule>(It.IsAny<ValidatorBuilderContext>(), configFunction), Times.Once);
        }

        [Test,AutoMoqData]
        public void AddRulesShouldAddBuilderReturnedFromManifestFactory([Frozen] IGetsValidatorManifest manifestFactory,
                                                                        [Frozen, ManifestModel] ValidatorBuilderContext context,
                                                                        ValueAccessorBuilder<ValidatedObject,string> sut,
                                                                        IGetsManifestValue manifest,
                                                                        [ManifestModel] ManifestRule rule,
                                                                        [ManifestModel] ManifestValue value)
        {
            Mock.Get(manifestFactory)
                .Setup(x => x.GetValidatorManifest(typeof(StringValidator), context))
                .Returns(manifest);
            Mock.Get(manifest).Setup(x => x.GetManifestValue()).Returns(() => value);

            sut.AddRules<StringValidator>();

            Assert.That(sut.GetManifestValue().Children.Single(), Is.SameAs(value));
        }

        [Test,AutoMoqData]
        public void IgnoreExceptionsShouldMarkTheManifestValueAsIgnoringAccessorExceptions([Frozen, ManifestModel] ValidatorBuilderContext context,
                                                                                           ValueAccessorBuilder<ValidatedObject, string> sut)
        {
            sut.IgnoreExceptions();
            Assert.That(() => sut.GetManifestValue()?.IgnoreAccessorExceptions, Is.True);
        }

        [Test,AutoMoqData]
        public void GetManifestValueShouldNotMarkTheManifestValueAsIgnoringAccessorExceptionsIfIgnoreExceptionsWasNotCalled([Frozen, ManifestModel] ValidatorBuilderContext context,
                                                                                                                            ValueAccessorBuilder<ValidatedObject, string> sut)
        {
            Assert.That(() => sut.GetManifestValue()?.IgnoreAccessorExceptions, Is.False);
        }
    }
}