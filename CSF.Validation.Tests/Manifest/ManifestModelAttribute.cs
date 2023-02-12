using System.Reflection;
using AutoFixture;
using AutoFixture.NUnit3;

namespace CSF.Validation.Manifest
{
    /// <summary>
    /// Instructs Autofixture to create validation manifest objects in a simple but valid manner
    /// which does not raise internal exceptions.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This affects the following types:
    /// </para>
    /// <list type="bullet">
    /// <item><description><see cref="ManifestRule"/></description></item>
    /// <item><description><see cref="ManifestRuleIdentifier"/></description></item>
    /// <item><description><see cref="ManifestItem"/></description></item>
    /// </list>
    /// </remarks>
    public class ManifestModelAttribute : CustomizeAttribute
    {
        public override ICustomization GetCustomization(ParameterInfo parameter)
            => new ManifestModelCustomization();
    }
}