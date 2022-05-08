using System;
using System.Reflection;
using AutoFixture;
using AutoFixture.NUnit3;

namespace CSF.Validation
{
    /// <summary>
    /// AutoFixture customization attribute which creates an <see cref="IServiceProvider"/> that
    /// gets services from AutoFixture.
    /// </summary>
    public class AutofixtureServicesAttribute : CustomizeAttribute
    {
        public override ICustomization GetCustomization(ParameterInfo parameter)
            => new AutofixtureServicesCustomization();
    }
}