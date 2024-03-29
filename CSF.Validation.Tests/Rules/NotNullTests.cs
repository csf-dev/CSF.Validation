using NUnit.Framework;

namespace CSF.Validation.Rules
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class NotNullTests
    {
        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassForANonNullInteger(NotNull sut, [RuleContext] RuleContext context, int value)
        {
            Assert.That(() =>sut.GetResultAsync(value, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnFailForANullInteger(NotNull sut, [RuleContext] RuleContext context)
        {
            int? value = null;
            Assert.That(() =>sut.GetResultAsync(value, context), Is.FailingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassForAnObject(NotNull sut, [RuleContext] RuleContext context, object value)
        {
            Assert.That(() =>sut.GetResultAsync(value, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnFailForNull(NotNull sut, [RuleContext] RuleContext context)
        {
            Assert.That(() =>sut.GetResultAsync(null, context), Is.FailingRuleResult);
        }
    }
}