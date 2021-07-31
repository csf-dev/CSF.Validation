using System;
using AutoFixture.NUnit3;
using CSF.Validation.Manifest;
using CSF.Validation.Stubs;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.ValidatorBuilding
{
    [TestFixture,Parallelizable]
    public class ValidatorBuilderTests
    {
        [Test,AutoMoqData]
        public void UseObjectIdentityShouldAddIdentityAccessorToTheContext([Frozen] ValidatorBuilderContext context,
                                                                           ValidatorBuilder<ValidatedObject> sut,
                                                                           ValidatedObject obj)
        {
            sut.UseObjectIdentity(x => x.Identity);

            Assert.That(() => context.ObjectIdentityAccessor(obj), Is.EqualTo(obj.Identity));
        }

        [Test,AutoMoqData]
        public void AddRuleShouldAddRuleCreatedFromFactoryUsingContext([Frozen] ValidatorBuilderContext context,
                                                                       [Frozen] IGetsRuleBuilderContext ruleContextFactory,
                                                                       [Frozen] IGetsRuleBuilder ruleBuilderFactory,
                                                                       ValidatorBuilder<ValidatedObject> sut,
                                                                       RuleBuilderContext ruleContext,
                                                                       ManifestRule rule)
        {
            Mock.Get(ruleContextFactory)
                .Setup(x => x.GetContext(context))
                .Returns(ruleContext);
            Mock.Get(ruleBuilderFactory)
                .Setup(x => x.GetRuleBuilder<ObjectRule>(ruleContext, It.IsAny<Action<IConfiguresRule<ObjectRule>>>()))
                .Returns(() => Mock.Of<IBuildsRule<ObjectRule>>(m => m.GetManifestRules() == new[] { rule }));
            
            sut.AddRule<ObjectRule>();

            Assert.That(() => sut.GetManifestRules(), Is.EqualTo(new[] { rule }));
        }

        [Test,AutoMoqData]
        public void AddRulesShouldAddBuilderReturnedFromManifestFactory([Frozen] ValidatorBuilderContext context,
                                                                       [Frozen] IGetsValidatorManifest manifestFactory,
                                                                        ValidatorBuilder<ValidatedObject> sut,
                                                                        IGetsManifestRules manifest,
                                                                        ManifestRule rule)
        {
            Mock.Get(manifestFactory)
                .Setup(x => x.GetValidatorManifest(typeof(ValidatedObjectValidator), context))
                .Returns(manifest);
            Mock.Get(manifest).Setup(x => x.GetManifestRules()).Returns(() => new[] { rule });

            sut.AddRules<ValidatedObjectValidator>();

            Assert.That(sut.GetManifestRules(), Is.EqualTo(new[] { rule }));
        }
    }
}