using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSF.Validation.Rules
{
    [TestFixture,Parallelizable]
    public class MatchesRegexTests
    {
        [Test,AutoMoqData]
        public async Task GetResultAsyncShouldReturnPassIfValueIsNull(MatchesRegex sut, [RuleContext] RuleContext context)
        {
            sut.Pattern = "abc";
            var result = await sut.GetResultAsync(null, context);
            Assert.That(result.Outcome, Is.EqualTo(RuleOutcome.Passed));
        }

        [Test,AutoMoqData]
        public async Task GetResultAsyncShouldReturnPassIfValueMatchesRegexPattern(MatchesRegex sut, [RuleContext] RuleContext context)
        {
            sut.Pattern = "abc";
            var result = await sut.GetResultAsync("123abc123", context);
            Assert.That(result.Outcome, Is.EqualTo(RuleOutcome.Passed));
        }

        [Test,AutoMoqData]
        public async Task GetResultAsyncShouldReturnFailIfValueDoesNotMatchRegexPattern(MatchesRegex sut, [RuleContext] RuleContext context)
        {
            // In this case we are expecting failure because default regex options are to be case-sensitive
            sut.Pattern = "ABC";
            var result = await sut.GetResultAsync("123abc123", context);
            Assert.That(result.Outcome, Is.EqualTo(RuleOutcome.Failed));
        }

        [Test,AutoMoqData]
        public async Task GetResultAsyncShouldApplyRegexOptionsIfSpecified(MatchesRegex sut, [RuleContext] RuleContext context)
        {
            sut.Pattern = "ABC";
            sut.RegexOptions = RegexOptions.IgnoreCase;
            var result = await sut.GetResultAsync("123abc123", context);
            Assert.That(result.Outcome, Is.EqualTo(RuleOutcome.Passed));
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldThrowIfItPatternIsNull(MatchesRegex sut, [RuleContext] RuleContext context, string value)
        {
            sut.Pattern = null;
            Assert.That(async () => await sut.GetResultAsync(value, context), Throws.InvalidOperationException);
        }
    }
}