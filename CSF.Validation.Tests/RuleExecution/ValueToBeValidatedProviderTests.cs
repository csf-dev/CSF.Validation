using System;
using CSF.Validation.Manifest;
using NUnit.Framework;

namespace CSF.Validation.RuleExecution
{
    [TestFixture,Parallelizable]
    public class ValueToBeValidatedProviderTests
    {
        [Test,AutoMoqData]
        public void TryGetValueToBeValidatedShouldReturnTrueIfTheValueIsReadable(ValueToBeValidatedProvider sut,
                                                                                 [ManifestModel] ManifestValue manifestValue,
                                                                                 object parentValue,
                                                                                 ValidationOptions validationOptions,
                                                                                 object value)
        {
            manifestValue.AccessorFromParent = obj => value;
            Assert.That(() => sut.TryGetValueToBeValidated(manifestValue, parentValue, validationOptions, out _), Is.True);
        }

        [Test,AutoMoqData]
        public void TryGetValueToBeValidatedShouldExposeTheCorrectValueWhenItIsReadable(ValueToBeValidatedProvider sut,
                                                                                        [ManifestModel] ManifestValue manifestValue,
                                                                                        object parentValue,
                                                                                        ValidationOptions validationOptions,
                                                                                        object expected)
        {
            manifestValue.AccessorFromParent = obj => expected;
            sut.TryGetValueToBeValidated(manifestValue, parentValue, validationOptions, out var actual);
            Assert.That(actual, Is.SameAs(expected));
        }

        [Test,AutoMoqData]
        public void TryGetValueToBeValidatedShouldReturnFalseIfTheParentValueIsNull(ValueToBeValidatedProvider sut,
                                                                                    [ManifestModel] ManifestValue manifestValue,
                                                                                    ValidationOptions validationOptions)
        {
            Assert.That(() => sut.TryGetValueToBeValidated(manifestValue, null, validationOptions, out _), Is.False);
        }

        [Test,AutoMoqData]
        public void TryGetValueToBeValidatedShouldThrowIfTheAccessorThrowsAndIgnoreExceptionsIsDisabled(ValueToBeValidatedProvider sut,
                                                                                 [ManifestModel] ManifestValue manifestValue,
                                                                                 object parentValue,
                                                                                 ValidationOptions validationOptions,
                                                                                 Exception exception)
        {
            manifestValue.AccessorExceptionBehaviour = false;
            validationOptions.AccessorExceptionBehaviour = false;
            manifestValue.AccessorFromParent = obj => throw exception;
            Assert.That(() => sut.TryGetValueToBeValidated(manifestValue, parentValue, validationOptions, out _),
                        Throws.InstanceOf<ValidationException>().And.InnerException.SameAs(exception));
        }

        [Test,AutoMoqData]
        public void TryGetValueToBeValidatedShouldReturnFalseIfTheAccessorThrowsAndIgnoreExceptionsIsEnabledForTheManifest(ValueToBeValidatedProvider sut,
                                                                                                                           [ManifestModel] ManifestValue manifestValue,
                                                                                                                           object parentValue,
                                                                                                                           ValidationOptions validationOptions,
                                                                                                                           Exception exception)
        {
            manifestValue.AccessorExceptionBehaviour = true;
            validationOptions.AccessorExceptionBehaviour = false;
            manifestValue.AccessorFromParent = obj => throw exception;
            Assert.That(() => sut.TryGetValueToBeValidated(manifestValue, parentValue, validationOptions, out _), Is.False);
        }

        [Test,AutoMoqData]
        public void TryGetValueToBeValidatedShouldReturnFalseIfTheAccessorThrowsAndIgnoreExceptionsIsEnabledViaTheOptions(ValueToBeValidatedProvider sut,
                                                                                                                          [ManifestModel] ManifestValue manifestValue,
                                                                                                                          object parentValue,
                                                                                                                          ValidationOptions validationOptions,
                                                                                                                          Exception exception)
        {
            manifestValue.AccessorExceptionBehaviour = false;
            validationOptions.AccessorExceptionBehaviour = true;
            manifestValue.AccessorFromParent = obj => throw exception;
            Assert.That(() => sut.TryGetValueToBeValidated(manifestValue, parentValue, validationOptions, out _), Is.False);
        }
    }
}