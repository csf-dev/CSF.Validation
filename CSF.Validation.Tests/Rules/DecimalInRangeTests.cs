using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSF.Validation.Rules
{
    [TestFixture,Parallelizable]
    public class DecimalInRangeTests
    {
        [Test,AutoMoqData]
        public async Task GetResultAsyncShouldReturnPassIfValueIsNull(DecimalInRange sut, [RuleContext] RuleContext context)
        {
            var result = await sut.GetResultAsync(null, context);
            Assert.That(result.Outcome, Is.EqualTo(RuleOutcome.Passed));
        }

        [Test,AutoMoqData]
        public async Task GetResultAsyncShouldReturnPassIfNumberIsInRange(DecimalInRange sut, [RuleContext] RuleContext context)
        {
            sut.Min = 5;
            sut.Max = 10;
            var result = await sut.GetResultAsync(6, context);
            Assert.That(result.Outcome, Is.EqualTo(RuleOutcome.Passed));
        }

        [Test,AutoMoqData]
        public async Task GetResultAsyncShouldReturnPassIfNumberIsLowerThanMin(DecimalInRange sut, [RuleContext] RuleContext context)
        {
            sut.Min = 5;
            sut.Max = 10;
            var result = await sut.GetResultAsync(2, context);
            Assert.That(result.Outcome, Is.EqualTo(RuleOutcome.Failed));
        }

        [Test,AutoMoqData]
        public async Task GetResultAsyncShouldReturnPassIfNumberIsHigherThanMax(DecimalInRange sut, [RuleContext] RuleContext context)
        {
            sut.Min = 5;
            sut.Max = 10;
            var result = await sut.GetResultAsync(20, context);
            Assert.That(result.Outcome, Is.EqualTo(RuleOutcome.Failed));
        }
    }
}