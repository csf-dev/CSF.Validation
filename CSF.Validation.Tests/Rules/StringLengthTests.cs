using System.Threading.Tasks;
using NUnit.Framework;

namespace CSF.Validation.Rules
{
    [TestFixture,Parallelizable]
    public class StringLengthTests
    {
        [Test,AutoMoqData]
        public async Task GetResultAsyncShouldReturnPassIfStringIsNull(StringLength sut, [RuleContext] RuleContext context)
        {
            var result = await sut.GetResultAsync(null, context);
            Assert.That(result.Outcome, Is.EqualTo(RuleOutcome.Passed));
        }

        [Test,AutoMoqData]
        public async Task GetResultAsyncShouldReturnPassIfStringIsWithinRange(StringLength sut, [RuleContext] RuleContext context)
        {
            sut.Min = 2;
            sut.Max = 4;
            var result = await sut.GetResultAsync("XYZ", context);
            Assert.That(result.Outcome, Is.EqualTo(RuleOutcome.Passed));
        }

        [Test,AutoMoqData]
        public async Task GetResultAsyncShouldReturnFailIfStringIsShorterThanMinimum(StringLength sut, [RuleContext] RuleContext context)
        {
            sut.Min = 2;
            sut.Max = 4;
            var result = await sut.GetResultAsync("X", context);
            Assert.That(result.Outcome, Is.EqualTo(RuleOutcome.Failed));
        }

        [Test,AutoMoqData]
        public async Task GetResultAsyncShouldReturnFailIfStringIsLongerThanMaximum(StringLength sut, [RuleContext] RuleContext context)
        {
            sut.Min = 2;
            sut.Max = 4;
            var result = await sut.GetResultAsync("XYZ123", context);
            Assert.That(result.Outcome, Is.EqualTo(RuleOutcome.Failed));
        }
    }
}