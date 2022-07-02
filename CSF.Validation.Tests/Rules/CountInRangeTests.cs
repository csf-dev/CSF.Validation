using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace CSF.Validation.Rules
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class CountInRangeTests
    {
        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassForCollectionWithCountInRange(CountInRange<int> sut, [RuleContext] RuleContext context)
        {
            sut.Min = 1;
            sut.Max = 3;
            Assert.That(() => sut.GetResultAsync(new[] { 1, 2 }, context), Is.PassingValidationResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnFailForCollectionWithCountOutsideRange(CountInRange<int> sut, [RuleContext] RuleContext context)
        {
            sut.Min = 1;
            sut.Max = 2;
            Assert.That(() => sut.GetResultAsync(new[] { 1, 2, 3 }, context), Is.FailingValidationResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassForReadOnlyCollectionWithCountInRange(CountInRange<int> sut, [RuleContext] RuleContext context)
        {
            sut.Min = 1;
            sut.Max = 3;
            Assert.That(() => ((IRule<IReadOnlyCollection<int>>) sut).GetResultAsync(new[] { 1, 2 }, context), Is.PassingValidationResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnFailForReadOnlyCollectionWithCountOutsideRange(CountInRange<int> sut, [RuleContext] RuleContext context)
        {
            sut.Min = 1;
            sut.Max = 2;
            Assert.That(() => ((IRule<IReadOnlyCollection<int>>) sut).GetResultAsync(new[] { 1, 2, 3 }, context), Is.FailingValidationResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassForQueryableWithCountInRange(CountInRange<int> sut, [RuleContext] RuleContext context)
        {
            sut.Min = 1;
            sut.Max = 3;
            Assert.That(() => sut.GetResultAsync(new[] { 1, 2 }.AsQueryable(), context), Is.PassingValidationResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnFailForQueryableWithCountOutsideRange(CountInRange<int> sut, [RuleContext] RuleContext context)
        {
            sut.Min = 1;
            sut.Max = 2;
            Assert.That(() => sut.GetResultAsync(new[] { 1, 2, 3 }.AsQueryable(), context), Is.FailingValidationResult);
        }
    }
}