using System;
using AutoFixture;
using CSF.Validation.ManifestModel;
using CSF.Validation.ValidatorBuilding;

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
    public class ManifestModelCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<ManifestItem>(c =>
            {
                return c
                    .Without(x => x.Parent)
                    .Without(x => x.Children)
                    .Without(x => x.Rules)
                    .Without(x => x.CollectionItemValue)
                    .Without(x => x.PolymorphicTypes)
                    .Without(x => x.RecursiveAncestor)
                    .Without(x => x.ItemType);
            });
            fixture.Customize<ManifestRuleIdentifier>(c => c.FromFactory((ManifestItem val, Type ruleType) => new ManifestRuleIdentifier(val, ruleType)));
            fixture.Customize<ManifestRule>(c => {
                return c
                    .FromFactory((ManifestItem val, ManifestRuleIdentifier id) => new ManifestRule(val, id))
                    .Without(x => x.DependencyRules);
            });
            fixture.Customize<Value>(c =>
            {
                return c
                    .Without(x => x.Children)
                    .Without(x => x.Rules)
                    .Without(x => x.CollectionItemValue)
                    .Without(x => x.PolymorphicValues)
                    .Without(x => x.ValidateRecursivelyAsAncestor);
            });
            fixture.Customize<PolymorphicValue>(c => c.Without(x => x.Children).Without(x => x.Rules).Without(x => x.CollectionItemValue));
            fixture.Customize<Rule>(c => c.Without(x => x.Dependencies));
            fixture.Customize<ValidatorBuilderContext>(c => c.FromFactory((ManifestItem val) => new ValidatorBuilderContext(val)));
            fixture.Customize<ModelToManifestConversionContext>(c =>
            {
                return c
                    .FromFactory((Value v) => new ModelToManifestConversionContext { CurrentValue = v })
                    .Without(x => x.CurrentValue)
                    .Without(x => x.ConversionType);
            });
        }
    }
}