using System;
using AutoFixture;
using CSF.Validation.ManifestModel;

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
    /// <item><description><see cref="ManifestValue"/></description></item>
    /// </list>
    /// </remarks>
    public class ManifestModelCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<ManifestValue>(c => c.Without(x => x.Parent).Without(x => x.Children).Without(x => x.Rules));
            fixture.Customize<ManifestRuleIdentifier>(c => c.FromFactory((ManifestValue val, Type ruleType) => new ManifestRuleIdentifier(val, ruleType)));
            fixture.Customize<ManifestRule>(c => {
                return c
                    .FromFactory((ManifestValue val, ManifestRuleIdentifier id) => new ManifestRule(val, id))
                    .Without(x => x.DependencyRules);
            });
            fixture.Customize<Value>(c => c.Without(x => x.Children).Without(x => x.Rules).With(x => x.EnumerateItems, false));
            fixture.Customize<Rule>(c => c.Without(x => x.Dependencies));
        }
    }
}