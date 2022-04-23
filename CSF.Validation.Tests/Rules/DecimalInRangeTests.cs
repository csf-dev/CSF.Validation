using NUnit.Framework;

namespace CSF.Validation.Rules
{
    [TestFixture,Parallelizable]
    public class DecimalInRangeTests
    {
        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfValueIsNull(DecimalInRange sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync(null, context), Is.PassingValidationResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfNumberIsInRange(DecimalInRange sut, [RuleContext] RuleContext context)
        {
            sut.Min = 5;
            sut.Max = 10;
            Assert.That(() => sut.GetResultAsync(6, context), Is.PassingValidationResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfNumberIsLowerThanMin(DecimalInRange sut, [RuleContext] RuleContext context)
        {
            sut.Min = 5;
            sut.Max = 10;
            Assert.That(() => sut.GetResultAsync(2, context), Is.FailingValidationResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfNumberIsHigherThanMax(DecimalInRange sut, [RuleContext] RuleContext context)
        {
            sut.Min = 5;
            sut.Max = 10;
            Assert.That(() => sut.GetResultAsync(20, context), Is.FailingValidationResult);
        }
    }
}