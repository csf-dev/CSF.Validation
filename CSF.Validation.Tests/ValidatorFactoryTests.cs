using System;
using AutoFixture.NUnit3;
using CSF.Validation.Bootstrap;
using CSF.Validation.Manifest;
using CSF.Validation.ManifestModel;
using CSF.Validation.Stubs;
using CSF.Validation.ValidatorBuilding;
using Moq;
using NUnit.Framework;

namespace CSF.Validation
{
    [TestFixture,Parallelizable]
    public class ValidatorFactoryTests
    {
        #region GetValidator

        [Test,AutoMoqData]
        public void GetValidatorFromBuilderTypeShouldReturnValidatorUsingManifestFromBuilderUsingResolver([Frozen,AutofixtureServices] IServiceProvider serviceProvider,
                                                                                                          [Frozen] IResolvesServices resolver,
                                                                                                          [Frozen] IGetsManifestFromBuilder manifestFromBuilderProvider,
                                                                                                          [Frozen] IGetsValidatorFromManifest validatorFromManifestProvider,
                                                                                                          [Frozen] IGetsValidatedTypeForBuilderType builderTypeProvider,
                                                                                                          ValidatorFactory sut,
                                                                                                          [ManifestModel] ValidationManifest manifest,
                                                                                                          ValidatedObjectValidator builder)
        {
            var expectedValidatorMock = new Mock<IValidator<ValidatedObject>>();
            var expectedValidator = expectedValidatorMock.As<IValidator>().Object;

            Mock.Get(resolver).Setup(x => x.ResolveService<object>(typeof(ValidatedObjectValidator))).Returns(builder);
            Mock.Get(manifestFromBuilderProvider).Setup(x => x.GetManifest(builder)).Returns(manifest);
            Mock.Get(validatorFromManifestProvider).Setup(x => x.GetValidator(manifest)).Returns(expectedValidator);
            Mock.Get(builderTypeProvider).Setup(x => x.GetValidatedType(typeof(ValidatedObjectValidator))).Returns(typeof(ValidatedObject));

            Assert.That(() => sut.GetValidator(typeof(ValidatedObjectValidator)), Is.SameAs(expectedValidator));
        }

        [Test,AutoMoqData]
        public void GetValidatorFromBuilderShouldReturnValidatorUsingManifestFromBuilder([Frozen,AutofixtureServices] IServiceProvider serviceProvider,
                                                                                         [Frozen] IGetsManifestFromBuilder manifestFromBuilderProvider,
                                                                                         [Frozen] IGetsValidatorFromManifest validatorFromManifestProvider,
                                                                                         ValidatorFactory sut,
                                                                                         [ManifestModel] ValidationManifest manifest,
                                                                                         ValidatedObjectValidator builder)
        {
            var expectedValidatorMock = new Mock<IValidator<ValidatedObject>>();
            var expectedValidator = expectedValidatorMock.As<IValidator>().Object;

            Mock.Get(manifestFromBuilderProvider).Setup(x => x.GetManifest(builder)).Returns(manifest);
            Mock.Get(validatorFromManifestProvider).Setup(x => x.GetValidator(manifest)).Returns(expectedValidator);

            Assert.That(() => sut.GetValidator(builder), Is.SameAs(expectedValidator));
        }

        [Test,AutoMoqData]
        public void GetValidatorFromBuilderShouldThrowAneIfTheBuilderIsNull(ValidatorFactory sut)
        {
            Assert.That(() => sut.GetValidator((IBuildsValidator<object>) null), Throws.ArgumentNullException);
        }

        [Test,AutoMoqData]
        public void GetValidatorFromManifestShouldReturnValidatorUsingManifest([Frozen,AutofixtureServices] IServiceProvider serviceProvider,
                                                                               [Frozen] IGetsValidatorFromManifest validatorFromManifestProvider,
                                                                               ValidatorFactory sut,
                                                                               [ManifestModel] ValidationManifest manifest)
        {
            var expectedValidatorMock = new Mock<IValidator<ValidatedObject>>();
            var expectedValidator = expectedValidatorMock.As<IValidator>().Object;

            Mock.Get(validatorFromManifestProvider).Setup(x => x.GetValidator(manifest)).Returns(expectedValidator);

            Assert.That(() => sut.GetValidator(manifest), Is.SameAs(expectedValidator));
        }

        [Test,AutoMoqData]
        public void GetValidatorFromManifestModelShouldReturnValidatorUsingManifestCreatedFromModel([Frozen,AutofixtureServices] IServiceProvider serviceProvider,
                                                                                                    [Frozen] IGetsValidatorFromManifest validatorFromManifestProvider,
                                                                                                    [Frozen] IGetsValidationManifestFromModel manifestFromModelProvider,
                                                                                                    ValidatorFactory sut,
                                                                                                    [ManifestModel] ValidationManifest manifest,
                                                                                                    Value manifestModel,
                                                                                                    Type validatedType)
        {
            var expectedValidatorMock = new Mock<IValidator<ValidatedObject>>();
            var expectedValidator = expectedValidatorMock.As<IValidator>().Object;

            Mock.Get(manifestFromModelProvider).Setup(x => x.GetValidationManifest(manifestModel, validatedType)).Returns(manifest);
            Mock.Get(validatorFromManifestProvider).Setup(x => x.GetValidator(manifest)).Returns(expectedValidator);

            Assert.That(() => sut.GetValidator(manifestModel, validatedType), Is.SameAs(expectedValidator));
        }

        #endregion

        #region GetValidatorWithMessageSupport

