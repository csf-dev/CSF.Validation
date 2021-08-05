using System;
using System.Collections.Generic;
using System.Linq;
using CSF.Validation.Rules;

namespace CSF.Validation.Manifest
{
    /// <summary>
    /// A model for a single validation rule within a <see cref="ValidationManifest"/>.
    /// </summary>
    public class ManifestRule
    {
        ICollection<ManifestRuleIdentifier> dependencyRules = new List<ManifestRuleIdentifier>();
        Action<object> ruleConfiguration = o => { };

        /// <summary>
        /// Gets the manifest value to which this rule applies.
        /// </summary>
        public ManifestValue ManifestValue { get; }

        /// <summary>
        /// Gets or sets the rule's unique identifier, including its type.
        /// </summary>
        public ManifestRuleIdentifier Identifier { get; }

        /// <summary>
        /// Gets or sets an optional action which is used to configure the rule
        /// instance after it has been instantiated.
        /// </summary>
        public Action<object> RuleConfiguration
        {
            get => ruleConfiguration;
            set => ruleConfiguration = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets or sets a collection of identifiers upon which the current rule depends.
        /// </summary>
        /// <remarks>
        /// <para>
        /// A dependency rule is a rule which must have executed and passed before the current rule may be executed.
        /// If the dependency rule does not complete with a <see cref="RuleOutcome.Passed"/> outcome, then the current
        /// rule will not be executed and will automatically be recorded with a <see cref="RuleOutcome.DependencyFailed"/>
        /// outcome.
        /// </para>
        /// </remarks>
        public ICollection<ManifestRuleIdentifier> DependencyRules
        {
            get => dependencyRules;
            set => dependencyRules = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Initialises a new instance of <see cref="ManifestRule"/>.
        /// </summary>
        /// <param name="manifestValue">The manifest value for which this rule applies.</param>
        /// <param name="identifier">The identifier for this rule.</param>
        public ManifestRule(ManifestValue manifestValue, ManifestRuleIdentifier identifier)
        {
            ManifestValue = manifestValue ?? throw new ArgumentNullException(nameof(manifestValue));
            Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
        }
    }
}