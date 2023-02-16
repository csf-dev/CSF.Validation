using System.Threading.Tasks;
using AutoFixture.NUnit3;
using CSF.Validation.Rules;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.Messages
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class SingleRuleValidationResultMessageEnricherTests
    {
        [Test,AutoMoqData]
        public void GetRuleResultWithMessageAsyncShouldReturnTheSameResultIfTheOutcomeIsPass(SingleRuleValidationResultMessageEnricher sut,
                                                                                             [RuleResult(Outcome = RuleOutcome.Passed)] ValidationRuleResult result)
        {
            Assert.That(async () => await sut.GetRuleResultWithMessageAsync(result, default), Is.SameAs(result));
        }

        [Test,AutoMoqData]
        public void GetRuleResultWithMessageAsyncShouldReturnTheSameResultIfThereIsNoMessageProvider([Frozen] IGetsFailureMessageProvider messageProviderFactory,
                                                                                                     SingleRuleValidationResultMessageEnricher sut,
                                                                                                     [RuleResult(Outcome = RuleOutcome.Failed)] ValidationRuleResult result)
        {
            Mock.Get(messageProviderFactory).Setup(x => x.GetProvider(result)).Returns(() => null);
            Assert.That(async () => await sut.GetRuleResultWithMessageAsync(result, default), Is.SameAs(result));
        }

        [Test,AutoMoqData]
        public void GetRuleResultWithMessageAsyncShouldReturnTheSameResultIfThereIsNoMessageProvider([Frozen] IGetsFailureMessageProvider messageProviderFactory,
                                                                                                     SingleRuleValidationResultMessageEnricher sut,
                                                                                                     IGetsFailureMessage messageProvider,
                                                                                                     string message,
                                                                                                     [RuleResult(Outcome = RuleOutcome.Failed)] ValidationRuleResult result)
        {
            Mock.Get(messageProviderFactory).Setup(x => x.GetProvider(result)).Returns(messageProvider);
            Mock.Get(messageProvider).Setup(x => x.GetFailureMessageAsync(result, default)).Returns(Task.FromResult(message));
            Assert.That(async () => await sut.GetRuleResultWithMessageAsync(result, default), Has.Property(nameof(ValidationRuleResult.Message)).EqualTo(message));
        }
    }
}