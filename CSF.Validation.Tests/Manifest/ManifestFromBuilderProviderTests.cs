using AutoFixture.NUnit3;
using CSF.Validation.ValidatorBuilding;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.Manifest
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class ManifestFromBuilderProviderTests
    {
        [Test,AutoMoqData]
        public void GetManifestShouldReturnManifestCreatedFromBuilderAndBuilderCustomisation([Frozen] IGetsValidatorBuilder builderFactory,
                                                                                             ManifestFromBuilderProvider sut,
                                                                                             IBuildsValidator<string> builder,
                                                                                             Mock<IConfiguresValidator<string>> validatorBuilder,
                                                                                             [ManifestModel] ValidatorBuilderContext context)
        {
            Mock.Get(builderFactory)
                .Setup(x => x.GetValidatorBuilder<string>(It.IsAny<ValidatorBuilderContext>()))
                .Returns(() => validatorBuilder.Object);
            validatorBuilder
                .As<IHasValidationBuilderContext>()
                .SetupGet(x => x.Context)
                .Returns(context);

            Assert.That(() => sut.GetManifest(builder)?.RootValue, Is.SameAs(context.ManifestValue));
        }

        [Test,AutoMoqData]
        public void GetManifestShouldConfigureTheValidatorUsingTheSpecifiedBuilder([Frozen] IGetsValidatorBuilder builderFactory,
                                                                                             ManifestFromBuilderProvider sut,
                                                                                             IBuildsValidator<string> builder,
                                                                                             Mock<IConfiguresValidator<string>> validatorBuilder,
                                                                                             [ManifestModel] ValidatorBuilderContext context)
        {
            Mock.Get(builderFactory)
                .Setup(x => x.GetValidatorBuilder<string>(It.IsAny<ValidatorBuilderContext>()))
                .Returns(() => validatorBuilder.Object);
            validatorBuilder
                .As<IHasValidationBuilderContext>()
                .SetupGet(x => x.Context)
                .Returns(context);

            sut.GetManifest(builder);

            Mock.Get(builder).Verify(x => x.ConfigureValidator(validatorBuilder.Object), Times.Once);
        }
    }
}