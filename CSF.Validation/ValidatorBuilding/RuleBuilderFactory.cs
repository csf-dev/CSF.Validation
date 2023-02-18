using System;

namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// A factory service which gets a <see cref="RuleBuilder{TRule}"/> from a specified
    /// context and rule definition action.
    /// </summary>
    public class RuleBuilderFactory : IGetsRuleBuilder
    {
        readonly Func<IGetsManifestRuleIdentifierFromRelativeIdentifier> manifestIdentifierConverter;
        readonly Func<IGetsManifestRuleIdentifier> manifestIdentifierFactory;

        /// <summary>
        /// Gets a rule builder instance from the specified context and rule-definition action.
        /// </summary>
        /// <typeparam name="TRule">The type of object which the rule validates.</typeparam>
        /// <param name="context">Contextual information from which to build this rule.</param>
        /// <param name="ruleDefinition">An optional configuration/definition action which will be used to customise the rule.</param>
        /// <returns>A rule builder object.</returns>
        public IConfiguresContext GetRuleBuilder<TRule>(ValidatorBuilderContext context, Action<IConfiguresRule<TRule>> ruleDefinition)
        {
            var builder = new RuleBuilder<TRule>(manifestIdentifierConverter(), manifestIdentifierFactory());
            if(!(ruleDefinition is null))
                ruleDefinition(builder);
            return builder;
        }

        /// <summary>
        /// Initialises a new instance of <see cref="RuleBuilderFactory"/>.
        /// </summary>
        /// <param name="manifestIdentifierConverter">A factory function which gets a manifest identifier conversion service.</param>
        /// <param name="manifestIdentifierFactory">A factory function which gets a manifest identifier factory service.</param>
        public RuleBuilderFactory(Func<IGetsManifestRuleIdentifierFromRelativeIdentifier> manifestIdentifierConverter,
                                  Func<IGetsManifestRuleIdentifier> manifestIdentifierFactory)
        {
            this.manifestIdentifierConverter = manifestIdentifierConverter ?? throw new ArgumentNullException(nameof(manifestIdentifierConverter));
            this.manifestIdentifierFactory = manifestIdentifierFactory ?? throw new ArgumentNullException(nameof(manifestIdentifierFactory));
        }
    }
}