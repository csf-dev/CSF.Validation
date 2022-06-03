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
                                                                         IGetsValidatorManifest validatorManifestFactory)
        {
            var sut = new ValueAccessorBuilderFactory(() => ruleBuilderFactory, () => validatorManifestFactory);
            Assert.That(() => sut.GetValueAccessorBuilder<ValidatedObject, string>(context, c => { }), Is.Not.Null);
        }

        [Test,AutoMoqData]
        public void GetValueAccessorBuilderShouldExecuteConfigurationUponBuilder([ManifestModel] ValidatorBuilderContext context,
                                                                                 IGetsRuleBuilder ruleBuilderFactory,
                                                                                 IGetsValidatorManifest validatorManifestFactory)
        {
            var sut = new ValueAccessorBuilderFactory(() => ruleBuilderFactory, () => validatorManifestFactory);
            sut.GetValueAccessorBuilder<ValidatedObject, string>(context, c => c.AddRuleWithParent<StringValueRule>());
            Mock.Get(ruleBuilderFactory)
                .Verify(x => x.GetRuleBuilder<StringValueRule>(context, It.IsAny<Action<IConfiguresRule<StringValueRule>>>()), Times.Once);
        }
    }
}