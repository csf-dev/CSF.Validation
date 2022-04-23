using System.Threading.Tasks;
using NUnit.Framework;

namespace CSF.Validation.Rules
{
    [TestFixture,Parallelizable]
    public class LengthTests
    {
        [Test,AutoMoqData]
        public async Task GetResultAsyncShouldReturnPassIfStringIsNull(Length sut, [RuleContext] RuleContext context)
        {
            var result = await sut.GetResultAsync((string) null, context);
            Assert.That(result.Outcome, Is.EqualTo(RuleOutcome.Passed));
        }

        [Test,AutoMoqData]
        public async Task GetResultAsyncShouldReturnPassIfStringIsWithinRange(Length sut, [RuleContext] RuleContext context)
        {
            sut.Min = 2;
            sut.Max = 4;
            var result = await sut.GetResultAsync("XYZ", context);
            Assert.That(result.Outcome, Is.EqualTo(RuleOutcome.Passed));
        }

        [Test,AutoMoqData]
        public async Task GetResultAsyncShouldReturnFailIfStringIsShorterThanMinimum(Length sut, [RuleContext] RuleContext context)
        {
            sut.Min = 2;
            sut.Max = 4;
            var result = await sut.GetResultAsync("X", context);
            Assert.That(result.Outcome, Is.EqualTo(RuleOutcome.Failed));
        }

        [Test,AutoMoqData]
        public async Task GetResultAsyncShouldReturnFailIfStringIsLongerThanMaximum(Length sut, [RuleContext] RuleContext context)
        {
            sut.Min = 2;
            sut.Max = 4;
            var result = await sut.GetResultAsync("XYZ123", context);
            Assert.That(result.Outcome, Is.EqualTo(RuleOutcome.Failed));
        }
    }
}