using AutoFixture.NUnit3;
using CSF.Validation.Manifest;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace CSF.Validation
{
    [TestFixture,Parallelizable]
    public class ValidationOptionsResolverTests
    {
        [Test,AutoMoqData]
        public void GetResolvedValidationOptionsShouldReturnValuesFromSpecifiedOptionsIfTheyAreNonNull([Frozen] IOptions<ValidationOptions> opts,
                                                                                                       ValidationOptionsResolver sut,
                                                                                                       ValidationOptions specifiedOptions,
                                                                                                       ValidationOptions defaultOptions)
        {
            Mock.Get(opts).SetupGet(x => x.Value).Returns(defaultOptions);
            specifiedOptions.AccessorExceptionBehaviour = ValueAccessExceptionBehaviour.Throw;
            specifiedOptions.EnableMessageGeneration = true;
            specifiedOptions.RuleThrowingBehaviour = RuleThrowingBehaviour.OnFailure;

            var result = sut.GetResolvedValidationOptions(specifiedOptions);

            Assert.Multiple(() =>
            {
                Assert.That(result.AccessorExceptionBehaviour,
                            Is.EqualTo(ValueAccessExceptionBehaviour.Throw),
                            nameof(ValidationOptions.AccessorExceptionBehaviour));
                Assert.That(result.EnableMessageGeneration,
                            Is.EqualTo(true),
                            nameof(ValidationOptions.EnableMessageGeneration));
                Assert.That(result.RuleThrowingBehaviour,
                            Is.EqualTo(RuleThrowingBehaviour.OnFailure),
                            nameof(ValidationOptions.RuleThrowingBehaviour));
            });
        }
        [Test,AutoMoqData]
        public void GetResolvedValidationOptionsShouldReturnValuesFromDefaultOptionsIfSpecifiedOptionsAreNull([Frozen] IOptions<ValidationOptions> opts,
                                                                                                              ValidationOptionsResolver sut,
                                                                                                              ValidationOptions defaultOptions)
        {
            Mock.Get(opts).SetupGet(x => x.Value).Returns(defaultOptions);
            defaultOptions.AccessorExceptionBehaviour = ValueAccessExceptionBehaviour.Throw;
            defaultOptions.EnableMessageGeneration = true;
            defaultOptions.RuleThrowingBehaviour = RuleThrowingBehaviour.OnFailure;

            var result = sut.GetResolvedValidationOptions(null);

            Assert.Multiple(() =>
            {
                Assert.That(result.AccessorExceptionBehaviour,
                            Is.EqualTo(ValueAccessExceptionBehaviour.Throw),
                            nameof(ValidationOptions.AccessorExceptionBehaviour));
                Assert.That(result.EnableMessageGeneration,
                            Is.EqualTo(true),
                            nameof(ValidationOptions.EnableMessageGeneration));
                Assert.That(result.RuleThrowingBehaviour,
                            Is.EqualTo(RuleThrowingBehaviour.OnFailure),
                            nameof(ValidationOptions.RuleThrowingBehaviour));
            });
        }
        [Test,AutoMoqData]
        public void GetResolvedValidationOptionsShouldReturnHardcodedDefaultsIfSpecifiedAndDefaultOptionsAreNull([Frozen] IOptions<ValidationOptions> opts,
                                                                                                                 ValidationOptionsResolver sut,
                                                                                                                 ValidationOptions defaultOptions)
        {
            Mock.Get(opts).SetupGet(x => x.Value).Returns(defaultOptions);
            defaultOptions.AccessorExceptionBehaviour = null;
            defaultOptions.EnableMessageGeneration = null;
            defaultOptions.RuleThrowingBehaviour = null;

            var result = sut.GetResolvedValidationOptions(null);

            Assert.Multiple(() =>
            {
                Assert.That(result.AccessorExceptionBehaviour,
                            Is.EqualTo(ValueAccessExceptionBehaviour.TreatAsError),
                            nameof(ValidationOptions.AccessorExceptionBehaviour));
                Assert.That(result.EnableMessageGeneration,
                            Is.EqualTo(false),
                            nameof(ValidationOptions.EnableMessageGeneration));
                Assert.That(result.RuleThrowingBehaviour,
                            Is.EqualTo(RuleThrowingBehaviour.OnError),
                            nameof(ValidationOptions.RuleThrowingBehaviour));
            });
        }
    }
}