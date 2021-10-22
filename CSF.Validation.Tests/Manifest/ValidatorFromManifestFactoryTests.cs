using System;
using AutoFixture.NUnit3;
using CSF.Validation.RuleExecution;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.Manifest
{
    [TestFixture,Parallelizable]
    public class ValidatorFromManifestFactoryTests
    {
        [Test,AutoMoqData]
        public void GetValidatorShouldThrowAneIfTheManifestIsNull(ValidatorFromManifestFactory sut)
        {
            Assert.That(() => sut.GetValidator(null), Throws.ArgumentNullException);
        }

        [Test,AutoMoqData]
        public void GetValidatorShouldReturnACorrectlyTypedGenericValidator([Frozen] IServiceProvider resolver,
                                                                            ValidatorFromManifestFactory sut,
                                                                            IGetsRuleExecutor executorFactory,
                                                                            IGetsAllExecutableRulesWithDependencies ruleProvider,
                                                                            [ManifestModel] ValidationManifest manifest)
        {
            manifest.ValidatedType = typeof(string);
            Mock.Get(resolver).Setup(x => x.GetService(typeof(IGetsRuleExecutor))).Returns(executorFactory);
            Mock.Get(resolver).Setup(x => x.GetService(typeof(IGetsAllExecutableRulesWithDependencies))).Returns(ruleProvider);

            Assert.That(() => sut.GetValidator(manifest), Is.InstanceOf<Validator<string>>());
        }
    }
}