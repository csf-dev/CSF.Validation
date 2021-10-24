using System.Collections.Generic;
using CSF.Validation.Rules;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// A service which gets a collection of <see cref="ValidationRuleResult"/> from a <see cref="ITracksRuleDependencies"/>,
    /// using the <see cref="ITracksRuleDependencies.GetRulesWhoseDependenciesHaveFailed"/> method.
    /// As part of this process it updates <see cref="ExecutableRule.Result"/> for each of those rules.
    /// </summary>
    public class FailedDependencyUpdaterAndResultProvider : IGetsResultsAndUpdatesRulesWhichHaveDependencyFailures
    {
        /// <summary>
        /// Gets a collection of validation rule results and also updates the underlying <see cref="ExecutableRule"/>
        /// objects for rules which should not run because of dependency failures.
        /// </summary>
        /// <param name="dependencyTracker">An object that tracks dependencies.</param>
        /// <returns>A collection of validation rule results.</returns>
        public IEnumerable<ValidationRuleResult> GetResultsAndUpdateRules(ITracksRuleDependencies dependencyTracker)
        {
            var rulesWithDependencyFailures = dependencyTracker.GetRulesWhoseDependenciesHaveFailed();
            var results = new List<ValidationRuleResult>();

            foreach(var rule in rulesWithDependencyFailures)
            {
                var result = new ValidationRuleResult(rule.RuleIdentifier, RuleOutcome.DependencyFailed);
                rule.Result = result;
                results.Add(result);
            }

            return results;
        }
    }
}