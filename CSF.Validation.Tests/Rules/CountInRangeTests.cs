using System;
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
            Assert.That(() => sut.GetResultAsync(new[] { 1, 2 }, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnFailForCollectionWithCountOutsideRange(CountInRange<int> sut, [RuleContext] RuleContext context)
        {
            sut.Min = 1;
            sut.Max = 2;
            Assert.That(() => sut.GetResultAsync(new[] { 1, 2, 3 }, context), Is.FailingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassForReadOnlyCollectionWithCountInRange(CountInRange<int> sut, [RuleContext] RuleContext context)
        {
            sut.Min = 1;
            sut.Max = 3;
            Assert.That(() => ((IRule<IReadOnlyCollection<int>>) sut).GetResultAsync(new[] { 1, 2 }, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnFailForReadOnlyCollectionWithCountOutsideRange(CountInRange<int> sut, [RuleContext] RuleContext context)
        {
            sut.Min = 1;
            sut.Max = 2;
            Assert.That(() => ((IRule<IReadOnlyCollection<int>>) sut).GetResultAsync(new[] { 1, 2, 3 }, context), Is.FailingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassForQueryableWithCountInRange(CountInRange<int> sut, [RuleContext] RuleContext context)
        {
            sut.Min = 1;
            sut.Max = 3;
            Assert.That(() => sut.GetResultAsync(new[] { 1, 2 }.AsQueryable(), context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnFailForQueryableWithCountOutsideRange(CountInRange<int> sut, [RuleContext] RuleContext context)
        {
            sut.Min = 1;
            sut.Max = 2;
            Assert.That(() => sut.GetResultAsync(new[] { 1, 2, 3 }.AsQueryable(), context), Is.FailingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetFailureMessageAsyncShouldReturnTheCorrectMessageWhenMinAndMaxAreSet(CountInRange<int> sut,
                                                                                           [RuleContext] RuleContext context,
                                                                                           IValidationLogic logic)
        {
            sut.Min = 1;
            sut.Max = 2;
            var result = new ValidationRuleResult(new RuleResult(RuleOutcome.Failed, new Dictionary<string, object> { { "Actual", 3 } }), context, logic);
            Assert.That(async () => await sut.GetFailureMessageAsync(Array.Empty<int>(), result),
                        Is.EqualTo("The value must have a count in the range 1 to 2 (inclusive). The actual count is 3."));
        }

        [Test,AutoMoqData]
        public void GetFailureMessageAsyncShouldReturnTheCorrectMessageWhenMinIsSet(CountInRange<int> sut,
                                                                                    [RuleContext] RuleContext context,
                                                                                    IValidationLogic logic)
        {
            sut.Min = 1;
            sut.Max = null;
            var result = new ValidationRuleResult(new RuleResult(RuleOutcome.Failed, new Dictionary<string, object> { { "Actual", 3 } }), context, logic);
            Assert.That(async () => await sut.GetFailureMessageAsync(Array.Empty<int>(), result),
                        Is.EqualTo("The value must have a count greater than or equal to 1. The actual count is 3."));
        }

        [Test,AutoMoqData]
        public void GetFailureMessageAsyncShouldReturnTheCorrectMessageWhenMaxIsSet(CountInRange<int> sut,
                                                                                    [RuleContext] RuleContext context,
                                                                                    IValidationLogic logic)
        {
            sut.Min = null;
            sut.Max = 2;
            var result = new ValidationRuleResult(new RuleResult(RuleOutcome.Failed, new Dictionary<string, object> { { "Actual", 3 } }), context, logic);
            Assert.That(async () => await sut.GetFailureMessageAsync(Array.Empty<int>(), result),
                        Is.EqualTo("The value must have a count less than or equal to 2. The actual count is 3."));
        }
    }
}