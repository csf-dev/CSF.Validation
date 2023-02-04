using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace CSF.Validation.Rules
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class EmptyGenericTests
    {
        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnFailWithAnEmptyCollection(Empty<string> sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync(new List<string>(), context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassWithANonEmptyCollection(Empty<string> sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync(new List<string> {"Foo", "Bar"}, context), Is.FailingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnFailWithAnEmptyReadonlyCollection(Empty<string> sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => ((IRule<IReadOnlyCollection<string>>) sut).GetResultAsync(new List<string>(), context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassWithANonEmptyReadonlyCollection(Empty<string> sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => ((IRule<IReadOnlyCollection<string>>) sut).GetResultAsync(new List<string> {"Foo", "Bar"}, context), Is.FailingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnFailWithAnEmptyQueryable(Empty<string> sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync(new List<string>().AsQueryable(), context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassWithANonEmptyQueryable(Empty<string> sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync(new List<string> {"Foo", "Bar"}.AsQueryable(), context), Is.FailingRuleResult);
        }
    }
}