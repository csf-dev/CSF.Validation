using System;
using AutoFixture;
using CSF.Validation.Manifest;

namespace CSF.Validation.Autofixture
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
    /// <item><description><see cref="ManifestValue"/></description></item>
    /// </list>
    /// </remarks>
    public class ManifestModelCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<ManifestValue>(c => c.FromFactory(() => new ManifestValue()));
            fixture.Customize<ManifestRuleIdentifier>(c => c.FromFactory((ManifestValue val, Type ruleType) => new ManifestRuleIdentifier(val, ruleType)));
            fixture.Customize<ManifestRule>(c => c.FromFactory((ManifestRuleIdentifier id) => new ManifestRule(id)));
        }
    }
}