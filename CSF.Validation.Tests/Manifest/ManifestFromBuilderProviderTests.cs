using AutoFixture.NUnit3;
using CSF.Validation.ValidatorBuilding;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.Manifest
{
    [TestFixture,Parallelizable]
    public class ManifestFromBuilderProviderTests
    {
        [Test,AutoMoqData]
        public void GetManifestShouldReturnManifestCreatedFromBuilderAndBuilderCustomisation([Frozen] IGetsValidatorBuilder builderFactory,
                                                                                             ManifestFromBuilderProvider sut,
                                                                                             IBuildsValidator<string> builder,
                                                                                             IValidatorBuilder<string> validatorBuilder,
                                                                                             [ManifestModel] ValidationManifest manifest)
        {
            Mock.Get(builderFactory)
                .Setup(x => x.GetValidatorBuilder<string>(It.IsAny<ValidatorBuilderContext>()))
                .Returns(validatorBuilder);
            Mock.Get(validatorBuilder)
                .Setup(x => x.GetManifest())
                .Returns(manifest);

            var result = sut.GetManifest(builder);

            Assert.That(result, Is.SameAs(manifest));
            Mock.Get(builder).Verify(x => x.ConfigureValidator(validatorBuilder), Times.Once);
        }
    }
}