using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace CSF.Validation.Rules
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class EmptyTests
    {
        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnAPassResultForAnEmptyNonGenericList(Empty sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync(new ArrayList(), context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnAFailureResultForANonEmptyEmptyNonGenericList(Empty sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync(new ArrayList {5}, context), Is.FailingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnAPassResultForAnEmptyEnumerable(Empty sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync((IEnumerable) new ArrayList(), context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnAFailureResultForANonEmptyEnumerable(Empty sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync((IEnumerable) new ArrayList {5}, context), Is.FailingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnAPassResultForAnEmptyArray(Empty sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync(new object[0], context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnAFailureResultForANonEmptyArray(Empty sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync(new [] {5}, context), Is.FailingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnAPassResultForAnEmptyString(Empty sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync(String.Empty, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnAFailureResultForANonEmptyString(Empty sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync("Foo bar", context), Is.FailingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetFailureMessageAsyncShouldReturnCorrectMessageIfNoCountInformationAvailable(Empty sut,
                                                                                                  [RuleContext] RuleContext context,
                                                                                                  IValidationLogic logic)
        {
            var result = new ValidationRuleResult(new RuleResult(RuleOutcome.Failed), context, logic);
            Assert.That(async () => await sut.GetFailureMessageAsync(Array.Empty<int>(), result),
                        Is.EqualTo("The value must be empty but it is not."));
        }

        [Test,AutoMoqData]
        public void GetFailureMessageAsyncShouldReturnCorrectMessageIfCountIsOne(Empty sut,
                                                                                 [RuleContext] RuleContext context,
                                                                                 IValidationLogic logic)
        {
            var result = new ValidationRuleResult(new RuleResult(RuleOutcome.Failed, new Dictionary<string, object> { { "Count", 1 } }), context, logic);
            Assert.That(async () => await sut.GetFailureMessageAsync(Array.Empty<int>(), result),
                        Is.EqualTo("The value must be empty but it actually has one item."));
        }

        [Test,AutoMoqData]
        public void GetFailureMessageAsyncShouldReturnCorrectMessageIfCountIsThree(Empty sut,
                                                                                   [RuleContext] RuleContext context,
                                                                                   IValidationLogic logic)
        {
            var result = new ValidationRuleResult(new RuleResult(RuleOutcome.Failed, new Dictionary<string, object> { { "Count", 3 } }), context, logic);
            Assert.That(async () => await sut.GetFailureMessageAsync(Array.Empty<int>(), result),
                        Is.EqualTo("The value must be empty but it actually has 3 items."));
        }
    }
}