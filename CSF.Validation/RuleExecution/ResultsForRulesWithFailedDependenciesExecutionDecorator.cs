using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Rules;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// A decorator for the interface <see cref="IExecutesAllRules"/> which populates the rule results
    /// for rules which have not been executed because one or more of their dependency rules did not
    /// result in a <see cref="Rules.RuleOutcome.Passed"/> outcome.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Rules which have failed dependencies are not executed, because their dependencies failed.
    /// That means that they won't automatically have results created by the validation process.
    /// This decorator creates results for all of those rules which have been skipped due to failed dependencies.
    /// All of those results will have the outcome <see cref="RuleOutcome.DependencyFailed"/>.
    /// </para>
    /// </remarks>
    public class ResultsForRulesWithFailedDependenciesExecutionDecorator : IExecutesAllRules
    {
        readonly IExecutesAllRules wrapped;
        readonly IGetsRuleContext contextFactory;

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<ValidationRuleResult>> ExecuteAllRulesAsync(IRuleExecutionContext executionContext, CancellationToken cancellationToken = default)
        {
            var results = (await wrapped.ExecuteAllRulesAsync(executionContext, cancellationToken).ConfigureAwait(false)).ToList();
            cancellationToken.ThrowIfCancellationRequested();

            var resultsForRulesWithFailedDependencies = executionContext
                .GetRulesWhoseDependenciesHaveFailed()
                .Select(x => new ValidationRuleResult(x.Result, contextFactory.GetRuleContext(x), x.RuleLogic));
            results.AddRange(resultsForRulesWithFailedDependencies);

            return results;
        }

        /// <summary>
        /// Initialises a new instance of <see cref="ResultsForRulesWithFailedDependenciesExecutionDecorator"/>.
        /// </summary>
        /// <param name="wrapped">The wrapped executor instance.</param>
        /// <param name="contextFactory">A rule context factory.</param>
        /// <exception cref="System.ArgumentNullException">If any parameter is <see langword="null" />.</exception>
        public ResultsForRulesWithFailedDependenciesExecutionDecorator(IExecutesAllRules wrapped, IGetsRuleContext contextFactory)
        {
            this.wrapped = wrapped ?? throw new System.ArgumentNullException(nameof(wrapped));
            this.contextFactory = contextFactory ?? throw new System.ArgumentNullException(nameof(contextFactory));
        }
    }
}