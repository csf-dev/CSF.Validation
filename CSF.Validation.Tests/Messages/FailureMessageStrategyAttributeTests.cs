using System;
using CSF.Validation.RuleExecution;
using CSF.Validation.Rules;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.Messages
{
    [TestFixture,Parallelizable]
    public class FailureMessageStrategyAttributeTests
    {
        [Test,AutoMoqData]
        public void GetPriorityShouldReturnZeroForAnEmptyAttribute()
        {
            var sut = new FailureMessageStrategyAttribute();
            Assert.That(() => sut.GetPriority(), Is.Zero);
        }

        [Test,AutoMoqData]
        public void GetPriorityShouldReturn7ForAnAttributeWithEveryPropertySet(string stringVal,
                                                                               RuleOutcome outcome,
                                                                               Type type)
        {
            var sut = new FailureMessageStrategyAttribute
            {
                MemberName = stringVal,
                Outcome = outcome,
                ParentValidatedType = type,
                RuleInterface = type,
                RuleName = stringVal,
                RuleType = type,
                ValidatedType = type,
            };
            Assert.That(() => sut.GetPriority(), Is.EqualTo(7));
        }

        [Test,AutoMoqData]
        public void IsMatchShouldReturnTrueForAnEmptyAttribute([RuleResult] ValidationRuleResult result)
        {
            var sut = new FailureMessageStrategyAttribute();
            Assert.That(() => sut.IsMatch(result), Is.True);
        }

        [Test,AutoMoqData]
        public void IsMatchShouldReturnFalseIfTheRuleInterfaceIsSpecifiedButDoesNotMatch([RuleResult] RuleResult result,
                                                                                         [RuleContext] RuleContext ruleContext,
                                                                                         [ExecutableModel] ExecutableRule rule,
                                                                                         IValidationLogic logic)
        {
            rule.RuleLogic = logic;
            Mock.Get(logic).SetupGet(x => x.RuleInterface).Returns(typeof(string));
            var ruleResult = new ValidationRuleResult(result, ruleContext, rule);
            var sut = new FailureMessageStrategyAttribute
            {
                RuleInterface = typeof(int),
            };
            Assert.That(() => sut.IsMatch(ruleResult), Is.False);
        }
    }
}