using System;

namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// An object which gets a <see cref="RuleBuilder{TRule}"/> from a specified
    /// context and rule definition action.
    /// </summary>
    public interface IGetsRuleBuilder
    {
        /// <summary>
        /// Gets a rule builder instance from the specified context and rule-definition action.
        /// </summary>
        /// <typeparam name="TRule">The type of object which the rule validates.</typeparam>
        /// <param name="context">Contextual information from which to build this rule.</param>
        /// <param name="ruleDefinition">An optional configuration/definition action which will be used to customise the rule.</param>
        /// <returns>A rule builder object.</returns>
        IBuildsRule<TRule> GetRuleBuilder<TRule>(RuleBuilderContext context, Action<IConfiguresRule<TRule>> ruleDefinition);
    }
}