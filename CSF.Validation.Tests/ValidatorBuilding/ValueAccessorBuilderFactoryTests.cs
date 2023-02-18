using System;
using CSF.Validation.Manifest;
using CSF.Validation.Stubs;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.ValidatorBuilding
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class ValueAccessorBuilderFactoryTests
    {
        [Test,AutoMoqData]
        public void GetValueAccessorBuilderShouldReturnAnAccessorBuilder([ManifestModel] ValidatorBuilderContext context,
                                                                         IGetsRuleBuilder ruleBuilderFactory,
                                                                         IGetsValidatorBuilderContextFromBuilder validatorManifestFactory,
                                                                         IGetsValidatorBuilderContext contextFactory)
        {
            var sut = new ValueAccessorBuilderFactory(() => ruleBuilderFactory, () => validatorManifestFactory, () => contextFactory);
            Assert.That(() => sut.GetValueAccessorBuilder<ValidatedObject, string>(context, c => { }), Is.Not.Null);
        }

        [Test,AutoMoqData]
        public void GetValueAccessorBuilderShouldExecuteConfigurationUponBuilder([ManifestModel] ValidatorBuilderContext context,
                                                                                 IGetsRuleBuilder ruleBuilderFactory,
                                                                                 IGetsValidatorBuilderContextFromBuilder validatorManifestFactory,
                                                                                 IGetsValidatorBuilderContext contextFactory,
                                                                                 IConfiguresContext ruleBuilder,
                                                                                 [ManifestModel] ValidatorBuilderContext ruleContext)
        {
            Mock.Get(ruleBuilderFactory)
                .Setup(x => x.GetRuleBuilder<StringValueRule>(It.IsAny<ValidatorBuilderContext>(), It.IsAny<Action<IConfiguresRule<StringValueRule>>>()))
                .Returns(() => ruleBuilder);
            var sut = new ValueAccessorBuilderFactory(() => ruleBuilderFactory, () => validatorManifestFactory, () => contextFactory);

            sut.GetValueAccessorBuilder<ValidatedObject, string>(context, c => c.AddRuleWithParent<StringValueRule>());
            Mock.Get(ruleBuilderFactory)
                .Verify(x => x.GetRuleBuilder<StringValueRule>(context, It.IsAny<Action<IConfiguresRule<StringValueRule>>>()), Times.Once);
        }
    }
}