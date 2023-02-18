using System;
using System.Collections.Generic;
using System.Reflection;
using AutoFixture;
using CSF.Validation.Manifest;
using CSF.Validation.RuleExecution;

namespace CSF.Validation.Rules
{
    public class RuleResultCustomization : ICustomization
    {
        readonly Type parameterType;
        readonly string parameterName;

        public object ValidatedValue { get; set; } = new object();

        public object ParentValue { get; set; } = new object();

        public Type RuleInterface { get; set; } = typeof(object);

        public RuleOutcome Outcome { get; set; } = RuleOutcome.Failed;

        public void Customize(IFixture fixture)
        {
            new RuleContextCustomization().Customize(fixture);
            new ExecutableModelCustomisation().Customize(fixture);
            fixture.CustomizeForParameter<ValidationRuleResult>(parameterType, parameterName, c => c.FromFactory(GetValidationRuleResultFunc));
            fixture.Customize<RuleResult>(c => c.FromFactory((Dictionary<string, object> data) => new RuleResult(Outcome, data)));
        }

        Func<RuleResult, ManifestRule, RuleIdentifier, IValidationLogic, ValidationRuleResult> GetValidationRuleResultFunc => GetValidationRuleResult;

        ValidationRuleResult GetValidationRuleResult(RuleResult result, ManifestRule manifestRule, RuleIdentifier ruleIdentifier, IValidationLogic logic)
        {
            var valueContext = new ValueContext(Guid.NewGuid(), ParentValue, manifestRule.ManifestValue);
            var context = new RuleContext(manifestRule, ruleIdentifier, ValidatedValue, new [] { valueContext }, RuleInterface);
            return new ValidationRuleResult(result, context, logic);
        }

        public RuleResultCustomization(ParameterInfo parameter) : this(parameter.ParameterType, parameter.Name) {}

        public RuleResultCustomization(Type parameterType, string parameterName)
        {
            this.parameterType = parameterType ?? throw new ArgumentNullException(nameof(parameterType));
            this.parameterName = parameterName ?? throw new ArgumentNullException(nameof(parameterName));
        }
    }
}