using System;
using System.Collections;
using NUnit.Framework;

namespace CSF.Validation.Rules
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class EmptyTests
    {
        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnAPassResultForAnEmptyNonGenericList(Empty sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync(new ArrayList(), context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnAFailureResultForANonEmptyEmptyNonGenericList(Empty sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync(new ArrayList {5}, context), Is.FailingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnAPassResultForAnEmptyEnumerable(Empty sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync((IEnumerable) new ArrayList(), context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnAFailureResultForANonEmptyEnumerable(Empty sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync((IEnumerable) new ArrayList {5}, context), Is.FailingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnAPassResultForAnEmptyArray(Empty sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync(new object[0], context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnAFailureResultForANonEmptyArray(Empty sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync(new [] {5}, context), Is.FailingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnAPassResultForAnEmptyString(Empty sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync(String.Empty, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnAFailureResultForANonEmptyString(Empty sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync("Foo bar", context), Is.FailingRuleResult);
        }
    }
}