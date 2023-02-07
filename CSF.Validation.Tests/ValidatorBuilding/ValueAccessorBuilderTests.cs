using System;
using System.Linq;
using AutoFixture.NUnit3;
using CSF.Validation.Manifest;
using CSF.Validation.Stubs;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.ValidatorBuilding
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class ValueAccessorBuilderTests
    {
        [Test,AutoMoqData]
        public void GetManifestRulesShouldIterateOverEveryRuleAdded([Frozen, ManifestModel] ValidatorBuilderContext context,
                                                                    [Frozen] IGetsRuleBuilder ruleBuilderFactory,
                                                                    ValueAccessorBuilder<ValidatedObject,string> sut,
                                                                    [ManifestModel] ManifestItem value)
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

            sut.AddRuleWithParent<StringValueRule>();
            sut.AddRuleWithParent<StringValueRule>();

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
                                                                    [ManifestModel] ManifestItem value)
        {
            Mock.Get(ruleBuilderFactory)
                .Setup(x => x.GetRuleBuilder<StringValueRule>(It.IsAny<ValidatorBuilderContext>(), It.IsAny<Action<IConfiguresRule<StringValueRule>>>()))
                .Returns(ruleBuilder);
            Mock.Get(ruleBuilder)
                .Setup(x => x.GetManifestValue())
                .Returns(() => value);

            Action<IConfiguresRule<StringValueRule>> configFunction = r => { };
            sut.AddRuleWithParent<StringValueRule>(configFunction);

            Mock.Get(ruleBuilderFactory)
                .Verify(x => x.GetRuleBuilder<StringValueRule>(It.IsAny<ValidatorBuilderContext>(), configFunction), Times.Once);
        }

        [Test,AutoMoqData]
        public void AddRulesShouldAddBuilderReturnedFromManifestFactory([Frozen] IGetsValidatorManifest manifestFactory,
                                                                        [Frozen, ManifestModel] ValidatorBuilderContext context,
                                                                        ValueAccessorBuilder<ValidatedObject,string> sut,
                                                                        IGetsManifestValue manifest,
                                                                        [ManifestModel] ManifestRule rule,
                                                                        [ManifestModel] ManifestItem value)
        {
            Mock.Get(manifestFactory)
                .Setup(x => x.GetValidatorManifest(typeof(StringValidator), context))
                .Returns(manifest);
            Mock.Get(manifest).Setup(x => x.GetManifestValue()).Returns(() => value);

            sut.AddRules<StringValidator>();

            Assert.That(sut.GetManifestValue().Children.Single(), Is.SameAs(value));
        }

        [Test,AutoMoqData]
        public void AccessorExceptionBehaviourShouldMarkTheManifestValueWithThatBehaviour([Frozen, ManifestModel] ValidatorBuilderContext context,
                                                                                           ValueAccessorBuilder<ValidatedObject, string> sut,
                                                                                           ValueAccessExceptionBehaviour behaviour)
        {
            context.ManifestValue.AccessorExceptionBehaviour = behaviour;
            sut.AccessorExceptionBehaviour(behaviour);
            Assert.That(() => sut.GetManifestValue(), Has.Property(nameof(ManifestItem.AccessorExceptionBehaviour)).EqualTo(behaviour));
        }

        [Test,AutoMoqData]
        public void GetManifestValueShouldNotMarkTheManifestValueWithAnExceptionBehaviourIfAccessorExceptionBehaviourWasNotCalled([Frozen, ManifestModel] ValidatorBuilderContext context,
                                                                                                                                  ValueAccessorBuilder<ValidatedObject, string> sut)
        {
            context.ManifestValue.AccessorExceptionBehaviour = null;
            Assert.That(() => sut.GetManifestValue(), Has.Property(nameof(ManifestItem.AccessorExceptionBehaviour)).Null);
        }

        [Test,AutoMoqData]
        public void WhenValueIsShouldAddAPolymorphicTypeToTheManifestValue([Frozen] IGetsValidatorBuilderContext contextFactory,
                                                                           [Frozen, ManifestModel] ValidatorBuilderContext context,
                                                                           ValueAccessorBuilder<object, ValidatedObject> sut,
                                                                           [ManifestModel] ManifestItem polymorphicType)
        {
            polymorphicType.ItemType = ManifestItemType.PolymorphicType;
            var derivedContext = new ValidatorBuilderContext(polymorphicType);
            Mock.Get(contextFactory).Setup(x => x.GetPolymorphicContext(context, typeof(DerivedValidatedObject))).Returns(derivedContext);
            sut.WhenValueIs<DerivedValidatedObject>(c => { });

            Assert.Multiple(() =>
            {
                var manifestValue = sut.GetManifestValue();
                Assert.That(manifestValue.IsValue,
                            Is.True,
                            "Manifest value has polymorphic types.");
                Assert.That(manifestValue,
                            Has.Property(nameof(ManifestItem.PolymorphicTypes)).One.SameAs(polymorphicType),
                            "Manifest includes expected polymotphic type");
            });
        }
    }
}