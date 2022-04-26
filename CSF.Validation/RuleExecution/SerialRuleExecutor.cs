using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Rules;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// An implementation of <see cref="IExecutesAllRules"/> which executed each rule in serial with no parallelisation.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This service works by using a <see cref="ITracksRuleDependencies"/> instance for all of the available rules.
    /// This class uses <see cref="ITracksRuleDependencies.GetRulesWhichMayBeExecuted"/> to get a collection of available
    /// rules (which either have no dependencies or where all dependencies have passed).
    /// </para>
    /// <para>
    /// The logic in this class then executes all of these available rules and gets their results, storing them back
    /// against the original <see cref="ExecutableRule"/> object.  Once all of the available rules have been executed
    /// to their logical completion, another iteration is performed using <see cref="ITracksRuleDependencies.GetRulesWhichMayBeExecuted"/>
    /// to get the next collection of available rules.
    /// </para>
    /// <para>
    /// This process continues until the dependency-tracker object fails to yield any available rules, at which point
    /// the rule execution stops and further results are added based upon those rules which have dependency failures.
    /// </para>
    /// </remarks>
    public class SerialRuleExecutor : IExecutesAllRules
    {
        readonly IGetsRuleDependencyTracker dependencyTrackerFactory;
        readonly IGetsSingleRuleExecutor ruleExecutorFactory;
        readonly ValidationOptions options;
        readonly IGetsRuleContext contextFactory;

        /// <summary>
        /// Execute all of the specified validation rules and return their results.
        /// </summary>
        /// <param name="rules">The validation rules to be executed.</param>
        /// <param name="cancellationToken">An optional cancellation token.</param>
        /// <returns>A task containing a collection of the results from the executed validation rules.</returns>
        public async Task<IReadOnlyCollection<ValidationRuleResult>> ExecuteAllRulesAsync(IReadOnlyList<ExecutableRuleAndDependencies> rules, CancellationToken cancellationToken = default)
        {
            var ruleExecutor = ruleExecutorFactory.GetRuleExecutor(options);
            var dependencyTracker = dependencyTrackerFactory.GetDependencyTracker(rules, options);
            var results = new List<ValidationRuleResult>();

            for (var availableRules = dependencyTracker.GetRulesWhichMayBeExecuted();
                 availableRules.Any();
                 availableRules = dependencyTracker.GetRulesWhichMayBeExecuted())
            {
                var ruleResults = await ExecuteAvailableRulesAsync(availableRules, ruleExecutor, dependencyTracker, cancellationToken)
                    .ConfigureAwait(false);
                results.AddRange(ruleResults);
            }

            results.AddRange(GetResultsForRulesWithFailedDependencies(dependencyTracker));

            return results;
        }

        /// <summary>
        /// Gets a collection of <see cref="ValidationRuleResult"/> representing rules which have failed dependencies.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Rules which have failed dependencies are not executed, because their dependencies failed.
        /// That means that they won't automatically have results created by the validation process.
        /// This method creates results for all of those rules which have been skipped due to failed dependencies.
        /// All of those results will have the outcome <see cref="RuleOutcome.DependencyFailed"/>.
        /// </para>
        /// </remarks>
        /// <param name="dependencyTracker">A rule-dependency tracker.</param>
        /// <returns>A collection of <see cref="ValidationRuleResult"/>.</returns>
        IEnumerable<ValidationRuleResult> GetResultsForRulesWithFailedDependencies(ITracksRuleDependencies dependencyTracker)
        {
            return dependencyTracker
                .GetRulesWhoseDependenciesHaveFailed()
                .Select(x => new ValidationRuleResult(x.Result, contextFactory.GetRuleContext(x)));
        }

        static async Task<IEnumerable<ValidationRuleResult>> ExecuteAvailableRulesAsync(IEnumerable<ExecutableRule> availableRules,
                                                                                        IExeucutesSingleRule ruleExecutor,
                                                                                        ITracksRuleDependencies dependencyTracker,
                                                                                        CancellationToken cancellationToken)
        {
            var results = new List<ValidationRuleResult>();

            foreach(var rule in availableRules)
            {
                var result = await ruleExecutor.ExecuteRuleAsync(rule, cancellationToken).ConfigureAwait(false);
                rule.Result = result;
                dependencyTracker.HandleValidationRuleResult(rule);
                results.Add(result);
            }

            return results;
        }

        /// <summary>
        /// Initialises a new instance of <see cref="SerialRuleExecutor"/>.
        /// </summary>
        /// <param name="dependencyTrackerFactory">The factory for an object that tracks rule dependencies.</param>
        /// <param name="ruleExecutorFactory">The factory for an object which executes rules.</param>
        /// <param name="options">The validation options.</param>
        /// <param name="contextFactory">A factory for rule contexts.</param>
        /// <exception cref="ArgumentNullException">If any parameter value is <see langword="null" />.</exception>
        public SerialRuleExecutor(IGetsRuleDependencyTracker dependencyTrackerFactory,
                                  IGetsSingleRuleExecutor ruleExecutorFactory,
                                  ValidationOptions options,
                                  IGetsRuleContext contextFactory)
        {
            this.dependencyTrackerFactory = dependencyTrackerFactory ?? throw new ArgumentNullException(nameof(dependencyTrackerFactory));
            this.ruleExecutorFactory = ruleExecutorFactory ?? throw new ArgumentNullException(nameof(ruleExecutorFactory));
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        }
    }
}