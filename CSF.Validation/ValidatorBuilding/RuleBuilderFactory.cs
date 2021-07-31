using System;

namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// A factory service which gets a <see cref="RuleBuilder{TRule}"/> from a specified
    /// context and rule definition action.
    /// </summary>
    public class RuleBuilderFactory : IGetsRuleBuilder
    {
        readonly Func<IGetsManifestRuleIdentifierFromRelativeIdentifier> manifestIdentifierFactory;

        /// <summary>
        /// Gets a rule builder instance from the specified context and rule-definition action.
        /// </summary>
        /// <typeparam name="TRule">The type of object which the rule validates.</typeparam>
        /// <param name="context">Contextual information from which to build this rule.</param>
        /// <param name="ruleDefinition">An optional configuration/definition action which will be used to customise the rule.</param>
        /// <returns>A rule builder object.</returns>
        public IBuildsRule<TRule> GetRuleBuilder<TRule>(RuleBuilderContext context, Action<IConfiguresRule<TRule>> ruleDefinition)
        {
            var builder = new RuleBuilder<TRule>(context, manifestIdentifierFactory());
            if(!(ruleDefinition is null))
                ruleDefinition(builder);
            return builder;
        }

        /// <summary>
        /// Initialises a new instance of <see cref="RuleBuilderFactory"/>.
        /// </summary>
        /// <param name="manifestIdentifierFactory">A factory function which gets a manifest identifier service.</param>
        public RuleBuilderFactory(Func<IGetsManifestRuleIdentifierFromRelativeIdentifier> manifestIdentifierFactory)
        {
            this.manifestIdentifierFactory = manifestIdentifierFactory ?? throw new ArgumentNullException(nameof(manifestIdentifierFactory));
        }
    }
}