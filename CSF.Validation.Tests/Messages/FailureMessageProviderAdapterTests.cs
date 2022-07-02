using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using CSF.Validation.Manifest;
using CSF.Validation.Rules;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.Messages
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class FailureMessageProviderAdapterTests
    {
        [Test,AutoMoqData]
        public void GetFailureMessageAsyncShouldCallWrappedServiceWithOneGenericType([Frozen] IGetsFailureMessage<string> wrapped,
                                                                                   FailureMessageProviderAdapter<string> sut,
                                                                                   [RuleResult] RuleResult ruleResult,
                                                                                   [ManifestModel] ManifestRule rule,
                                                                                   Type ruleInterface,
                                                                                   RuleIdentifier id,
                                                                                   string actualValue,
                                                                                   string expectedResult,
                                                                                   IValidationLogic validationLogic)
        {
            var context = new RuleContext(rule, id, actualValue, Enumerable.Empty<ValueContext>(), ruleInterface);
            var validationRuleResult = new ValidationRuleResult(ruleResult, context, validationLogic);
            Mock.Get(wrapped).Setup(x => x.GetFailureMessageAsync(actualValue, validationRuleResult, default)).Returns(Task.FromResult(expectedResult));
            Assert.That(async () => await sut.GetFailureMessageAsync(validationRuleResult), Is.EqualTo(expectedResult));
        }

        [Test,AutoMoqData]
        public void GetFailureMessageAsyncShouldCallWrappedServiceWithTwoGenericTypes([Frozen] IGetsFailureMessage<string, int> wrapped,
                                                                                    FailureMessageProviderAdapter<string, int> sut,
                                                                                    [RuleResult] RuleResult ruleResult,
                                                                                    [ManifestModel] ManifestRule rule,
                                                                                    [ManifestModel] ManifestValue value,
                                                                                    Type ruleInterface,
                                                                                    RuleIdentifier id,
                                                                                    string actualValue,
                                                                                    int parentValue,
                                                                                    string expectedResult,
                                                                                   IValidationLogic validationLogic)
        {
            var context = new RuleContext(rule, id, actualValue, new [] { new ValueContext(null, parentValue, value) }, ruleInterface);
            var validationRuleResult = new ValidationRuleResult(ruleResult, context, validationLogic);
            Mock.Get(wrapped).Setup(x => x.GetFailureMessageAsync(actualValue, parentValue, validationRuleResult, default)).Returns(Task.FromResult(expectedResult));
            Assert.That(async () => await sut.GetFailureMessageAsync(validationRuleResult), Is.EqualTo(expectedResult));
        }
    }
}