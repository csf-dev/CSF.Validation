using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace CSF.Validation.Rules
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class NotNullOrEmptyGenericTests
    {
        // This is quite light on tests, because the NotNull and NotEmpty rules are already well-tested individually

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassForANonEmptyCollection(NotNullOrEmpty<int> sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync((ICollection<int>)new List<int> { 1, 2 }, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassForANonEmptyReadOnlyCollection(NotNullOrEmpty<int> sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync((IReadOnlyCollection<int>)new List<int> { 1, 2 }, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassForANonEmptyQueryable(NotNullOrEmpty<int> sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync(new [] { 1, 2 }.AsQueryable(), context), Is.PassingRuleResult);
        }
    }
}