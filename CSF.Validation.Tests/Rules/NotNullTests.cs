using NUnit.Framework;

namespace CSF.Validation.Rules
{
    [TestFixture,Parallelizable]
    public class NotNullTests
    {
        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassForANonNullInteger(NotNull sut, [RuleContext] RuleContext context, int value)
        {
            Assert.That(() =>sut.GetResultAsync(value, context), Is.PassingValidationResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnFailForANullInteger(NotNull sut, [RuleContext] RuleContext context)
        {
            int? value = null;
            Assert.That(() =>sut.GetResultAsync(value, context), Is.FailingValidationResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassForAnObject(NotNull sut, [RuleContext] RuleContext context, object value)
        {
            Assert.That(() =>sut.GetResultAsync(value, context), Is.PassingValidationResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnFailForNull(NotNull sut, [RuleContext] RuleContext context)
        {
            Assert.That(() =>sut.GetResultAsync(null, context), Is.FailingValidationResult);
        }
    }
}