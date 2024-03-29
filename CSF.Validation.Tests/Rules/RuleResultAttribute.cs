using System;
using System.Reflection;
using AutoFixture;
using AutoFixture.NUnit3;

namespace CSF.Validation.Rules
{
    public class RuleResultAttribute : CustomizeAttribute
    {
        object validatedValue, parentValue;
        Type ruleInterface;
        RuleOutcome outcome;
        bool isValidatedValueSet, isParentValueSet, isRuleInterfaceSet, isOutcomeSet;

        public object ValidatedValue
        {
            get => validatedValue;
            set {
                isValidatedValueSet = true;
                validatedValue = value;
            }
        }

        public RuleOutcome Outcome
        {
            get => outcome;
            set {
                isOutcomeSet = true;
                outcome = value;
            }
        }

        public object ParentValue
        {
            get => parentValue;
            set {
                isParentValueSet = true;
                parentValue = value;
            }
        }

        public Type RuleInterface
        {
            get => ruleInterface;
            set {
                isRuleInterfaceSet = true;
                ruleInterface = value;
            }
        }

        public override ICustomization GetCustomization(ParameterInfo parameter)
        {
            var customisation = new RuleResultCustomization(parameter);

            if(isValidatedValueSet)
                customisation.ValidatedValue = ValidatedValue;
            if(isParentValueSet)
                customisation.ParentValue = ParentValue;
            if(isRuleInterfaceSet)
                customisation.RuleInterface = RuleInterface;
            if(isOutcomeSet)
                customisation.Outcome = Outcome;

            return customisation;
        }
    }
}