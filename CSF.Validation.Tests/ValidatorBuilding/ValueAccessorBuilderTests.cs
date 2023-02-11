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
                                                                    ValueAccessorBuilder<ValidatedObject,string> sut)
        {
            sut.AddRuleWithParent<StringValueRule>();
            sut.AddRuleWithParent<StringValueRule>();

            var manifestRules = sut.Context.GetManifestValue().Rules.ToList();

            Mock.Get(ruleBuilderFactory)
                .Verify(x => x.GetRuleBuilder<StringValueRule>(context, It.IsAny<Action<IConfiguresRule<StringValueRule>>>()),
                        Times.Exactly(2));
        }

        [Test,AutoMoqData]
        public void AddRuleShouldProvideConfigFunctionToRuleBuilder([Frozen] IGetsRuleBuilder ruleBuilderFactory,
                                                                    [Frozen, ManifestModel] ValidatorBuilderContext context,
                                                                    ValueAccessorBuilder<ValidatedObject,string> sut,
                                                                    IConfiguresContext ruleBuilder,
                                                                    [ManifestModel] ManifestRule rule,
                                                                    [ManifestModel] ManifestItem value)
        {
            Mock.Get(ruleBuilderFactory)
                .Setup(x => x.GetRuleBuilder<StringValueRule>(It.IsAny<ValidatorBuilderContext>(), It.IsAny<Action<IConfiguresRule<StringValueRule>>>()))
                .Returns(() => ruleBuilder);

            Action<IConfiguresRule<StringValueRule>> configFunction = r => { };
            sut.AddRuleWithParent<StringValueRule>(configFunction);

            Mock.Get(ruleBuilderFactory)
                .Verify(x => x.GetRuleBuilder<StringValueRule>(It.IsAny<ValidatorBuilderContext>(), configFunction), Times.Once);
        }

        [Test,AutoMoqData]
        public void AddRulesShouldNotAddAnyAdditionalContexts([Frozen] IGetsValidatorBuilderContextFromBuilder manifestFactory,
                                                                        [Frozen, ManifestModel] ValidatorBuilderContext context,
                                                                        ValueAccessorBuilder<ValidatedObject,string> sut,
                                                                        [ManifestModel] ValidatorBuilderContext subContext)
        {
            Mock.Get(manifestFactory)
                .Setup(x => x.GetValidatorBuilderContext(typeof(StringValidator), context))
                .Returns(subContext);

            sut.AddRules<StringValidator>();

            Assert.That(sut.Context.Contexts, Is.Empty);
        }

        [Test,AutoMoqData]
        public void AccessorExceptionBehaviourShouldMarkTheManifestValueWithThatBehaviour([Frozen, ManifestModel] ValidatorBuilderContext context,
                                                                                           ValueAccessorBuilder<ValidatedObject, string> sut,
                                                                                           ValueAccessExceptionBehaviour behaviour)
        {
            context.ManifestValue.AccessorExceptionBehaviour = behaviour;
            sut.AccessorExceptionBehaviour(behaviour);
            Assert.That(() => sut.Context.GetManifestValue(), Has.Property(nameof(ManifestItem.AccessorExceptionBehaviour)).EqualTo(behaviour));
        }

        [Test,AutoMoqData]
        public void GetManifestValueShouldNotMarkTheManifestValueWithAnExceptionBehaviourIfAccessorExceptionBehaviourWasNotCalled([Frozen, ManifestModel] ValidatorBuilderContext context,
                                                                                                                                  ValueAccessorBuilder<ValidatedObject, string> sut)
        {
            context.ManifestValue.AccessorExceptionBehaviour = null;
            Assert.That(() => sut.Context.GetManifestValue(), Has.Property(nameof(ManifestItem.AccessorExceptionBehaviour)).Null);
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
                var manifestValue = sut.Context.GetManifestValue();
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