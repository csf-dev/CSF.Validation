using System;
using System.Collections.Generic;
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
        public ManifestRuleIdentifier Identifier { get; set; }
        
        /// <summary>
        /// Gets or sets an optional action which is used to configure the rule
        /// instance after it has been instantiated.
        /// </summary>
        public Action<object> RuleConfiguration { get; set; }

        /// <summary>
        /// Gets or sets an accessor function which is used to get the primary
        /// object under validation for this rule.  The input to this function is
        /// the original object under validation for the validator at the root of
        /// the manifest.
        /// </summary>
        public Func<object,object> ValidatedObjectAccessor { get; set; }

        /// <summary>
        /// Gets or sets an accessor function which is used to get a value to be
        /// validated for this rule.  The input to this function is
        /// the original object under validation for the validator at the root of
        /// the manifest.
        /// </summary>
        /// <remarks>
        /// <para>
        /// For a rule that does not validate a specific value, this property will be <see langword="null"/>.
        /// </para>
        /// </remarks>
        public Func<object,object> ValueAccessor { get; set; }

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
        public ICollection<ManifestRuleIdentifier> DependencyRules { get; set; }
    }
}