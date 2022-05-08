using System.Reflection;
using AutoFixture;
using AutoFixture.NUnit3;

namespace CSF.Validation.Rules
{
    public class RuleResultAttribute : CustomizeAttribute
    {
        object validatedValue, parentValue;
        bool isValidatedValueSet, isParentValueSet;

        public object ValidatedValue
        {
            get => validatedValue;
            set {
                isValidatedValueSet = true;
                validatedValue = value;
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

        public override ICustomization GetCustomization(ParameterInfo parameter)
        {
            var customisation = new RuleResultCustomization(parameter);
            if(isValidatedValueSet)
                customisation.ValidatedValue = ValidatedValue;
            if(isParentValueSet)
                customisation.ParentValue = ParentValue;
            return customisation;
        }
    }
}