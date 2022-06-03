using System.Collections.Generic;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// An object which provides context for the process of executing validation rules.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This object assists the validation process by tracking which rules have been executed and which have not.
    /// It also tracks the dependencies between rules, so that <see cref="GetRulesWhichMayBeExecuted"/> will only
    /// return those which either have no dependencies or where all of their dependencies have already been executed.
    /// </para>
    /// <para>
    /// Use <see cref="HandleValidationRuleResult(ExecutableRule)"/> after each rule completes (regardless of its
    /// outcome) in order to update this object so that it may return the appropriate result for future invocations
    /// of <see cref="GetRulesWhichMayBeExecuted"/>.
    /// </para>
    /// </remarks>
    public interface IRuleExecutionContext
    {
        /// <summary>
        /// Gets a read-only collection of all of the available validation rules and their dependencies.
        /// </summary>
        IReadOnlyCollection<ExecutableRuleAndDependencies> AllRules { get; }

        /// <summary>
        /// Gets a collection of rules which are ready to be executed.  Either they have no dependencies or
        /// their dependencies have executed and passed.
        /// </summary>
        /// <returns>A collection of rules that are ready to execute.</returns>
        IEnumerable<ExecutableRule> GetRulesWhichMayBeExecuted();

        /// <summary>
        /// Gets a collection of rules which should not be executed because their dependency rule(s)
        /// have failed.
        /// </summary>
        /// <returns>A collection of rules which have failed dependencies.</returns>
        IEnumerable<ExecutableRule> GetRulesWhoseDependenciesHaveFailed();

        /// <summary>
        /// Handles the alteration of dependency chains and available rules based upon a single validation rule receiving a result.
        /// </summary>
        /// <param name="rule">The rule which now has a non-<see langword="null" /> <see cref="ExecutableRule.Result"/> property.</param>
        void HandleValidationRuleResult(ExecutableRule rule);
    }
}