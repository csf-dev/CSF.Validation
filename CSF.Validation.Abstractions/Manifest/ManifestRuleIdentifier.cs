using System;
using CSF.Validation.Rules;

namespace CSF.Validation.Manifest
{
    /// <summary>
    /// A model for information which uniquely identifies a validation rule within a validation manifest.
    /// </summary>
    public class ManifestRuleIdentifier : RuleIdentifierBase
    {
        /// <summary>
        /// Gets the manifest object to which this rule relates.
        /// </summary>
        public ManifestValue ManifestValue { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="RuleIdentifier"/>.
        /// </summary>
        /// <param name="manifestValue">The manifest value/object which 'contains' this rule.</param>
        /// <param name="ruleType">The rule type.</param>
        /// <param name="ruleName">An optional rule name.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="ruleType"/> or <paramref name="manifestValue"/> are <see langword="null"/>.</exception>
        public ManifestRuleIdentifier(ManifestValue manifestValue,
                                      Type ruleType,
                                      string ruleName = default) : base(ruleType, ruleName)
        {
            ManifestValue = manifestValue ?? throw new ArgumentNullException(nameof(manifestValue));
        }
    }
}