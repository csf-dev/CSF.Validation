using System;
using System.Linq;
using AutoFixture.NUnit3;
using CSF.Validation.Manifest;
using CSF.Validation.Rules;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.Messages
{
    [TestFixture,Parallelizable]
    public class FailureMessageCriteriaAdapterTests
    {
        [Test,AutoMoqData]
        public void CanGetFailureMessageShouldCallWrappedServiceWithOneGenericType([Frozen] IHasFailureMessageUsageCriteria<string> wrapped,
                                                                                   FailureMessageCriteriaAdapter<string> sut,
                                                                                   [RuleResult] RuleResult ruleResult,
                                                                                   [ManifestModel] ManifestRule rule,
                                                                                   Type ruleInterface,
                                                                                   RuleIdentifier id,
                                                                                   string actualValue,
                                                                                   bool expectedResult)
        {
            var context = new RuleContext(rule, id, actualValue, Enumerable.Empty<ValueContext>(), ruleInterface);
            var validationRuleResult = new ValidationRuleResult(ruleResult, context);
            Mock.Get(wrapped).Setup(x => x.CanGetFailureMessage(actualValue, validationRuleResult)).Returns(expectedResult);
            var actualResult = sut.CanGetFailureMessage(validationRuleResult);
            Assert.Multiple(() =>
            {
                Assert.That(actualResult, Is.EqualTo(expectedResult), "Actual result matches expected");
                Mock.Get(wrapped).Verify(x => x.CanGetFailureMessage(actualValue, validationRuleResult), Times.Once, "Wrapped service was called");
            });
        }

        [Test,AutoMoqData]
        public void CanGetFailureMessageShouldCallWrappedServiceWithTwoGenericTypes([Frozen] IHasFailureMessageUsageCriteria<string, int> wrapped,
                                                                                    FailureMessageCriteriaAdapter<string, int> sut,
                                                                                    [RuleResult] RuleResult ruleResult,
                                                                                    [ManifestModel] ManifestRule rule,
                                                                                    [ManifestModel] ManifestValue value,
                                                                                    Type ruleInterface,
                                                                                    RuleIdentifier id,
                                                                                    string actualValue,
                                                                                    int parentValue,
                                                                                    bool expectedResult)
        {
            var context = new RuleContext(rule, id, actualValue, new [] { new ValueContext(null, parentValue, value) }, ruleInterface);
            var validationRuleResult = new ValidationRuleResult(ruleResult, context);
            Mock.Get(wrapped).Setup(x => x.CanGetFailureMessage(actualValue, parentValue, validationRuleResult)).Returns(expectedResult);
            var actualResult = sut.CanGetFailureMessage(validationRuleResult);
            Assert.Multiple(() =>
            {
                Assert.That(actualResult, Is.EqualTo(expectedResult), "Actual result matches expected");
                Mock.Get(wrapped).Verify(x => x.CanGetFailureMessage(actualValue, parentValue, validationRuleResult), Times.Once, "Wrapped service was called");
            });
        }
    }
}