using System.Collections.Generic;
using NUnit.Framework;

namespace CSF.Validation.Rules
{
    [TestFixture,Parallelizable]
    public class NotNullOrEmptyTests
    {
        // This is quite light on tests, because the NotNull and NotEmpty rules are already well-tested individually

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassForANonEmptyCollection(NotNullOrEmpty sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync((ICollection<int>)new List<int> { 1, 2 }, context), Is.PassingValidationResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassForANonEmptyEnumerable(NotNullOrEmpty sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync((IEnumerable<int>)new List<int> { 1, 2 }, context), Is.PassingValidationResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassForANonEmptyArray(NotNullOrEmpty sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync(new [] { 1, 2 }, context), Is.PassingValidationResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassForANonEmptyString(NotNullOrEmpty sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync("Foo bar", context), Is.PassingValidationResult);
        }
    }
}