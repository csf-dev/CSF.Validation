using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using CSF.Validation.Messages;
using CSF.Validation.Rules;
using Moq;
using NUnit.Framework;

namespace CSF.Validation
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class MessageEnrichingValidatorDecoratorTests
    {
        [Test,AutoMoqData]
        public void ValidateAsyncShouldUseTheWrappedValidatorThenReturnTheResultEnrichedWithMessagesIfOptionsEnableIt([Frozen] IValidator<object> validator,
                                                                                                     [Frozen] IAddsFailureMessagesToResult failureMessageEnricher,
                                                                                                     MessageEnrichingValidatorDecorator<object> sut,
                                                                                                     [RuleResult] ValidationResult<object> originalResult,
                                                                                                     [RuleResult] ValidationResult<object> resultWithMessages,
                                                                                                     object validatedObject,
                                                                                                     ValidationOptions options)
        {
            options.EnableMessageGeneration = true;
            Mock.Get(validator).Setup(x => x.ValidateAsync(validatedObject, options, default)).Returns(Task.FromResult<IQueryableValidationResult<object>>(originalResult));
            Mock.Get(failureMessageEnricher).Setup(x => x.GetResultWithMessagesAsync(originalResult, default)).Returns(Task.FromResult<IQueryableValidationResult<object>>(resultWithMessages));

            Assert.That(async () => await sut.ValidateAsync(validatedObject, options), Is.SameAs(resultWithMessages));
        }

        [Test,AutoMoqData]
        public void ValidateAsyncShouldReturnTheWrappedValidatorResultIfOptionsDisableMessageGeneration([Frozen] IValidator<object> validator,
                                                                                                     [Frozen] IAddsFailureMessagesToResult failureMessageEnricher,
                                                                                                     MessageEnrichingValidatorDecorator<object> sut,
                                                                                                     [RuleResult] ValidationResult<object> originalResult,
                                                                                                     [RuleResult] ValidationResult<object> resultWithMessages,
                                                                                                     object validatedObject,
                                                                                                     ValidationOptions options)
        {
            options.EnableMessageGeneration = false;
            Mock.Get(validator).Setup(x => x.ValidateAsync(validatedObject, options, default)).Returns(Task.FromResult<IQueryableValidationResult<object>>(originalResult));

            Assert.That(async () => await sut.ValidateAsync(validatedObject, options), Is.SameAs(originalResult));
            Mock.Get(failureMessageEnricher).Verify(x => x.GetResultWithMessagesAsync<object>(It.IsAny<IQueryableValidationResult<object>>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test,AutoMoqData]
        public void ValidateAsyncShouldReturnTheWrappedValidatorResultIfOptionsAreOmitted([Frozen] IValidator<object> validator,
                                                                                                     [Frozen] IAddsFailureMessagesToResult failureMessageEnricher,
                                                                                                     MessageEnrichingValidatorDecorator<object> sut,
                                                                                                     [RuleResult] ValidationResult<object> originalResult,
                                                                                                     [RuleResult] ValidationResult<object> resultWithMessages,
                                                                                                     object validatedObject)
        {
            Mock.Get(validator).Setup(x => x.ValidateAsync(validatedObject, default, default)).Returns(Task.FromResult<IQueryableValidationResult<object>>(originalResult));

            Assert.That(async () => await sut.ValidateAsync(validatedObject), Is.SameAs(originalResult));
            Mock.Get(failureMessageEnricher).Verify(x => x.GetResultWithMessagesAsync(It.IsAny<IQueryableValidationResult<object>>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test,AutoMoqData]
        public void ValidateAsyncNonGenericShouldUseTheWrappedValidatorThenReturnTheResultEnrichedWithMessagesIfOptionsEnableIt([Frozen] IValidator<object> validator,
                                                                                                     [Frozen] IAddsFailureMessagesToResult failureMessageEnricher,
                                                                                                     MessageEnrichingValidatorDecorator<object> sut,
                                                                                                     [RuleResult] ValidationResult<object> originalResult,
                                                                                                     [RuleResult] ValidationResult<object> resultWithMessages,
                                                                                                     object validatedObject,
                                                                                                     ValidationOptions options)
        {
            options.EnableMessageGeneration = true;
            Mock.Get(validator).Setup(x => x.ValidateAsync(validatedObject, options, default)).Returns(Task.FromResult<IQueryableValidationResult<object>>(originalResult));
            Mock.Get(failureMessageEnricher).Setup(x => x.GetResultWithMessagesAsync(originalResult, default)).Returns(Task.FromResult<IQueryableValidationResult<object>>(resultWithMessages));

            Assert.That(async () => await ((IValidator) sut).ValidateAsync(validatedObject, options), Is.SameAs(resultWithMessages));
        }
    }
}