using System;
using System.Linq;
using CSF.Validation.Manifest;
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
        public void IsMatchShouldReturnTrueIfActualValidatedTypeIsASubclassOfAttributeType([RuleResult] RuleResult result,
                                                                                           [ManifestModel] ManifestRule rule,
                                                                                           Type ruleType,
                                                                                           object objectId,
                                                                                           object actualValue,
                                                                                           IValidationLogic validationLogic)
        {
            var id = new RuleIdentifier(ruleType, typeof(Employee), objectId);
            var context = new RuleContext(rule, id, actualValue, Enumerable.Empty<ValueContext>(), typeof(string));
            var ruleResult = new ValidationRuleResult(result, context, validationLogic);
            var sut = new FailureMessageStrategyAttribute
            {
                ValidatedType = typeof(Person),
            };

            Assert.That(() => sut.IsMatch(ruleResult), Is.True);
        }

        [Test,AutoMoqData]
        public void IsMatchShouldReturnFalseIfActualValidatedTypeIsNotASubclassOfAttributeType([RuleResult] RuleResult result,
                                                                                               [ManifestModel] ManifestRule rule,
                                                                                               Type ruleType,
                                                                                               object objectId,
                                                                                               object actualValue,
                                                                                               IValidationLogic validationLogic)
        {
            var id = new RuleIdentifier(ruleType, typeof(Employee), objectId);
            var context = new RuleContext(rule, id, actualValue, Enumerable.Empty<ValueContext>(), typeof(string));
            var ruleResult = new ValidationRuleResult(result, context, validationLogic);
            var sut = new FailureMessageStrategyAttribute
            {
                ValidatedType = typeof(string),
            };

            Assert.That(() => sut.IsMatch(ruleResult), Is.False);
        }

        [Test,AutoMoqData]
        public void IsMatchShouldReturnTrueIfActualParentValidatedTypeIsASubclassOfAttributeParentType([RuleResult] RuleResult result,
                                                                                                       [ManifestModel] ManifestRule rule,
                                                                                                       Type type,
                                                                                                       object objectId,
                                                                                                       object actualValue,
                                                                                                       IValidationLogic validationLogic)
        {
            var id = new RuleIdentifier(type, type, objectId);
            var context = new RuleContext(rule, id, actualValue, new [] { new ValueContext(objectId, actualValue, new ManifestValue { ValidatedType = typeof(Employee) }) }, typeof(string));
            var ruleResult = new ValidationRuleResult(result, context, validationLogic);
            var sut = new FailureMessageStrategyAttribute
            {
                ParentValidatedType = typeof(Person),
            };
            
            Assert.That(() => sut.IsMatch(ruleResult), Is.True);
        }

        [Test,AutoMoqData]
        public void IsMatchShouldReturnFalseIfTheRuleInterfaceIsSpecifiedButDoesNotMatch([RuleResult] RuleResult result,
                                                                                         [ManifestModel] ManifestRule rule,
                                                                                         RuleIdentifier id,
                                                                                         object actualValue,
                                                                                         IValidationLogic validationLogic)
        {
            var context = new RuleContext(rule, id, actualValue, Enumerable.Empty<ValueContext>(), typeof(string));
            var ruleResult = new ValidationRuleResult(result, context, validationLogic);
            var sut = new FailureMessageStrategyAttribute
            {
                RuleInterface = typeof(int),
            };
            Assert.That(() => sut.IsMatch(ruleResult), Is.False);
        }

        class Person {}

        class Employee : Person {}
    }
}