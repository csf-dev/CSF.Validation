using System.Threading.Tasks;
using AutoFixture.NUnit3;
using CSF.Validation.Rules;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.Messages
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class FailureMessageValidationResultPopulatorTests
    {
        [Test,AutoMoqData]
        public void GetResultWithMessagesAsyncShouldReturnResultsFromRuleMessageProvider([Frozen] IGetsRuleWithMessageProvider messageProviderFactory,
                                                                                         IGetsValidationRuleResultWithMessage provider,
                                                                                         FailureMessageValidationResultPopulator sut,
                                                                                         [RuleResult] ValidationRuleResult ruleResult1,
                                                                                         [RuleResult] ValidationRuleResult ruleResult2,
                                                                                         [RuleResult] ValidationRuleResult ruleResult3,
                                                                                         [RuleResult] ValidationRuleResult ruleResult4,
                                                                                         [RuleResult] ValidationRuleResult ruleResult5,
                                                                                         [RuleResult] ValidationRuleResult ruleResult6,
                                                                                         ResolvedValidationOptions options)
        {
            var validationResult = new ValidationResult<object>(new[] { ruleResult1, ruleResult2, ruleResult3 }, new Manifest.ValidationManifest { ValidatedType = typeof(object) });
            Mock.Get(messageProviderFactory).Setup(x => x.GetRuleWithMessageProvider(It.IsAny<ResolvedValidationOptions>())).Returns(provider);
            Mock.Get(provider).Setup(x => x.GetRuleResultWithMessageAsync(ruleResult1, default)).Returns(new ValueTask<ValidationRuleResult>(ruleResult4));
            Mock.Get(provider).Setup(x => x.GetRuleResultWithMessageAsync(ruleResult2, default)).Returns(new ValueTask<ValidationRuleResult>(ruleResult5));
            Mock.Get(provider).Setup(x => x.GetRuleResultWithMessageAsync(ruleResult3, default)).Returns(new ValueTask<ValidationRuleResult>(ruleResult6));
            Assert.That(async () => (await sut.GetResultWithMessagesAsync(validationResult, options, default))?.RuleResults,
                        Is.EquivalentTo(new[] { ruleResult4, ruleResult5, ruleResult6 }));
        }
    }
}