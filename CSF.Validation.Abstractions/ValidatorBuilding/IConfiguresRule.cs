using System;
using System.Collections.Generic;

namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// A builder service which is used to define &amp; configure a single instance of a validation rule.
    /// </summary>
    /// <typeparam name="TRule">The concrete type of the configured validation rule.</typeparam>
    public interface IConfiguresRule<TRule>
    {
        /// <summary>
        /// Specifies an action to be executed in order to configure the rule before it is executed.
        /// </summary>
        /// <param name="ruleConfig">The rule configuration action.</param>
        void ConfigureRule(Action<TRule> ruleConfig);

        /// <summary>
        /// Gets or sets an optional rule name.  This may be used to differentiate different instances
        /// of the same rule (but with different configuration), applied to the same member or validated object.
        /// </summary>
        string Name { get; set; }

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
        ICollection<RelativeRuleIdentifier> Dependencies { get; set; }
    }
}