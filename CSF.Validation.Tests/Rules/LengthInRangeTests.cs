using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace CSF.Validation.Rules
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class LengthInRangeTests
    {
        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfStringIsNull(LengthInRange sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync((string) null, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfStringIsWithinRange(LengthInRange sut, [RuleContext] RuleContext context)
        {
            sut.Min = 2;
            sut.Max = 4;
            Assert.That(() => sut.GetResultAsync("XYZ", context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnFailIfStringIsShorterThanMinimum(LengthInRange sut, [RuleContext] RuleContext context)
        {
            sut.Min = 2;
            sut.Max = 4;
            Assert.That(() => sut.GetResultAsync("X", context), Is.FailingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnFailIfStringIsLongerThanMaximum(LengthInRange sut, [RuleContext] RuleContext context)
        {
            sut.Min = 2;
            sut.Max = 4;
            Assert.That(() => sut.GetResultAsync("XYZ123", context), Is.FailingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfArrayIsNull(LengthInRange sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync((int[]) null, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfArrayLengthIsInRange(LengthInRange sut, [RuleContext] RuleContext context)
        {
            sut.Min = 2;
            sut.Max = 4;
            Assert.That(() => sut.GetResultAsync(new [] {1, 2, 3}, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfListIsNull(LengthInRange sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync((List<int>) null, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfListCountIsInRange(LengthInRange sut, [RuleContext] RuleContext context)
        {
            sut.Min = 2;
            sut.Max = 4;
            Assert.That(() => sut.GetResultAsync(new List<int>{1, 2, 3}, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData,SetCulture("en-GB")]
        public void GetFailureMessageAsyncShouldReturnTheCorrectMessageWhenMinAndMaxAreSet(LengthInRange sut,
                                                                                           [RuleContext] RuleContext context,
                                                                                           IValidationLogic logic)
        {
            sut.Min = 1;
            sut.Max = 2;
            var result = new ValidationRuleResult(new RuleResult(RuleOutcome.Failed, new Dictionary<string, object> { { "Actual", 3 } }), context, logic);
            Assert.That(async () => await sut.GetFailureMessageAsync(Array.Empty<object>(), result),
                        Is.EqualTo("The value must have a length in the range 1 to 2 (inclusive).  The actual length is 3."));
        }

        [Test,AutoMoqData,SetCulture("en-GB")]
        public void GetFailureMessageAsyncShouldReturnTheCorrectMessageWhenMinIsSet(LengthInRange sut,
                                                                                           [RuleContext] RuleContext context,
                                                                                           IValidationLogic logic)
        {
            sut.Min = 1;
            sut.Max = null;
            var result = new ValidationRuleResult(new RuleResult(RuleOutcome.Failed, new Dictionary<string, object> { { "Actual", 0 } }), context, logic);
            Assert.That(async () => await sut.GetFailureMessageAsync(Array.Empty<object>(), result),
                        Is.EqualTo("The value must have a length greater than or equal to 1.  The actual length is 0."));
        }

        [Test,AutoMoqData,SetCulture("en-GB")]
        public void GetFailureMessageAsyncShouldReturnTheCorrectMessageWhenMaxIsSet(LengthInRange sut,
                                                                                           [RuleContext] RuleContext context,
                                                                                           IValidationLogic logic)
        {
            sut.Min = null;
            sut.Max = 2;
            var result = new ValidationRuleResult(new RuleResult(RuleOutcome.Failed, new Dictionary<string, object> { { "Actual", 3 } }), context, logic);
            Assert.That(async () => await sut.GetFailureMessageAsync(Array.Empty<object>(), result),
                        Is.EqualTo("The value must have a length less than or equal to 2.  The actual length is 3."));
        }

        [Test,AutoMoqData,SetCulture("en-GB")]
        public void GetFailureMessageAsyncShouldReturnTheCorrectMessageWhenActualValueIsUnknown(LengthInRange sut,
                                                                                                [RuleResult] ValidationRuleResult result)
        {
            sut.Min = 1;
            sut.Max = 2;
            Assert.That(async () => await sut.GetFailureMessageAsync(Array.Empty<object>(), result),
                        Is.EqualTo("The value must have a length in the range 1 to 2 (inclusive).  The actual length is unknown."));
        }
    }
}