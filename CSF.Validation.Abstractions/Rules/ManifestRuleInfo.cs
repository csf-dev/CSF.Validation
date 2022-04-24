using System.Collections.Generic;
using CSF.Validation.Manifest;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// An immutable model which provides information about the configuration of a validation rule.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This type roughly corresponds to a <see cref="ManifestRule"/>.  The key difference between that and this 'info'
    /// class is that this type is immutable and presents a read-only API.
    /// </para>
    /// </remarks>
    public class ManifestRuleInfo
    {
        /// <summary>
        /// Gets information about the manifest value to which this rule applies.
        /// </summary>
        public ManifestValueInfo ManifestValue { get; }

        /// <summary>
        /// Gets the rule's unique identifier.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This value includes the rule's concrete type.
        /// </para>
        /// </remarks>
        public ManifestRuleIdentifier Identifier { get; }

        /// <summary>
        /// Gets a collection of identifiers upon which the current rule depends.
        /// </summary>
        /// <remarks>
        /// <para>
        /// A dependency rule is a rule which must have executed and passed before the current rule may be executed.
        /// If the dependency rule does not complete with a <see cref="RuleOutcome.Passed"/> outcome, then the current
        /// rule will not be executed and will automatically be recorded with a <see cref="RuleOutcome.DependencyFailed"/>
        /// outcome.
        /// </para>
        /// </remarks>
        public IReadOnlyCollection<ManifestRuleIdentifier> DependencyRules { get; }

        /// <summary>
        /// Initialises an instance of <see cref="ManifestRuleInfo"/>.
        /// This is essentially a copy-constructor for a <see cref="ManifestRule"/>.
        /// </summary>
        /// <param name="manifestRule">The manifest rule from which to create this instance.</param>
        /// <exception cref="System.ArgumentNullException">If <paramref name="manifestRule"/> is <see langword="null" />.</exception>
        public ManifestRuleInfo(ManifestRule manifestRule)
        {
            if (manifestRule is null)
                throw new System.ArgumentNullException(nameof(manifestRule));

            Identifier = manifestRule.Identifier;
            DependencyRules = new List<ManifestRuleIdentifier>(manifestRule.DependencyRules);
            ManifestValue = new ManifestValueInfo(manifestRule.ManifestValue);
        }
    }
}