using NUnit.Framework;

namespace CSF.Validation.Rules
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class IntegerInRangeTests
    {
        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfValueIsNull(IntegerInRange sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync(null, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfNumberIsInRange(IntegerInRange sut, [RuleContext] RuleContext context)
        {
            sut.Min = 5;
            sut.Max = 10;
            Assert.That(() => sut.GetResultAsync(6, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnFailIfNumberIsLowerThanMin(IntegerInRange sut, [RuleContext] RuleContext context)
        {
            sut.Min = 5;
            sut.Max = 10;
            Assert.That(() => sut.GetResultAsync(2, context), Is.FailingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnFailIfNumberIsHigherThanMax(IntegerInRange sut, [RuleContext] RuleContext context)
        {
            sut.Min = 5;
            sut.Max = 10;
            Assert.That(() => sut.GetResultAsync(20, context), Is.FailingRuleResult);
        }

        [Test,AutoMoqData,SetCulture("en-GB")]
        public void GetFailureMessageAsyncShouldReturnTheCorrectMessageWhenMinAndMaxAreSet(IntegerInRange sut,
                                                                                           [RuleResult] ValidationRuleResult result)
        {
            sut.Min = 1;
            sut.Max = 2;
            Assert.That(async () => await sut.GetFailureMessageAsync(3, result),
                        Is.EqualTo("The value must be in the range 1 to 2 (inclusive). The actual value is 3."));
        }

        [Test,AutoMoqData,SetCulture("en-GB")]
        public void GetFailureMessageAsyncShouldReturnTheCorrectMessageWhenMinIsSet(IntegerInRange sut,
                                                                                    [RuleResult] ValidationRuleResult result)
        {
            sut.Min = 1;
            sut.Max = null;
            Assert.That(async () => await sut.GetFailureMessageAsync(0, result),
                        Is.EqualTo("The value must be greater than or equal to 1. The actual value is 0."));
        }

        [Test,AutoMoqData,SetCulture("en-GB")]
        public void GetFailureMessageAsyncShouldReturnTheCorrectMessageWhenMaxIsSet(IntegerInRange sut,
                                                                                    [RuleResult] ValidationRuleResult result)
        {
            sut.Min = null;
            sut.Max = 2;
            Assert.That(async () => await sut.GetFailureMessageAsync(3, result),
                        Is.EqualTo("The value must be less than or equal to 2. The actual value is 3."));
        }
    }
}