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
        public void GetValidatorFromBuilderTypeShouldReturnValidatorUsingManifestFromBuilderUsingResolver([Frozen] IResolvesServices resolver,
                                                                                                          [Frozen] IGetsManifestFromBuilder manifestFromBuilderProvider,
                                                                                                          [Frozen] IGetsValidatorFromManifest validatorFromManifestProvider,
                                                                                                          ValidatorFactory sut,
                                                                                                          [ManifestModel] ValidationManifest manifest,
                                                                                                          ValidatedObjectValidator builder)
        {
            var expectedValidatorMock = new Mock<IValidator<ValidatedObject>>();
            var expectedValidator = expectedValidatorMock.As<IValidator>().Object;

            Mock.Get(resolver).Setup(x => x.ResolveService<object>(typeof(ValidatedObjectValidator))).Returns(builder);
            Mock.Get(manifestFromBuilderProvider).Setup(x => x.GetManifest(builder)).Returns(manifest);
            Mock.Get(validatorFromManifestProvider).Setup(x => x.GetValidator(manifest)).Returns(expectedValidator);

            Assert.That(() => sut.GetValidator(typeof(ValidatedObjectValidator)), Is.SameAs(expectedValidator));
        }

        [Test,AutoMoqData]
        public void GetValidatorFromBuilderTypeShouldThrowArgExIfTypeIsNotAValidatorBuilder(ValidatorFactory sut)
        {
            Assert.That(() => sut.GetValidator(typeof(object)), Throws.ArgumentException);
        }

        [Test,AutoMoqData]
        public void GetValidatorFromBuilderTypeShouldThrowArgExIfTheValidatedTypeIsAmbiguous(ValidatorFactory sut)
        {
            Assert.That(() => sut.GetValidator(typeof(MultiTypeValidator)), Throws.ArgumentException);
        }

        [Test,AutoMoqData]
        public void GetValidatorFromBuilderShouldReturnValidatorUsingManifestFromBuilder([Frozen] IGetsManifestFromBuilder manifestFromBuilderProvider,
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
        public void GetValidatorFromManifestShouldReturnValidatorUsingManifest([Frozen] IGetsValidatorFromManifest validatorFromManifestProvider,
                                                                               ValidatorFactory sut,
                                                                               [ManifestModel] ValidationManifest manifest)
        {
            var expectedValidatorMock = new Mock<IValidator<ValidatedObject>>();
            var expectedValidator = expectedValidatorMock.As<IValidator>().Object;

            Mock.Get(validatorFromManifestProvider).Setup(x => x.GetValidator(manifest)).Returns(expectedValidator);

            Assert.That(() => sut.GetValidator(manifest), Is.SameAs(expectedValidator));
        }

        [Test,AutoMoqData]
        public void GetValidatorFromManifestModelShouldReturnValidatorUsingManifestCreatedFromModel([Frozen] IGetsValidatorFromManifest validatorFromManifestProvider,
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

        class MultiTypeValidator : IBuildsValidator<string>, IBuildsValidator<int>
        {
            public void ConfigureValidator(IConfiguresValidator<string> config)
                => throw new System.NotImplementedException();

            public void ConfigureValidator(IConfiguresValidator<int> config)
                => throw new System.NotImplementedException();
        }

        abstract class InvalidTypeValidator : IBuildsValidator<string>
        {
            public void ConfigureValidator(IConfiguresValidator<string> config)
                => throw new System.NotImplementedException();
        }
    }
}