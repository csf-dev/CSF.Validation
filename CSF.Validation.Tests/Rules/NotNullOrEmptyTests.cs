using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSF.Validation.Rules
{
    [TestFixture,Parallelizable]
    public class NotNullOrEmptyTests
    {
        [Test,AutoMoqData]
        public async Task GetResultAsyncShouldReturnPassForANonEmptyString(NotNullOrEmpty sut, [RuleContext] RuleContext context, int value)
        {
            var result = await sut.GetResultAsync("abc", context);
            Assert.That(result.Outcome, Is.EqualTo(RuleOutcome.Passed));
        }

        [Test,AutoMoqData]
        public async Task GetResultAsyncShouldReturnFailForANullString(NotNullOrEmpty sut, [RuleContext] RuleContext context)
        {
            var result = await sut.GetResultAsync((string) null, context);
            Assert.That(result.Outcome, Is.EqualTo(RuleOutcome.Failed));
        }

        [Test,AutoMoqData]
        public async Task GetResultAsyncShouldReturnFailForAnEmptyString(NotNullOrEmpty sut, [RuleContext] RuleContext context)
        {
            var result = await sut.GetResultAsync(string.Empty, context);
            Assert.That(result.Outcome, Is.EqualTo(RuleOutcome.Failed));
        }
    }
}