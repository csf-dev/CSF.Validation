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
        public void GetValidatorFromBuilderTypeShouldReturnValidatorUsingBaseValidatorWrappedInThrowingBehaviour([Frozen,AutofixtureServices] IServiceProvider serviceProvider,
                                                                                                                 [Frozen] IResolvesServices resolver,
                                                                                                                 [Frozen] IGetsBaseValidator baseValidatorFactory,
                                                                                                                 [Frozen] IWrapsValidatorWithExceptionBehaviour exceptionBehaviourWrapper,
                                                                                                                 [Frozen] IGetsValidatedTypeForBuilderType builderTypeProvider,
                                                                                                                 ValidatorFactory sut,
                                                                                                                 ValidatedObjectValidator builder,
                                                                                                                 IValidator<ValidatedObject> baseValidator)
        {
            var expectedValidatorMock = new Mock<IValidator<ValidatedObject>>();
            expectedValidatorMock.As<IValidator>();
            var expectedValidator = expectedValidatorMock.Object;

            Mock.Get(resolver).Setup(x => x.ResolveService<object>(typeof(ValidatedObjectValidator))).Returns(builder);
            Mock.Get(builderTypeProvider).Setup(x => x.GetValidatedType(typeof(ValidatedObjectValidator))).Returns(typeof(ValidatedObject));
            Mock.Get(baseValidatorFactory).Setup(x => x.GetValidator(builder)).Returns(baseValidator);
            Mock.Get(exceptionBehaviourWrapper).Setup(x => x.WrapValidator(baseValidator)).Returns(expectedValidator);

            Assert.That(() => sut.GetValidator(typeof(ValidatedObjectValidator)), Is.SameAs(expectedValidator));
        }

        [Test,AutoMoqData]
        public void GetValidatorFromBuilderShouldReturnValidatorUsingBaseValidatorWrappedInThrowingBehaviour([Frozen,AutofixtureServices] IServiceProvider serviceProvider,
                                                                                                                 [Frozen] IGetsBaseValidator baseValidatorFactory,
                                                                                                                 [Frozen] IWrapsValidatorWithExceptionBehaviour exceptionBehaviourWrapper,
                                                                                                                 ValidatorFactory sut,
                                                                                                                 ValidatedObjectValidator builder,
                                                                                                                 IValidator<ValidatedObject> baseValidator)
        {
            var expectedValidatorMock = new Mock<IValidator<ValidatedObject>>();
            expectedValidatorMock.As<IValidator>();
            var expectedValidator = expectedValidatorMock.Object;

            Mock.Get(baseValidatorFactory).Setup(x => x.GetValidator(builder)).Returns(baseValidator);
            Mock.Get(exceptionBehaviourWrapper).Setup(x => x.WrapValidator(baseValidator)).Returns(expectedValidator);

            Assert.That(() => sut.GetValidator(builder), Is.SameAs(expectedValidator));
        }

        [Test,AutoMoqData]
        public void GetValidatorFromManifestShouldReturnValidatorUsingManifestWrappedInThrowingBehaviour([Frozen,AutofixtureServices] IServiceProvider serviceProvider,
                                                                                                         [Frozen] IGetsBaseValidator baseValidatorFactory,
                                                                                                         [Frozen] IWrapsValidatorWithExceptionBehaviour exceptionBehaviourWrapper,
                                                                                                         ValidatorFactory sut,
                                                                                                         [ManifestModel] ValidationManifest manifest,
                                                                                                         IValidator baseValidator)
        {
            var expectedValidatorMock = new Mock<IValidator<ValidatedObject>>();
            var expectedValidator = expectedValidatorMock.As<IValidator>().Object;

            Mock.Get(baseValidatorFactory).Setup(x => x.GetValidator(manifest)).Returns(baseValidator);
            Mock.Get(exceptionBehaviourWrapper).Setup(x => x.WrapValidator(baseValidator)).Returns(expectedValidator);

            Assert.That(() => sut.GetValidator(manifest), Is.SameAs(expectedValidator));
        }

        [Test,AutoMoqData]
        public void GetValidatorFromManifestModelShouldReturnValidatorUsingManifestCreatedFromModelWrappedInThrowingBehaviour([Frozen,AutofixtureServices] IServiceProvider serviceProvider,
                                                                                                                              [Frozen] IGetsBaseValidator baseValidatorFactory,
                                                                                                                              [Frozen] IWrapsValidatorWithExceptionBehaviour exceptionBehaviourWrapper,
                                                                                                                              [Frozen] IGetsValidationManifestFromModel manifestFromModelProvider,
                                                                                                                              ValidatorFactory sut,
                                                                                                                              [ManifestModel] Value manifestModel,
                                                                                                                              Type validatedType,
                                                                                                                              [ManifestModel] ValidationManifest manifest,
                                                                                                                              IValidator baseValidator)
        {
            var expectedValidatorMock = new Mock<IValidator<ValidatedObject>>();
            var expectedValidator = expectedValidatorMock.As<IValidator>().Object;

            Mock.Get(manifestFromModelProvider).Setup(x => x.GetValidationManifest(manifestModel, validatedType)).Returns(manifest);
            Mock.Get(baseValidatorFactory).Setup(x => x.GetValidator(manifest)).Returns(baseValidator);
            Mock.Get(exceptionBehaviourWrapper).Setup(x => x.WrapValidator(baseValidator)).Returns(expectedValidator);

            Assert.That(() => sut.GetValidator(manifestModel, validatedType), Is.SameAs(expectedValidator));
        }

        #endregion
    }
}