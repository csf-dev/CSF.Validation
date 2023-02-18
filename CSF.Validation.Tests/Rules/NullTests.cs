using NUnit.Framework;

namespace CSF.Validation.Rules
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class NullTests
    {
        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassResultIfObjectIsNull(Null sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync(null, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnFailResultIfObjectIsNotNull(Null sut, [RuleContext] RuleContext context, object obj)
        {
            Assert.That(() => sut.GetResultAsync(obj, context), Is.FailingRuleResult);
        }
    }
}