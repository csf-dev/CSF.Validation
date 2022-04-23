using NUnit.Framework;

namespace CSF.Validation.Rules
{
    [TestFixture,Parallelizable]
    public class NotNullOrEmptyTests
    {
        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassForANonEmptyString(NotNullOrEmpty sut, [RuleContext] RuleContext context, int value)
        {
            Assert.That(() =>sut.GetResultAsync("abc", context), Is.PassingValidationResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnFailForANullString(NotNullOrEmpty sut, [RuleContext] RuleContext context)
        {
            Assert.That(() =>sut.GetResultAsync((string) null, context), Is.FailingValidationResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnFailForAnEmptyString(NotNullOrEmpty sut, [RuleContext] RuleContext context)
        {
            Assert.That(() =>sut.GetResultAsync(string.Empty, context), Is.FailingValidationResult);
        }
    }
}