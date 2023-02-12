using NUnit.Framework;

namespace CSF.Validation.Rules
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class DecimalInRangeTests
    {
        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfValueIsNull(DecimalInRange sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync(null, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfNumberIsInRange(DecimalInRange sut, [RuleContext] RuleContext context)
        {
            sut.Min = 5;
            sut.Max = 10;
            Assert.That(() => sut.GetResultAsync(6, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfNumberIsLowerThanMin(DecimalInRange sut, [RuleContext] RuleContext context)
        {
            sut.Min = 5;
            sut.Max = 10;
            Assert.That(() => sut.GetResultAsync(2, context), Is.FailingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfNumberIsHigherThanMax(DecimalInRange sut, [RuleContext] RuleContext context)
        {
            sut.Min = 5;
            sut.Max = 10;
            Assert.That(() => sut.GetResultAsync(20, context), Is.FailingRuleResult);
        }

        [Test,AutoMoqData,SetCulture("en-GB")]
        public void GetFailureMessageAsyncShouldReturnTheCorrectMessageWhenMinAndMaxAreSet(DecimalInRange sut,
                                                                                           [RuleResult] ValidationRuleResult result)
        {
            sut.Min = 1.5m;
            sut.Max = 2.5m;
            Assert.That(async () => await sut.GetFailureMessageAsync(3.5m, result),
                        Is.EqualTo("The value must be in the range 1.5 to 2.5 (inclusive). The actual value is 3.5."));
        }

        [Test,AutoMoqData,SetCulture("en-GB")]
        public void GetFailureMessageAsyncShouldReturnTheCorrectMessageWhenMinIsSet(DecimalInRange sut,
                                                                                    [RuleResult] ValidationRuleResult result)
        {
            sut.Min = 1.5m;
            sut.Max = null;
            Assert.That(async () => await sut.GetFailureMessageAsync(0.5m, result),
                        Is.EqualTo("The value must be greater than or equal to 1.5. The actual value is 0.5."));
        }

        [Test,AutoMoqData,SetCulture("en-GB")]
        public void GetFailureMessageAsyncShouldReturnTheCorrectMessageWhenMaxIsSet(DecimalInRange sut,
                                                                                    [RuleResult] ValidationRuleResult result)
        {
            sut.Min = null;
            sut.Max = 2.5m;
            Assert.That(async () => await sut.GetFailureMessageAsync(3.5m, result),
                        Is.EqualTo("The value must be less than or equal to 2.5. The actual value is 3.5."));
        }
    }
}