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
    [TestFixture, NUnit.Framework.Parallelizable]
    public class ValidatorFactoryTests
    {
        #region GetValidator

        [Test,AutoMoqData]
        public void GetValidatorFromBuilderTypeShouldReturnValidatorUsingBaseValidatorWrappedInThrowingBehaviour([Frozen,AutofixtureServices] IServiceProvider serviceProvider,
                                                                                                                 [Frozen] IResolvesServices resolver,
                                                                                                                 [Frozen] IGetsBaseValidator baseValidatorFactory,
                                                                                                                 [Frozen] IWrapsValidatorWithExceptionBehaviour exceptionBehaviourWrapper,
                                                                                                                 [Frozen] IWrapsValidatorWithInstrumentationSupport instrumentationWrapper,
                                                                                                                 [Frozen] IGetsValidatedTypeForBuilderType builderTypeProvider,
                                                                                                                 [Frozen] IWrapsValidatorWithMessageSupport messageWrapper,
                                                                                                                 ValidatorFactory sut,
                                                                                                                 ValidatedObjectValidator builder,
                                                                                                                 IValidator<ValidatedObject> baseValidator)
        {
            var expectedValidatorMock = new Mock<IValidator<ValidatedObject>>();
            expectedValidatorMock.As<IValidator>();
            var expectedValidator = expectedValidatorMock.Object;
            var exceptionValidatorMock = new Mock<IValidator<ValidatedObject>>();
            exceptionValidatorMock.As<IValidator>();
            var exceptionValidator = exceptionValidatorMock.Object;
            var messageValidatorMock = new Mock<IValidator<ValidatedObject>>();
            messageValidatorMock.As<IValidator>();
            var messageValidator = messageValidatorMock.Object;

            Mock.Get(resolver).Setup(x => x.ResolveService<object>(typeof(ValidatedObjectValidator))).Returns(builder);
            Mock.Get(builderTypeProvider).Setup(x => x.GetValidatedType(typeof(ValidatedObjectValidator))).Returns(typeof(ValidatedObject));
            Mock.Get(baseValidatorFactory).Setup(x => x.GetValidator(builder)).Returns(baseValidator);
            Mock.Get(messageWrapper).Setup(x => x.GetValidatorWithMessageSupport(baseValidator)).Returns(messageValidator);
            Mock.Get(exceptionBehaviourWrapper).Setup(x => x.WrapValidator(messageValidator)).Returns(exceptionValidator);
            Mock.Get(instrumentationWrapper).Setup(x => x.WrapValidator(exceptionValidator)).Returns(expectedValidator);

            Assert.That(() => sut.GetValidator(typeof(ValidatedObjectValidator)), Is.SameAs(expectedValidator));
        }

        [Test,AutoMoqData]
        public void GetValidatorFromBuilderShouldReturnValidatorUsingBaseValidatorWrappedInThrowingBehaviour([Frozen,AutofixtureServices] IServiceProvider serviceProvider,
                                                                                                                 [Frozen] IGetsBaseValidator baseValidatorFactory,
                                                                                                                 [Frozen] IWrapsValidatorWithExceptionBehaviour exceptionBehaviourWrapper,
                                                                                                                 [Frozen] IWrapsValidatorWithMessageSupport messageWrapper,
                                                                                                                 [Frozen] IWrapsValidatorWithInstrumentationSupport instrumentationWrapper,
                                                                                                                 ValidatorFactory sut,
                                                                                                                 ValidatedObjectValidator builder,
                                                                                                                 IValidator<ValidatedObject> baseValidator)
        {
            var expectedValidatorMock = new Mock<IValidator<ValidatedObject>>();
            expectedValidatorMock.As<IValidator>();
            var expectedValidator = expectedValidatorMock.Object;
            var exceptionValidatorMock = new Mock<IValidator<ValidatedObject>>();
            exceptionValidatorMock.As<IValidator>();
            var exceptionValidator = exceptionValidatorMock.Object;
            var messageValidatorMock = new Mock<IValidator<ValidatedObject>>();
            messageValidatorMock.As<IValidator>();
            var messageValidator = messageValidatorMock.Object;

            Mock.Get(baseValidatorFactory).Setup(x => x.GetValidator(builder)).Returns(baseValidator);
            Mock.Get(messageWrapper).Setup(x => x.GetValidatorWithMessageSupport(baseValidator)).Returns(messageValidator);
            Mock.Get(exceptionBehaviourWrapper).Setup(x => x.WrapValidator(messageValidator)).Returns(exceptionValidator);
            Mock.Get(instrumentationWrapper).Setup(x => x.WrapValidator(exceptionValidator)).Returns(expectedValidator);

            Assert.That(() => sut.GetValidator(builder), Is.SameAs(expectedValidator));
        }

        [Test,AutoMoqData]
        public void GetValidatorFromManifestShouldReturnValidatorUsingManifestWrappedInThrowingBehaviour([Frozen,AutofixtureServices] IServiceProvider serviceProvider,
                                                                                                         [Frozen] IGetsBaseValidator baseValidatorFactory,
                                                                                                         [Frozen] IWrapsValidatorWithExceptionBehaviour exceptionBehaviourWrapper,
                                                                                                         [Frozen] IWrapsValidatorWithMessageSupport messageWrapper,
                                                                                                         [Frozen] IWrapsValidatorWithInstrumentationSupport instrumentationWrapper,
                                                                                                         ValidatorFactory sut,
                                                                                                         [ManifestModel] ValidationManifest manifest,
                                                                                                         IValidator baseValidator)
        {
            var expectedValidatorMock = new Mock<IValidator<ValidatedObject>>();
            var expectedValidator = expectedValidatorMock.As<IValidator>().Object;
            var messageValidatorMock = new Mock<IValidator<ValidatedObject>>();
            var messageValidator = messageValidatorMock.As<IValidator>().Object;
            var exceptionValidatorMock = new Mock<IValidator<ValidatedObject>>();
            var exceptionValidator = exceptionValidatorMock.As<IValidator>().Object;

            Mock.Get(baseValidatorFactory).Setup(x => x.GetValidator(manifest)).Returns(baseValidator);
            Mock.Get(messageWrapper).Setup(x => x.GetValidatorWithMessageSupport(baseValidator)).Returns(messageValidator);
            Mock.Get(exceptionBehaviourWrapper).Setup(x => x.WrapValidator(messageValidator)).Returns(exceptionValidator);
            Mock.Get(instrumentationWrapper).Setup(x => x.WrapValidator(exceptionValidator)).Returns(expectedValidator);

            Assert.That(() => sut.GetValidator(manifest), Is.SameAs(expectedValidator));
        }

        [Test,AutoMoqData]
        public void GetValidatorFromManifestModelShouldReturnValidatorUsingManifestCreatedFromModelWrappedInThrowingBehaviour([Frozen,AutofixtureServices] IServiceProvider serviceProvider,
                                                                                                                              [Frozen] IGetsBaseValidator baseValidatorFactory,
                                                                                                                              [Frozen] IWrapsValidatorWithExceptionBehaviour exceptionBehaviourWrapper,
                                                                                                                              [Frozen] IWrapsValidatorWithMessageSupport messageWrapper,
                                                                                                                              [Frozen] IWrapsValidatorWithInstrumentationSupport instrumentationWrapper,
                                                                                                                              [Frozen] IGetsValidationManifestFromModel manifestFromModelProvider,
                                                                                                                              ValidatorFactory sut,
                                                                                                                              [ManifestModel] Value manifestModel,
                                                                                                                              Type validatedType,
                                                                                                                              [ManifestModel] ValidationManifest manifest,
                                                                                                                              IValidator baseValidator)
        {
            var expectedValidatorMock = new Mock<IValidator<ValidatedObject>>();
            var expectedValidator = expectedValidatorMock.As<IValidator>().Object;
            var messageValidatorMock = new Mock<IValidator<ValidatedObject>>();
            var messageValidator = messageValidatorMock.As<IValidator>().Object;
            var exceptionValidatorMock = new Mock<IValidator<ValidatedObject>>();
            var exceptionValidator = exceptionValidatorMock.As<IValidator>().Object;

            Mock.Get(manifestFromModelProvider).Setup(x => x.GetValidationManifest(manifestModel, validatedType)).Returns(manifest);
            Mock.Get(baseValidatorFactory).Setup(x => x.GetValidator(manifest)).Returns(baseValidator);
            Mock.Get(messageWrapper).Setup(x => x.GetValidatorWithMessageSupport(baseValidator)).Returns(messageValidator);
            Mock.Get(exceptionBehaviourWrapper).Setup(x => x.WrapValidator(messageValidator)).Returns(exceptionValidator);
            Mock.Get(instrumentationWrapper).Setup(x => x.WrapValidator(exceptionValidator)).Returns(expectedValidator);

            Assert.That(() => sut.GetValidator(manifestModel, validatedType), Is.SameAs(expectedValidator));
        }

        #endregion
    }
}