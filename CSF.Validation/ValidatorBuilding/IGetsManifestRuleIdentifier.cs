using System;
using CSF.Validation.Manifest;

namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// An object which can create instances of <see cref="ManifestRuleIdentifier"/> for rules.
    /// </summary>
    public interface IGetsManifestRuleIdentifier
    {
        /// <summary>
        /// Gets the manifest rule identifier for the specified rule type and context.
        /// </summary>
        /// <param name="ruleType">The type of the validation rule.</param>
        /// <param name="context">Contextual information about how validation rules should be built.</param>
        /// <param name="name">An optional name for the rule.</param>
        /// <returns>A manifest rule identifier.</returns>
        ManifestRuleIdentifier GetManifestRuleIdentifier(Type ruleType, ValidatorBuilderContext context, string name = default);
    }
}