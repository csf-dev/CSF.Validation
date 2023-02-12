using System.Text.RegularExpressions;
using NUnit.Framework;

namespace CSF.Validation.Rules
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class MatchesRegexTests
    {
        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfValueIsNull(MatchesRegex sut, [RuleContext] RuleContext context)
        {
            sut.Pattern = "abc";
            Assert.That(() =>sut.GetResultAsync(null, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfValueMatchesRegexPattern(MatchesRegex sut, [RuleContext] RuleContext context)
        {
            sut.Pattern = "abc";
            Assert.That(() =>sut.GetResultAsync("123abc123", context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnFailIfValueDoesNotMatchRegexPattern(MatchesRegex sut, [RuleContext] RuleContext context)
        {
            // In this case we are expecting failure because default regex options are to be case-sensitive
            sut.Pattern = "ABC";
            Assert.That(() =>sut.GetResultAsync("123abc123", context), Is.FailingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldApplyRegexOptionsIfSpecified(MatchesRegex sut, [RuleContext] RuleContext context)
        {
            sut.Pattern = "ABC";
            sut.RegexOptions = RegexOptions.IgnoreCase;
            Assert.That(() =>sut.GetResultAsync("123abc123", context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldThrowIfItPatternIsNull(MatchesRegex sut, [RuleContext] RuleContext context, string value)
        {
            sut.Pattern = null;
            Assert.That(async () => await sut.GetResultAsync(value, context), Throws.InvalidOperationException);
        }
    }
}