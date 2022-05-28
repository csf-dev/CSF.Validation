using System;
using System.Linq;
using CSF.Validation.Manifest;
using CSF.Validation.Rules;
using CSF.Specifications;
using NUnit.Framework;

namespace CSF.Validation
{
    [TestFixture,Parallelizable]
    public class RuleResultIsForDescendentOfValueTests
    {
        [Test,AutoMoqData]
        public void GetExpressionShouldReturnAnExpressionWhichReturnsTrueForAResultWhichMatchesTheCurrentContext([ManifestModel] ManifestRule rule,
                                                                                                                 [RuleId] RuleIdentifier ruleIdentifier,
                                                                                                                 object actualValue,
                                                                                                                 Type ruleInterface,
                                                                                                                 [RuleResult] RuleResult ruleResult,
                                                                                                                 IValidationLogic logic)
        {
            rule.ManifestValue.IdentityAccessor = null;
            var context = new RuleContext(rule, ruleIdentifier, actualValue, Enumerable.Empty<ValueContext>(), ruleInterface);
            var validationRuleResult = new ValidationRuleResult(ruleResult, context, logic);
            var sut = new RuleResultIsForDescendentOfValue(rule.ManifestValue);

            Assert.That(() => sut.Matches(validationRuleResult), Is.True);
        }

        [Test,AutoMoqData]
        public void GetExpressionShouldReturnAnExpressionWhichReturnsFalseForAResultWhichMatchesNoContexts([ManifestModel] ManifestValue otherValue,
                                                                                                           [ManifestModel] ManifestRule rule,
                                                                                                           [RuleId] RuleIdentifier ruleIdentifier,
                                                                                                           object actualValue,
                                                                                                           Type ruleInterface,
                                                                                                           [RuleResult] RuleResult ruleResult,
                                                                                                           IValidationLogic logic)
        {
            rule.ManifestValue.IdentityAccessor = null;
            var context = new RuleContext(rule, ruleIdentifier, actualValue, Enumerable.Empty<ValueContext>(), ruleInterface);
            var validationRuleResult = new ValidationRuleResult(ruleResult, context, logic);
            var sut = new RuleResultIsForDescendentOfValue(otherValue);

            Assert.That(() => sut.Matches(validationRuleResult), Is.False);
        }

        [Test,AutoMoqData]
        public void GetExpressionShouldReturnAnExpressionWhichReturnsTrueForAResultWhichMatchesAnAncestorContextWhenAllowAncestorsIsTrue([ManifestModel] ManifestValue value,
                                                                                                                                         [ManifestModel] ManifestValue otherValue,
                                                                                                                                         [ManifestModel] ManifestRule rule,
                                                                                                                                         [RuleId] RuleIdentifier ruleIdentifier,
                                                                                                                                         object identity,
                                                                                                                                         object actualValue,
                                                                                                                                         Type ruleInterface,
                                                                                                                                         [RuleResult] RuleResult ruleResult,
                                                                                                                                         IValidationLogic logic)
        {
            value.IdentityAccessor = null;
            var context = new RuleContext(rule, ruleIdentifier, actualValue, new [] { new ValueContext(identity, actualValue, value) }, ruleInterface);
            var validationRuleResult = new ValidationRuleResult(ruleResult, context, logic);
            var sut = new RuleResultIsForDescendentOfValue(value, true);

            Assert.That(() => sut.Matches(validationRuleResult), Is.True);
        }

        [Test,AutoMoqData]
        public void GetExpressionShouldReturnAnExpressionWhichReturnsFalseForAResultWhichMatchesAnAncestorContextWhenAllowAncestorsIsFalse([ManifestModel] ManifestValue value,
                                                                                                                                         [ManifestModel] ManifestValue otherValue,
                                                                                                                                         [ManifestModel] ManifestRule rule,
                                                                                                                                         [RuleId] RuleIdentifier ruleIdentifier,
                                                                                                                                         object identity,
                                                                                                                                         object actualValue,
                                                                                                                                         Type ruleInterface,
                                                                                                                                         [RuleResult] RuleResult ruleResult,
                                                                                                                                         IValidationLogic logic)
        {
            value.IdentityAccessor = null;
            var context = new RuleContext(rule, ruleIdentifier, actualValue, new [] { new ValueContext(identity, actualValue, value) }, ruleInterface);
            var validationRuleResult = new ValidationRuleResult(ruleResult, context, logic);
            var sut = new RuleResultIsForDescendentOfValue(value, false);

            Assert.That(() => sut.Matches(validationRuleResult), Is.False);
        }
    }
}