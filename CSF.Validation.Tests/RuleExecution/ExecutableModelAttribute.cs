using System.Reflection;
using AutoFixture;
using AutoFixture.NUnit3;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// Used to customize <see cref="ValidatedValue"/> or <see cref="ExecutableRule"/>.
    /// </summary>
    public class ExecutableModelAttribute : CustomizeAttribute
    {
        public override ICustomization GetCustomization(ParameterInfo parameter)
            => new ExecutableModelCustomisation();
    }
}