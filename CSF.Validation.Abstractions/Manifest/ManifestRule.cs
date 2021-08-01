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
        /// <summary>
        /// Gets or sets the rule's unique identifier, including its type.
        /// </summary>
        public ManifestRuleIdentifier Identifier { get; }
        
        /// <summary>
        /// Gets or sets an optional action which is used to configure the rule
        /// instance after it has been instantiated.
        /// </summary>
        public Action<object> RuleConfiguration { get; }

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
        public IReadOnlyCollection<ManifestRuleIdentifier> DependencyRules { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="ruleConfiguration"></param>
        /// <param name="dependencyRules"></param>
        public ManifestRule(ManifestRuleIdentifier identifier, Action<object> ruleConfiguration = default, IEnumerable<ManifestRuleIdentifier> dependencyRules = default)
        {
            Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
            RuleConfiguration = ruleConfiguration ?? (r => {});
            DependencyRules = new List<ManifestRuleIdentifier>(dependencyRules ?? Enumerable.Empty<ManifestRuleIdentifier>());
        }
    }
}