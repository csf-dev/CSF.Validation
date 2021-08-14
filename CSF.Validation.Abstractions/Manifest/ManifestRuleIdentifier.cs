using System;
using CSF.Validation.Rules;

namespace CSF.Validation.Manifest
{
    /// <summary>
    /// A model for information which uniquely identifies a validation rule within a validation manifest.
    /// </summary>
    public class ManifestRuleIdentifier : RuleIdentifierBase, IEquatable<ManifestRuleIdentifier>
    {
        /// <summary>
        /// Gets the manifest object to which this rule relates.
        /// </summary>
        public ManifestValue ManifestValue { get; }

        /// <summary>
        /// Gets a value that indicates whether the specified <see cref="ManifestRuleIdentifier"/>
        /// is equal to the current instance or not.
        /// </summary>
        /// <param name="other">A manifest rule identifier.</param>
        /// <returns><see langword="true"/> if the specified instance is equal to the current one; <see langword="false"/> otherwise.</returns>
        public bool Equals(ManifestRuleIdentifier other)
        {
            if(other is null) return false;

            return other.ManifestValue == ManifestValue
                && other.RuleName == RuleName
                && other.RuleType == RuleType;
        }

        /// <summary>
        /// Gets a value that indicates whether the specified object
        /// is equal to the current instance or not.
        /// </summary>
        /// <param name="obj">An object.</param>
        /// <returns><see langword="true"/> if the specified object is equal to the current instance; <see langword="false"/> otherwise.</returns>
        public override bool Equals(object obj) => Equals(obj as ManifestRuleIdentifier);

        /// <summary>
        /// Gets a hash value for the current instance.
        /// </summary>
        /// <returns>The hash value.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + ManifestValue.GetHashCode();
                hash = hash * 23 + RuleType.GetHashCode();
                hash = hash * 23 + (RuleName?.GetHashCode() ?? 0);
                return hash;
            }
        }

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