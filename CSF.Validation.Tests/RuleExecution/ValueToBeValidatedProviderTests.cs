using System;
using AutoFixture.NUnit3;
using CSF.Validation.Manifest;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.RuleExecution
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class ValueToBeValidatedProviderTests
    {
        [Test,AutoMoqData]
        public void GetValueToBeValidatedShouldReturnSuccessResponseIfTheValueIsReadable(ValueToBeValidatedProvider sut,
                                                                                 [ManifestModel] ManifestValue manifestValue,
                                                                                 object parentValue,
                                                                                 ResolvedValidationOptions validationOptions,
                                                                                 object value)
        {
            manifestValue.AccessorFromParent = obj => value;
            Assert.That(() => sut.GetValueToBeValidated(manifestValue, parentValue, validationOptions), Is.InstanceOf<SuccessfulGetValueToBeValidatedResponse>());
        }

        [Test,AutoMoqData]
        public void GetValueToBeValidatedShouldExposeTheCorrectValueWhenItIsReadable(ValueToBeValidatedProvider sut,
                                                                                        [ManifestModel] ManifestValue manifestValue,
                                                                                        object parentValue,
                                                                                        ResolvedValidationOptions validationOptions,
                                                                                        object expected)
        {
            manifestValue.AccessorFromParent = obj => expected;
            Assert.That(() => sut.GetValueToBeValidated(manifestValue, parentValue, validationOptions),
                        Has.Property(nameof(SuccessfulGetValueToBeValidatedResponse.Value)).EqualTo(expected));
        }

        [Test,AutoMoqData]
        public void GetValueToBeValidatedShouldReturnIgnoredResultIfTheParentValueIsNull(ValueToBeValidatedProvider sut,
                                                                                         [ManifestModel] ManifestValue manifestValue,
                                                                                         ResolvedValidationOptions validationOptions)
        {
            Assert.That(() => sut.GetValueToBeValidated(manifestValue, null, validationOptions), Is.InstanceOf<IgnoredGetValueToBeValidatedResponse>());
        }

        [Test,AutoMoqData]
        public void GetValueToBeValidatedShouldThrowIfTheAccessorThrowsAndExceptionBehaviourIsThrow([Frozen] IGetsAccessorExceptionBehaviour behaviourProvider,
                                                                                                    ValueToBeValidatedProvider sut,
                                                                                                    [ManifestModel] ManifestValue manifestValue,
                                                                                                    object parentValue,
                                                                                                    ResolvedValidationOptions validationOptions,
                                                                                                    Exception exception)
        {
            Mock.Get(behaviourProvider).Setup(x => x.GetBehaviour(manifestValue, validationOptions)).Returns(ValueAccessExceptionBehaviour.Throw);
            manifestValue.AccessorFromParent = obj => throw exception;
            Assert.That(() => sut.GetValueToBeValidated(manifestValue, parentValue, validationOptions),
                        Throws.InstanceOf<ValidationException>().And.InnerException.SameAs(exception));
        }

        [Test,AutoMoqData]
        public void GetValueToBeValidatedShouldReturnIgnoredResultIfAccessorThrowsAndExceptionBehaviourIsIgnore([Frozen] IGetsAccessorExceptionBehaviour behaviourProvider,
                                                                                                                ValueToBeValidatedProvider sut,
                                                                                                                [ManifestModel] ManifestValue manifestValue,
                                                                                                                object parentValue,
                                                                                                                ResolvedValidationOptions validationOptions,
                                                                                                                Exception exception)
        {
            Mock.Get(behaviourProvider).Setup(x => x.GetBehaviour(manifestValue, validationOptions)).Returns(ValueAccessExceptionBehaviour.Ignore);
            manifestValue.AccessorFromParent = obj => throw exception;
            Assert.That(() => sut.GetValueToBeValidated(manifestValue, parentValue, validationOptions), Is.InstanceOf<IgnoredGetValueToBeValidatedResponse>());
        }

        [Test,AutoMoqData]
        public void GetValueToBeValidatedShouldReturnErrorResultIfAccessorThrowsAndExceptionBehaviourIsError([Frozen] IGetsAccessorExceptionBehaviour behaviourProvider,
                                                                                                             ValueToBeValidatedProvider sut,
                                                                                                             [ManifestModel] ManifestValue manifestValue,
                                                                                                             object parentValue,
                                                                                                             ResolvedValidationOptions validationOptions,
                                                                                                             Exception exception)
        {
            Mock.Get(behaviourProvider).Setup(x => x.GetBehaviour(manifestValue, validationOptions)).Returns(ValueAccessExceptionBehaviour.TreatAsError);
            manifestValue.AccessorFromParent = obj => throw exception;
            Assert.That(() => sut.GetValueToBeValidated(manifestValue, parentValue, validationOptions), Is.InstanceOf<ErrorGetValueToBeValidatedResponse>());
        }
    }
}