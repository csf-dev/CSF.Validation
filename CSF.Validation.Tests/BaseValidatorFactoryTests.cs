using AutoFixture.NUnit3;
using CSF.Validation.Manifest;
using Moq;
using NUnit.Framework;

namespace CSF.Validation
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class BaseValidatorFactoryTests
    {
        [Test,AutoMoqData]
        public void GetValidatorGenericShouldReturnGenericValidatorFromCreatedManifest([Frozen] IGetsManifestFromBuilder manifestFromBuilderProvider,
                                                                                       [Frozen] IGetsValidatorFromManifest validatorFromManifestFactory,
                                                                                       BaseValidatorFactory sut,
                                                                                       IBuildsValidator<object> builder,
                                                                                       [ManifestModel] ValidationManifest manifest)
        {
            Mock.Get(manifestFromBuilderProvider).Setup(x => x.GetManifest(builder)).Returns(manifest);
            var validator = new Mock<IValidator<object>>();
            var nonGenericValidator = validator.As<IValidator>();
            Mock.Get(validatorFromManifestFactory).Setup(x => x.GetValidator(manifest)).Returns(nonGenericValidator.Object);

            Assert.That(() => sut.GetValidator(builder), Is.SameAs(nonGenericValidator.Object));
        }

        [Test,AutoMoqData]
        public void GetValidatorNonGenericShouldReturnValidatorManifest([Frozen] IGetsValidatorFromManifest validatorFromManifestFactory,
                                                                        BaseValidatorFactory sut,
                                                                        [ManifestModel] ValidationManifest manifest,
                                                                        IValidator validator)
        {
            Mock.Get(validatorFromManifestFactory).Setup(x => x.GetValidator(manifest)).Returns(validator);

            Assert.That(() => sut.GetValidator(manifest), Is.SameAs(validator));
        }
    }
}