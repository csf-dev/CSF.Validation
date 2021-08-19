using System;
using System.Reflection;
using AutoFixture;
using AutoFixture.NUnit3;

namespace CSF.Validation
{
    public class IntegrationTestingAttribute : CustomizeAttribute
    {
        public override ICustomization GetCustomization(ParameterInfo parameter)
            => new IntegrationTestingCustomization();
    }
}