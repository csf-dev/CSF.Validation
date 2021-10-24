using System.Collections.Generic;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// An object which both gets a collection of validation results and also updates the <see cref="ExecutableRule"/>
    /// instances for all rules which have failures in their dependency rules (and thus should not be executed).
    /// </summary>
    public interface IGetsResultsAndUpdatesRulesWhichHaveDependencyFailures
    {
        /// <summary>
        /// Gets a collection of validation rule results and also updates the underlying <see cref="ExecutableRule"/>
        /// objects for rules which should not run because of dependency failures.
        /// </summary>
        /// <param name="dependencyTracker">An object that tracks dependencies.</param>
        /// <returns>A collection of validation rule results.</returns>
        IEnumerable<ValidationRuleResult> GetResultsAndUpdateRules(ITracksRuleDependencies dependencyTracker);
    }
}