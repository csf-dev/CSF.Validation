using System.Reflection;
using AutoFixture;
using AutoFixture.NUnit3;

namespace CSF.Validation.IntegrationTests
{
    public class IntegrationValidatorFactoryAttribute : CustomizeAttribute
    {
        public override ICustomization GetCustomization(ParameterInfo parameter)
            => new IntegrationValidatorFactoryCustomisation();
    }
}