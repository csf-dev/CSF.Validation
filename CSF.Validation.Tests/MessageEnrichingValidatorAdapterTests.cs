using System.Threading.Tasks;
using AutoFixture.NUnit3;
using CSF.Validation.Messages;
using CSF.Validation.Rules;
using Moq;
using NUnit.Framework;

namespace CSF.Validation
{
    [TestFixture,Parallelizable]
    public class MessageEnrichingValidatorAdapterTests
    {
        [Test,AutoMoqData]
        public void ValidateAsyncShouldUseTheWrappedValidatorThenReturnTheResultEnrichedWithMessages([Frozen] IValidator<object> validator,
                                                                                                     [Frozen] IAddsFailureMessagesToResult failureMessageEnricher,
                                                                                                     MessageEnrichingValidatorAdapter<object> sut,
                                                                                                     [RuleResult] ValidationResult originalResult,
                                                                                                     [RuleResult] ValidationResultWithMessages resultWithMessages,
                                                                                                     object validatedObject)
        {
            Mock.Get(validator).Setup(x => x.ValidateAsync(validatedObject, default, default)).Returns(Task.FromResult(originalResult));
            Mock.Get(failureMessageEnricher).Setup(x => x.GetResultWithMessagesAsync(originalResult, default)).Returns(Task.FromResult(resultWithMessages));

            Assert.That(async () => await sut.ValidateAsync(validatedObject), Is.SameAs(resultWithMessages));
        }
    }
}