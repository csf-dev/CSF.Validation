using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSF.Validation.Rules
{
    [TestFixture,Parallelizable]
    public class DateTimeInRangeTests
    {
        [Test,AutoMoqData]
        public async Task GetResultAsyncShouldReturnPassIfValueIsNull(DateTimeInRange sut, [RuleContext] RuleContext context)
        {
            var result = await sut.GetResultAsync(null, context);
            Assert.That(result.Outcome, Is.EqualTo(RuleOutcome.Passed));
        }

        [Test,AutoMoqData]
        public async Task GetResultAsyncShouldReturnPassIfDateTimeIsWithinRange(DateTimeInRange sut, [RuleContext] RuleContext context)
        {
            sut.Start = new DateTime(2001, 1, 1);
            sut.End = new DateTime(2002, 1, 1);
            var result = await sut.GetResultAsync(new DateTime(2001, 6, 1), context);
            Assert.That(result.Outcome, Is.EqualTo(RuleOutcome.Passed));
        }

        [Test,AutoMoqData]
        public async Task GetResultAsyncShouldReturnFailIfDateTimeIsBeforeStart(DateTimeInRange sut, [RuleContext] RuleContext context)
        {
            sut.Start = new DateTime(2001, 1, 1);
            sut.End = new DateTime(2002, 1, 1);
            var result = await sut.GetResultAsync(new DateTime(1999, 6, 1), context);
            Assert.That(result.Outcome, Is.EqualTo(RuleOutcome.Failed));
        }

        [Test,AutoMoqData]
        public async Task GetResultAsyncShouldReturnFailIfDateTimeIsAfterEnd(DateTimeInRange sut, [RuleContext] RuleContext context)
        {
            sut.Start = new DateTime(2001, 1, 1);
            sut.End = new DateTime(2002, 1, 1);
            var result = await sut.GetResultAsync(new DateTime(2010, 6, 1), context);
            Assert.That(result.Outcome, Is.EqualTo(RuleOutcome.Failed));
        }
    }
}