using System;
using System.Collections.Generic;
using System.Linq;
using CSF.Validation.Rules;
using static CSF.Validation.Resources.ExceptionMessages;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// An object which tracks which rules may be executed and which are still
    /// awaiting the execution of their dependencies.  This may also be used to get a
    /// list of the rules which should not be executed because their dependencies failed.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Note that this is a stateful service, so it is not suitable to re-use instances
    /// outside of the context of a validation run.  This object maintains and tracks the
    /// state of a validation operation throughout the context of that operation.
    /// For a new validation run, create a new instance of this service.
    /// </para>
    /// </remarks>
    public class RuleDependencyTracker : ITracksRuleDependencies
    {
        readonly IDictionary<ExecutableRule,ExecutableRuleAndDependencies> pendingRules;
        readonly IDictionary<ExecutableRule,ExecutableRuleAndDependencies> allRules;
        readonly ISet<ExecutableRule> dependencyFailures = new HashSet<ExecutableRule>();

        /// <summary>
        /// Gets a collection of rules which are ready to be executed.  Either they have no dependencies or
        /// their dependencies have executed and passed.
        /// </summary>
        /// <returns>A collection of rules that are ready to execute.</returns>
        public IEnumerable<ExecutableRule> GetRulesWhichMayBeExecuted()
        {
            return (from rule in pendingRules.Values
                    where
                        rule.ExecutableRule.Result is null
                     && rule.Dependencies.All(x => x.Result?.Outcome == RuleOutcome.Passed)
                    select rule.ExecutableRule)
                .ToList();
        }

        /// <summary>
        /// Gets a collection of rules which should not be executed because their dependency rule(s)
        /// have failed.
        /// </summary>
        /// <returns>A collection of rules which have failed dependencies.</returns>
        public IEnumerable<ExecutableRule> GetRulesWhoseDependenciesHaveFailed() => dependencyFailures;

        /// <summary>
        /// Handles the alteration of dependency chains and available rules based upon a single validation rule receiving a result.
        /// </summary>
        /// <param name="rule">The rule which now has a non-<see langword="null" /> <see cref="ExecutableRule.Result"/> property.</param>
        public void HandleValidationRuleResult(ExecutableRule rule)
        {
            if(rule.Result is null)
            {
                var message = String.Format(GetExceptionMessage("RuleResultMustNotBeNull"), nameof(ExecutableRule.Result));
                throw new ArgumentException(message, nameof(rule));
            }

            if(rule.Result.Outcome != RuleOutcome.Passed)
                RemoveRuleAndAllDependentsFromPending(rule);
            else
                pendingRules.Remove(rule);
        }

        /// <summary>
        /// When the specified <paramref name="rule"/> has failed then this rule and all of its dependencies must be
        /// removed from the collection of pending rules.  Additionally, any rules removed in this way that do not yet
        /// have a <see cref="ExecutableRule.Result"/> must be marked with a result indicating
        /// <see cref="RuleOutcome.DependencyFailed"/>.
        /// </summary>
        /// <param name="rule">The rule which has failed validation in some manner.</param>
        void RemoveRuleAndAllDependentsFromPending(ExecutableRule rule)
        {
            var openList = new Queue<ExecutableRule>(GetRuleAndDependencies(rule).DependedUponBy);

            while(openList.Any())
            {
                var current = openList.Dequeue();
                pendingRules.Remove(current);

                var ruleAndDependencies = GetRuleAndDependencies(current);
                if(ruleAndDependencies.ExecutableRule.Result is null)
                {
                    ruleAndDependencies.ExecutableRule.Result = new RuleResult(RuleOutcome.DependencyFailed);
                    dependencyFailures.Add(ruleAndDependencies.ExecutableRule);
                }

                foreach(var dependedUpon in ruleAndDependencies.DependedUponBy)
                    openList.Enqueue(dependedUpon);
            }
        }

        ExecutableRuleAndDependencies GetRuleAndDependencies(ExecutableRule rule) => allRules[rule];

        /// <summary>
        /// Initialises a new instance of <see cref="RuleDependencyTracker"/>.
        /// </summary>
        /// <param name="allRules">A collection of all of the rules which could be executed.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="allRules"/> is <see langword="null" />.</exception>
        public RuleDependencyTracker(IEnumerable<ExecutableRuleAndDependencies> allRules)
        {
            if (allRules is null)
                throw new ArgumentNullException(nameof(allRules));

            this.allRules = allRules.ToDictionary(k => k.ExecutableRule, v => v);
            pendingRules = new Dictionary<ExecutableRule, ExecutableRuleAndDependencies>(this.allRules);
        }
    }
}