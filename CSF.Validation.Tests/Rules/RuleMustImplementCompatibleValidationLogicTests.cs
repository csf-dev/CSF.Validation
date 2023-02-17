using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Manifest;
using NUnit.Framework;

namespace CSF.Validation.Rules
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class RuleMustImplementCompatibleValidationLogicTests
    {
        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfValidatedTypeOfManifestValueIsNull(RuleMustImplementCompatibleValidationLogic sut,
                                                                                       [ManifestModel] ManifestRule rule,
                                                                                       [RuleContext] RuleContext context)
        {
            rule.ManifestValue.ValidatedType = null;
            rule.ManifestValue.Parent = null;
            Assert.That(() => sut.GetResultAsync(rule, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassResultIfRuleLogicImplementsSingleRuleInterface(RuleMustImplementCompatibleValidationLogic sut,
                                                                                                 [ManifestModel] ManifestItem value,
                                                                                                 [RuleContext] RuleContext context)
        {
            value.ValidatedType = typeof(int);
            value.Parent = null;
            var rule = new ManifestRule(value, new ManifestRuleIdentifier(value, typeof(IntegerRule)));
            Assert.That(() => sut.GetResultAsync(rule, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassResultIfRuleLogicImplementsDoubleRuleInterface(RuleMustImplementCompatibleValidationLogic sut,
                                                                                                 [ManifestModel] ManifestItem value,
                                                                                                 [ManifestModel] ManifestItem parent,
                                                                                                 [RuleContext] RuleContext context)
        {
            value.ValidatedType = typeof(int);
            value.Parent = parent;
            parent.ValidatedType = typeof(object);
            var rule = new ManifestRule(value, new ManifestRuleIdentifier(value, typeof(IntegerAndObjectRule)));
            Assert.That(() => sut.GetResultAsync(rule, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassResultIfRuleLogicImplementsSingleRuleInterfaceWhenManifestHasAParent(RuleMustImplementCompatibleValidationLogic sut,
                                                                                                                       [ManifestModel] ManifestItem value,
                                                                                                                       [ManifestModel] ManifestItem parent,
                                                                                                                       [RuleContext] RuleContext context)
        {
            value.ValidatedType = typeof(int);
            value.Parent = parent;
            parent.ValidatedType = typeof(object);
            var rule = new ManifestRule(value, new ManifestRuleIdentifier(value, typeof(IntegerRule)));
            Assert.That(() => sut.GetResultAsync(rule, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetFailureMessageAsyncShouldReturnCorrectMessageWhenOneCandidateInterface(RuleMustImplementCompatibleValidationLogic sut,
                                                                                              [ManifestModel] ManifestItem value,
                                                                                              [RuleResult] ValidationRuleResult result)
        {
            value.ValidatedType = typeof(decimal);
            value.Parent = null;
            var rule = new ManifestRule(value, new ManifestRuleIdentifier(value, typeof(IntegerRule)));
            Assert.That(async () => await sut.GetFailureMessageAsync(rule, result),
                        Is.EqualTo("The rule logic type (Identifier.RuleType) must implement IRule<System.Decimal>, either directly or contravariantly via a less-specific interface. The type CSF.Validation.Rules.RuleMustImplementCompatibleValidationLogicTests+IntegerRule does not implement this interface, though."));
        }

        [Test,AutoMoqData]
        public void GetFailureMessageAsyncShouldReturnCorrectMessageWhenTwoCandidateInterfaces(RuleMustImplementCompatibleValidationLogic sut,
                                                                                               [ManifestModel] ManifestItem value,
                                                                                               [ManifestModel] ManifestItem parent,
                                                                                               [RuleResult] ValidationRuleResult result)
        {
            value.ValidatedType = typeof(decimal);
            value.Parent = parent;
            parent.ValidatedType = typeof(object);
            var rule = new ManifestRule(value, new ManifestRuleIdentifier(value, typeof(IntegerRule)));
            Assert.That(async () => await sut.GetFailureMessageAsync(rule, result),
                        Is.EqualTo("The rule logic type (Identifier.RuleType) must implement one of IRule<System.Decimal> or IRule<System.Decimal,System.Object>, either directly or contravariantly via a less-specific interface. The type CSF.Validation.Rules.RuleMustImplementCompatibleValidationLogicTests+IntegerRule does not implement either of these interfaces, though."));
        }

        [Test,AutoMoqData]
        public void GetFailureMessageAsyncShouldThrowIfNoCandidateInterfaces(RuleMustImplementCompatibleValidationLogic sut,
                                                                             [ManifestModel] ManifestItem value,
                                                                             [ManifestModel] ManifestItem otherValue,
                                                                             [RuleResult] ValidationRuleResult result)
        {
            value.ValidatedType = null;
            value.Parent = null;
            var rule = new ManifestRule(value, new ManifestRuleIdentifier(otherValue, typeof(IntegerRule)));
            Assert.That(async () => await sut.GetFailureMessageAsync(rule, result), Throws.ArgumentException);
        }


        public class IntegerRule : IRule<int>
        {
            public ValueTask<RuleResult> GetResultAsync(int validated, RuleContext context, CancellationToken token = default)
                => throw new System.NotImplementedException();
        }

        public class IntegerAndObjectRule : IRule<int,object>
        {
            public ValueTask<RuleResult> GetResultAsync(int value, object parentValue, RuleContext context, CancellationToken token = default)
                => throw new System.NotImplementedException();
        }
    }
}