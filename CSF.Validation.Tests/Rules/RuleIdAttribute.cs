using System.Reflection;
using AutoFixture;
using AutoFixture.NUnit3;

namespace CSF.Validation.Rules
{
    public class RuleIdAttribute : CustomizeAttribute
    {
        public override ICustomization GetCustomization(ParameterInfo parameter)
            => new RuleIdCustomization();
    }
}