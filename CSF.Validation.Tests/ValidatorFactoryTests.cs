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
        [Test,AutoMoqData]
        public void GetValidatorFromBuilderTypeShouldReturnValidatorUsingManifestFromBuilderUsingResolver([Frozen] IServiceProvider serviceProvider,
                                                                                                          IResolvesServices resolver,
                                                                                                          IGetsManifestFromBuilder manifestFromBuilderProvider,
                                                                                                          IGetsValidatorFromManifest validatorFromManifestProvider,
                                                                                                          IGetsValidatedTypeForBuilderType builderTypeProvider,
                                                                                                          ValidatorFactory sut,
                                                                                                          [ManifestModel] ValidationManifest manifest,
                                                                                                          ValidatedObjectValidator builder)
        {
            var expectedValidatorMock = new Mock<IValidator<ValidatedObject>>();
            var expectedValidator = expectedValidatorMock.As<IValidator>().Object;

            Mock.Get(serviceProvider).Setup(x => x.GetService(typeof(IResolvesServices))).Returns(resolver);
            Mock.Get(serviceProvider).Setup(x => x.GetService(typeof(IGetsManifestFromBuilder))).Returns(manifestFromBuilderProvider);
            Mock.Get(serviceProvider).Setup(x => x.GetService(typeof(IGetsValidatorFromManifest))).Returns(validatorFromManifestProvider);
            Mock.Get(serviceProvider).Setup(x => x.GetService(typeof(IGetsValidatedTypeForBuilderType))).Returns(builderTypeProvider);
            Mock.Get(resolver).Setup(x => x.ResolveService<object>(typeof(ValidatedObjectValidator))).Returns(builder);
            Mock.Get(manifestFromBuilderProvider).Setup(x => x.GetManifest(builder)).Returns(manifest);
            Mock.Get(validatorFromManifestProvider).Setup(x => x.GetValidator(manifest)).Returns(expectedValidator);
            Mock.Get(builderTypeProvider).Setup(x => x.GetValidatedType(typeof(ValidatedObjectValidator))).Returns(typeof(ValidatedObject));

            Assert.That(() => sut.GetValidator(typeof(ValidatedObjectValidator)), Is.SameAs(expectedValidator));
        }

        [Test,AutoMoqData]
        public void GetValidatorFromBuilderShouldReturnValidatorUsingManifestFromBuilder([Frozen] IServiceProvider serviceProvider,
                                                                                         IGetsManifestFromBuilder manifestFromBuilderProvider,
                                                                                         IGetsValidatorFromManifest validatorFromManifestProvider,
                                                                                         ValidatorFactory sut,
                                                                                         [ManifestModel] ValidationManifest manifest,
                                                                                         ValidatedObjectValidator builder)
        {
            var expectedValidatorMock = new Mock<IValidator<ValidatedObject>>();
            var expectedValidator = expectedValidatorMock.As<IValidator>().Object;

            Mock.Get(serviceProvider).Setup(x => x.GetService(typeof(IGetsManifestFromBuilder))).Returns(manifestFromBuilderProvider);
            Mock.Get(serviceProvider).Setup(x => x.GetService(typeof(IGetsValidatorFromManifest))).Returns(validatorFromManifestProvider);
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
        public void GetValidatorFromManifestShouldReturnValidatorUsingManifest([Frozen] IServiceProvider serviceProvider,
                                                                               IGetsValidatorFromManifest validatorFromManifestProvider,
                                                                               ValidatorFactory sut,
                                                                               [ManifestModel] ValidationManifest manifest)
        {
            var expectedValidatorMock = new Mock<IValidator<ValidatedObject>>();
            var expectedValidator = expectedValidatorMock.As<IValidator>().Object;

            Mock.Get(serviceProvider).Setup(x => x.GetService(typeof(IGetsValidatorFromManifest))).Returns(validatorFromManifestProvider);
            Mock.Get(validatorFromManifestProvider).Setup(x => x.GetValidator(manifest)).Returns(expectedValidator);

            Assert.That(() => sut.GetValidator(manifest), Is.SameAs(expectedValidator));
        }

        [Test,AutoMoqData]
        public void GetValidatorFromManifestModelShouldReturnValidatorUsingManifestCreatedFromModel([Frozen] IServiceProvider serviceProvider,
                                                                                                    IGetsValidatorFromManifest validatorFromManifestProvider,
                                                                                                    IGetsValidationManifestFromModel manifestFromModelProvider,
                                                                                                    ValidatorFactory sut,
                                                                                                    [ManifestModel] ValidationManifest manifest,
                                                                                                    Value manifestModel,
                                                                                                    Type validatedType)
        {
            var expectedValidatorMock = new Mock<IValidator<ValidatedObject>>();
            var expectedValidator = expectedValidatorMock.As<IValidator>().Object;

            Mock.Get(serviceProvider).Setup(x => x.GetService(typeof(IGetsValidatorFromManifest))).Returns(validatorFromManifestProvider);
            Mock.Get(serviceProvider).Setup(x => x.GetService(typeof(IGetsValidationManifestFromModel))).Returns(manifestFromModelProvider);
            Mock.Get(manifestFromModelProvider).Setup(x => x.GetValidationManifest(manifestModel, validatedType)).Returns(manifest);
            Mock.Get(validatorFromManifestProvider).Setup(x => x.GetValidator(manifest)).Returns(expectedValidator);

            Assert.That(() => sut.GetValidator(manifestModel, validatedType), Is.SameAs(expectedValidator));
        }
    }
}