using NUnit.Framework;

namespace CSF.Validation.Rules
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class FloatInRangeTests
    {
        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfValueIsNull(FloatInRange sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync(null, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfNumberIsInRange(FloatInRange sut, [RuleContext] RuleContext context)
        {
            sut.Min = 5;
            sut.Max = 10;
            Assert.That(() => sut.GetResultAsync(6, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnFailIfNumberIsLowerThanMin(FloatInRange sut, [RuleContext] RuleContext context)
        {
            sut.Min = 5;
            sut.Max = 10;
            Assert.That(() => sut.GetResultAsync(2, context), Is.FailingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnFailIfNumberIsHigherThanMax(FloatInRange sut, [RuleContext] RuleContext context)
        {
            sut.Min = 5;
            sut.Max = 10;
            Assert.That(() => sut.GetResultAsync(20, context), Is.FailingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfMinAndMaxAreNull(FloatInRange sut, [RuleContext] RuleContext context, int anyNumber)
        {
            sut.Min = null;
            sut.Max = null;
            Assert.That(() => sut.GetResultAsync(anyNumber, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfFloatIsInRange(FloatInRange sut, [RuleContext] RuleContext context)
        {
            sut.Min = 5;
            sut.Max = 10;
            Assert.That(() => ((IRule<float>) sut).GetResultAsync(6, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfNullableFloatIsInRange(FloatInRange sut, [RuleContext] RuleContext context)
        {
            sut.Min = 5;
            sut.Max = 10;
            Assert.That(() => ((IRule<float?>) sut).GetResultAsync(6, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData,SetCulture("en-GB")]
        public void GetFailureMessageAsyncShouldReturnTheCorrectMessageWhenMinAndMaxAreSet(FloatInRange sut,
                                                                                           [RuleResult] ValidationRuleResult result)
        {
            sut.Min = 1.5;
            sut.Max = 2.5;
            Assert.That(async () => await sut.GetFailureMessageAsync(3.5, result),
                        Is.EqualTo("The value must be in the range 1.5 to 2.5 (inclusive). The actual value is 3.5."));
        }

        [Test,AutoMoqData,SetCulture("en-GB")]
        public void GetFailureMessageAsyncShouldReturnTheCorrectMessageWhenMinIsSet(FloatInRange sut,
                                                                                    [RuleResult] ValidationRuleResult result)
        {
            sut.Min = 1.5;
            sut.Max = null;
            Assert.That(async () => await sut.GetFailureMessageAsync(0.5, result),
                        Is.EqualTo("The value must be greater than or equal to 1.5. The actual value is 0.5."));
        }

        [Test,AutoMoqData,SetCulture("en-GB")]
        public void GetFailureMessageAsyncShouldReturnTheCorrectMessageWhenMaxIsSet(FloatInRange sut,
                                                                                    [RuleResult] ValidationRuleResult result)
        {
            sut.Min = null;
            sut.Max = 2.5;
            Assert.That(async () => await sut.GetFailureMessageAsync(3.5, result),
                        Is.EqualTo("The value must be less than or equal to 2.5. The actual value is 3.5."));
        }
    }
}