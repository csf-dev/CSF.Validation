using System;
using CSF.Validation.Manifest;

namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// A factory service for creating instances of <see cref="ManifestRuleIdentifier"/>.
    /// </summary>
    public class ManifestIdentifierFactory : IGetsManifestRuleIdentifier
    {
        /// <summary>
        /// Gets the manifest rule identifier for the specified rule type and context.
        /// </summary>
        /// <param name="ruleType">The type of the validation rule.</param>
        /// <param name="context">Contextual information about how validation rules should be built.</param>
        /// <param name="name">An optional name for the rule.</param>
        /// <returns>A manifest rule identifier.</returns>
        public ManifestRuleIdentifier GetManifestRuleIdentifier(Type ruleType, ValidatorBuilderContext context, string name = null)
            => new ManifestRuleIdentifier(context.ManifestValue, ruleType, name);
    }
}