using System;
using AutoFixture.NUnit3;
using CSF.Validation.RuleExecution;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.Manifest
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class ValidatorFromManifestFactoryTests
    {
        [Test,AutoMoqData]
        public void GetValidatorShouldThrowAneIfTheManifestIsNull(ValidatorFromManifestFactory sut)
        {
            Assert.That(() => sut.GetValidator(null), Throws.ArgumentNullException);
        }

        [Test,AutoMoqData]
        public void GetValidatorShouldReturnACorrectlyTypedGenericValidator([Frozen,AutofixtureServices] IServiceProvider resolver,
                                                                            ValidatorFromManifestFactory sut,
                                                                            [ManifestModel] ValidationManifest manifest)
        {
            manifest.ValidatedType = typeof(string);

            Assert.That(() => sut.GetValidator(manifest), Is.InstanceOf<Validator<string>>());
        }
    }
}