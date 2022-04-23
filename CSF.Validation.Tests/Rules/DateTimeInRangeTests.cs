using System;
using NUnit.Framework;

namespace CSF.Validation.Rules
{
    [TestFixture,Parallelizable]
    public class DateTimeInRangeTests
    {
        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfValueIsNull(DateTimeInRange sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync(null, context), Is.PassingValidationResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfDateTimeIsWithinRange(DateTimeInRange sut, [RuleContext] RuleContext context)
        {
            sut.Start = new DateTime(2001, 1, 1);
            sut.End = new DateTime(2002, 1, 1);
            Assert.That(() => sut.GetResultAsync(new DateTime(2001, 6, 1), context), Is.PassingValidationResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnFailIfDateTimeIsBeforeStart(DateTimeInRange sut, [RuleContext] RuleContext context)
        {
            sut.Start = new DateTime(2001, 1, 1);
            sut.End = new DateTime(2002, 1, 1);
            Assert.That(() => sut.GetResultAsync(new DateTime(1999, 6, 1), context), Is.FailingValidationResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnFailIfDateTimeIsAfterEnd(DateTimeInRange sut, [RuleContext] RuleContext context)
        {
            sut.Start = new DateTime(2001, 1, 1);
            sut.End = new DateTime(2002, 1, 1);
            Assert.That(() => sut.GetResultAsync(new DateTime(2010, 6, 1), context), Is.FailingValidationResult);
        }
    }
}