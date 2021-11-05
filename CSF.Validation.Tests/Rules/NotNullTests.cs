using System.Threading.Tasks;
using NUnit.Framework;

namespace CSF.Validation.Rules
{
    [TestFixture,Parallelizable]
    public class NotNullTests
    {
        [Test,AutoMoqData]
        public async Task GetResultAsyncShouldReturnPassForANonNullInteger(NotNull sut, [RuleContext] RuleContext context, int value)
        {
            var result = await sut.GetResultAsync(value, context);
            Assert.That(result.Outcome, Is.EqualTo(RuleOutcome.Passed));
        }

        [Test,AutoMoqData]
        public async Task GetResultAsyncShouldReturnFailForANullInteger(NotNull sut, [RuleContext] RuleContext context)
        {
            int? value = null;
            var result = await sut.GetResultAsync(value, context);
            Assert.That(result.Outcome, Is.EqualTo(RuleOutcome.Failed));
        }

        [Test,AutoMoqData]
        public async Task GetResultAsyncShouldReturnPassForAnObject(NotNull sut, [RuleContext] RuleContext context, object value)
        {
            var result = await sut.GetResultAsync(value, context);
            Assert.That(result.Outcome, Is.EqualTo(RuleOutcome.Passed));
        }

        [Test,AutoMoqData]
        public async Task GetResultAsyncShouldReturnFailForNull(NotNull sut, [RuleContext] RuleContext context)
        {
            var result = await sut.GetResultAsync(null, context);
            Assert.That(result.Outcome, Is.EqualTo(RuleOutcome.Failed));
        }
    }
}