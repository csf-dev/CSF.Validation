using System.Collections.Generic;
using NUnit.Framework;

namespace CSF.Validation.Rules
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class LengthInRangeTests
    {
        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfStringIsNull(LengthInRange sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync((string) null, context), Is.PassingValidationResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfStringIsWithinRange(LengthInRange sut, [RuleContext] RuleContext context)
        {
            sut.Min = 2;
            sut.Max = 4;
            Assert.That(() => sut.GetResultAsync("XYZ", context), Is.PassingValidationResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnFailIfStringIsShorterThanMinimum(LengthInRange sut, [RuleContext] RuleContext context)
        {
            sut.Min = 2;
            sut.Max = 4;
            Assert.That(() => sut.GetResultAsync("X", context), Is.FailingValidationResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnFailIfStringIsLongerThanMaximum(LengthInRange sut, [RuleContext] RuleContext context)
        {
            sut.Min = 2;
            sut.Max = 4;
            Assert.That(() => sut.GetResultAsync("XYZ123", context), Is.FailingValidationResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfArrayIsNull(LengthInRange sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync((int[]) null, context), Is.PassingValidationResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfArrayLengthIsInRange(LengthInRange sut, [RuleContext] RuleContext context)
        {
            sut.Min = 2;
            sut.Max = 4;
            Assert.That(() => sut.GetResultAsync(new [] {1, 2, 3}, context), Is.PassingValidationResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfListIsNull(LengthInRange sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync((List<int>) null, context), Is.PassingValidationResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfListCountIsInRange(LengthInRange sut, [RuleContext] RuleContext context)
        {
            sut.Min = 2;
            sut.Max = 4;
            Assert.That(() => sut.GetResultAsync(new List<int>{1, 2, 3}, context), Is.PassingValidationResult);
        }
    }
}