using System;
using System.Collections.Generic;
using System.Linq;
using CSF.Validation.Manifest;

namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// A builder which is used to define &amp; configure a single instance of a validation rule.
    /// </summary>
    /// <typeparam name="TRule">The concrete type of the configured validation rule.</typeparam>
    public class RuleBuilder<TRule> : IConfiguresRule<TRule>, IConfiguresContext
    {
        readonly IGetsManifestRuleIdentifierFromRelativeIdentifier relativeToManifestId;
        readonly IGetsManifestRuleIdentifier identifierFactory;

        ICollection<RelativeRuleIdentifier> dependencies = new HashSet<RelativeRuleIdentifier>();
        Action<TRule> ruleConfig = r => { };

        /// <summary>
        /// Gets or sets an optional rule name.  This may be used to differentiate different instances
        /// of the same rule (but with different configuration), applied to the same member or validated object.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a collection of the relative identifiers of other validation rules upon which the current
        /// rule depends.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Validation rule dependencies work in two ways:
        /// </para>
        /// <list type="bullet">
        /// <item>
        /// <description>The current rule will not be executed until all of its dependencies have been executed.</description>
        /// </item>
        /// <item>
        /// <description>
        /// If any of the current rule's dependencies does not complete with a <see cref="CSF.Validation.Rules.RuleOutcome.Passed"/> outcome
        /// then the current rule's execution will be skipped and automatically recorded as having a <see cref="CSF.Validation.Rules.RuleOutcome.DependencyFailed"/>
        /// outcome.
        /// </description>
        /// </item>
        /// </list>
        /// <para>
        /// In case it is not obvious, specifying a circular set of validation rule dependencies is not allowed.
        /// </para>
        /// </remarks>
        public ICollection<RelativeRuleIdentifier> Dependencies
        {
            get => dependencies;
            set => dependencies = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Specifies an action to be executed in order to configure the rule before it is executed.
        /// </summary>
        /// <param name="ruleConfig">The rule configuration action.</param>
        public void ConfigureRule(Action<TRule> ruleConfig)
        {
            this.ruleConfig = ruleConfig ?? (r => { });
        }

        /// <inheritdoc/>
        public void ConfigureContext(ValidatorBuilderContext context)
        {
            var identifier = identifierFactory.GetManifestRuleIdentifier(typeof(TRule), context, Name);
            var dependencyRules = Dependencies
                .Select(relativeIdentifier => relativeToManifestId.GetManifestRuleIdentifier(context.ManifestValue, relativeIdentifier))
                .ToList();

            var rule = new ManifestRule(context.ManifestValue, identifier)
            {
                RuleConfiguration = r => ruleConfig((TRule) r),
                DependencyRules = dependencyRules,
            };
            context.ManifestValue.Rules.Add(rule);
        }

        /// <summary>
        /// Initialises an instance of <see cref="RuleBuilder{TRule}"/>.
        /// </summary>
        /// <param name="relativeToManifestIdentityConverter">A service which converts relative rule identifiers to absolute ones.</param>
        /// <param name="identifierFactory">A factory which creates manifest identifiers.</param>
        public RuleBuilder(IGetsManifestRuleIdentifierFromRelativeIdentifier relativeToManifestIdentityConverter,
                           IGetsManifestRuleIdentifier identifierFactory)
        {
            this.relativeToManifestId = relativeToManifestIdentityConverter ?? throw new ArgumentNullException(nameof(relativeToManifestIdentityConverter));
            this.identifierFactory = identifierFactory ?? throw new ArgumentNullException(nameof(identifierFactory));
        }
    }
}