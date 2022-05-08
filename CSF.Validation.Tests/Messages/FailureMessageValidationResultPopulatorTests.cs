using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using CSF.Validation.Rules;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.Messages
{
    [TestFixture,Parallelizable]
    public class FailureMessageValidationResultPopulatorTests
    {
        [Test,AutoMoqData]
        public void GetResultWithMessagesAsyncShouldAddMessagesToResultsSkippingResultsWithNoMessage([Frozen] IGetsFailureMessageProvider messageProviderFactory,
                                                                                                     FailureMessageValidationResultPopulator sut,
                                                                                                     [RuleResult] ValidationRuleResult ruleResult1,
                                                                                                     [RuleResult] ValidationRuleResult ruleResult2,
                                                                                                     [RuleResult] ValidationRuleResult ruleResult3,
                                                                                                     IGetsFailureMessage messageProvider1,
                                                                                                     IGetsFailureMessage messageProvider3,
                                                                                                     string message1,
                                                                                                     string message3)
        {
            var validationResult = new ValidationResult(new[] { ruleResult1, ruleResult2, ruleResult3 });
            Mock.Get(messageProviderFactory).Setup(x => x.GetProvider(ruleResult1)).Returns(messageProvider1);
            Mock.Get(messageProviderFactory).Setup(x => x.GetProvider(ruleResult2)).Returns(() => null);
            Mock.Get(messageProviderFactory).Setup(x => x.GetProvider(ruleResult3)).Returns(messageProvider3);
            Mock.Get(messageProvider1)
                .Setup(x => x.GetFailureMessageAsync(ruleResult1, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(message1));
            Mock.Get(messageProvider3)
                .Setup(x => x.GetFailureMessageAsync(ruleResult3, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(message3));
            Assert.That(async () => (await sut.GetResultWithMessagesAsync(validationResult, default)).RuleResults.Select(x => x.Message),
                        Is.EquivalentTo(new[] { message1, null, message3 }));
        }

        [Test,AutoMoqData]
        public async Task GetResultWithMessagesAsyncShouldCopyAllOtherPropertiesOfARuleResultWhenAddingAMessage([Frozen] IGetsFailureMessageProvider messageProviderFactory,
                                                                                                                FailureMessageValidationResultPopulator sut,
                                                                                                                [RuleResult] ValidationRuleResult ruleResult,
                                                                                                                IGetsFailureMessage messageProvider,
                                                                                                                string message)
        {
            var validationResult = new ValidationResult(new[] { ruleResult });
            Mock.Get(messageProviderFactory).Setup(x => x.GetProvider(ruleResult)).Returns(messageProvider);
            Mock.Get(messageProvider)
                .Setup(x => x.GetFailureMessageAsync(ruleResult, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(message));

            var results = await sut.GetResultWithMessagesAsync(validationResult);
            var firstResult = results.RuleResults.First();

            Assert.Multiple(() =>
            {
                Assert.That(firstResult.Exception,          Is.EqualTo(ruleResult.Exception),           $"{nameof(ValidationRuleResult.Exception)} should have been copied");
                Assert.That(firstResult.Identifier,         Is.EqualTo(ruleResult.Identifier),          $"{nameof(ValidationRuleResult.Identifier)} should have been copied");
                Assert.That(firstResult.IsPass,             Is.EqualTo(ruleResult.IsPass),              $"{nameof(ValidationRuleResult.IsPass)} should have been copied");
                Assert.That(firstResult.Outcome,            Is.EqualTo(ruleResult.Outcome),             $"{nameof(ValidationRuleResult.Outcome)} should have been copied");
                Assert.That(firstResult.RuleContext,        Is.EqualTo(ruleResult.RuleContext),         $"{nameof(ValidationRuleResult.RuleContext)} should have been copied");
                Assert.That(firstResult.RuleInterface,      Is.EqualTo(ruleResult.RuleInterface),       $"{nameof(ValidationRuleResult.RuleInterface)} should have been copied");
                Assert.That(firstResult.ValidatedValue,     Is.EqualTo(ruleResult.ValidatedValue),      $"{nameof(ValidationRuleResult.ValidatedValue)} should have been copied");
                Assert.That(firstResult.ValidationLogic,    Is.EqualTo(ruleResult.ValidationLogic),     $"{nameof(ValidationRuleResult.ValidationLogic)} should have been copied");
            });
        }
    }
}