        [Test,AutoMoqData]
        public void GetValidatorWithMessageSupportFromBuilderShouldReturnMessageEnrichingValidatorAdapter([Frozen,AutofixtureServices] IServiceProvider serviceProvider,
                                                                                                          [Frozen] IResolvesServices resolver,
                                                                                                          [Frozen] IGetsManifestFromBuilder manifestFromBuilderProvider,
                                                                                                          [Frozen] IGetsValidatorFromManifest validatorFromManifestProvider,
                                                                                                          [Frozen] IGetsValidatedTypeForBuilderType builderTypeProvider,
                                                                                                          ValidatorFactory sut,
                                                                                                          [ManifestModel] ValidationManifest manifest,
                                                                                                          ValidatedObjectValidator builder)
        {
            var expectedValidatorMock = new Mock<IValidator<ValidatedObject>>();
            var expectedValidatorNonGenericMock = expectedValidatorMock.As<IValidator>();
            var expectedValidator = expectedValidatorNonGenericMock.Object;

            expectedValidatorNonGenericMock.SetupGet(x => x.ValidatedType).Returns(typeof(ValidatedObject));
            Mock.Get(resolver).Setup(x => x.ResolveService<object>(typeof(ValidatedObjectValidator))).Returns(builder);
            Mock.Get(manifestFromBuilderProvider).Setup(x => x.GetManifest(builder)).Returns(manifest);
            Mock.Get(validatorFromManifestProvider).Setup(x => x.GetValidator(manifest)).Returns(expectedValidator);
            Mock.Get(builderTypeProvider).Setup(x => x.GetValidatedType(typeof(ValidatedObjectValidator))).Returns(typeof(ValidatedObject));

            Assert.That(() => sut.GetValidatorWithMessageSupport(typeof(ValidatedObjectValidator)),
                        Is.InstanceOf<MessageEnrichingValidatorAdapter<ValidatedObject>>());
        }

        [Test,AutoMoqData]
        public void GetValidatorWithMessageSupportFromBuilderShouldReturnMessageEnrichingValidatorAdapter([Frozen,AutofixtureServices] IServiceProvider serviceProvider,
                                                                                         [Frozen] IGetsManifestFromBuilder manifestFromBuilderProvider,
                                                                                         [Frozen] IGetsValidatorFromManifest validatorFromManifestProvider,
                                                                                         ValidatorFactory sut,
                                                                                         [ManifestModel] ValidationManifest manifest,
                                                                                         ValidatedObjectValidator builder)
        {
            var expectedValidatorMock = new Mock<IValidator<ValidatedObject>>();
            var expectedValidatorNonGenericMock = expectedValidatorMock.As<IValidator>();
            var expectedValidator = expectedValidatorNonGenericMock.Object;

            expectedValidatorNonGenericMock.SetupGet(x => x.ValidatedType).Returns(typeof(ValidatedObject));
            Mock.Get(manifestFromBuilderProvider).Setup(x => x.GetManifest(builder)).Returns(manifest);
            Mock.Get(validatorFromManifestProvider).Setup(x => x.GetValidator(manifest)).Returns(expectedValidator);

            Assert.That(() => sut.GetValidatorWithMessageSupport(builder),
                        Is.InstanceOf<MessageEnrichingValidatorAdapter<ValidatedObject>>());
        }

        [Test,AutoMoqData]
        public void GetValidatorWithMessageSupportFromManifestShouldReturnMessageEnrichingValidatorAdapter([Frozen,AutofixtureServices] IServiceProvider serviceProvider,
                                                                               [Frozen] IGetsValidatorFromManifest validatorFromManifestProvider,
                                                                               ValidatorFactory sut,
                                                                               [ManifestModel] ValidationManifest manifest)
        {
            var expectedValidatorMock = new Mock<IValidator<ValidatedObject>>();
            var expectedValidatorNonGenericMock = expectedValidatorMock.As<IValidator>();
            var expectedValidator = expectedValidatorNonGenericMock.Object;

            expectedValidatorNonGenericMock.SetupGet(x => x.ValidatedType).Returns(typeof(ValidatedObject));
            Mock.Get(validatorFromManifestProvider).Setup(x => x.GetValidator(manifest)).Returns(expectedValidator);

            Assert.That(() => sut.GetValidatorWithMessageSupport(manifest),
                        Is.InstanceOf<MessageEnrichingValidatorAdapter<ValidatedObject>>());
        }

        [Test,AutoMoqData]
        public void GetValidatorWithMessageSupportFromManifestModelShouldReturnMessageEnrichingValidatorAdapter([Frozen,AutofixtureServices] IServiceProvider serviceProvider,
                                                                                                    [Frozen] IGetsValidatorFromManifest validatorFromManifestProvider,
                                                                                                    [Frozen] IGetsValidationManifestFromModel manifestFromModelProvider,
                                                                                                    ValidatorFactory sut,
                                                                                                    [ManifestModel] ValidationManifest manifest,
                                                                                                    Value manifestModel,
                                                                                                    Type validatedType)
        {
            var expectedValidatorMock = new Mock<IValidator<ValidatedObject>>();
            var expectedValidatorNonGenericMock = expectedValidatorMock.As<IValidator>();
            var expectedValidator = expectedValidatorNonGenericMock.Object;

            expectedValidatorNonGenericMock.SetupGet(x => x.ValidatedType).Returns(typeof(ValidatedObject));
            Mock.Get(manifestFromModelProvider).Setup(x => x.GetValidationManifest(manifestModel, validatedType)).Returns(manifest);
            Mock.Get(validatorFromManifestProvider).Setup(x => x.GetValidator(manifest)).Returns(expectedValidator);

            Assert.That(() => sut.GetValidatorWithMessageSupport(manifestModel, validatedType),
                        Is.InstanceOf<MessageEnrichingValidatorAdapter<ValidatedObject>>());
        }

        #endregion
    }
}