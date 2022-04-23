using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace CSF.Validation.Rules
{
    [TestFixture,Parallelizable]
    public class NotEmptyGenericTests
    {
        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnFailWithAnEmptyCollection(NotEmpty<string> sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync(new List<string>(), context), Is.FailingValidationResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassWithANonEmptyCollection(NotEmpty<string> sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync(new List<string> {"Foo", "Bar"}, context), Is.PassingValidationResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnFailWithAnEmptyReadonlyCollection(NotEmpty<string> sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => ((IRule<IReadOnlyCollection<string>>) sut).GetResultAsync(new List<string>(), context), Is.FailingValidationResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassWithANonEmptyReadonlyCollection(NotEmpty<string> sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => ((IRule<IReadOnlyCollection<string>>) sut).GetResultAsync(new List<string> {"Foo", "Bar"}, context), Is.PassingValidationResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnFailWithAnEmptyQueryable(NotEmpty<string> sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync(new List<string>().AsQueryable(), context), Is.FailingValidationResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassWithANonEmptyQueryable(NotEmpty<string> sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync(new List<string> {"Foo", "Bar"}.AsQueryable(), context), Is.PassingValidationResult);
        }
    }
}