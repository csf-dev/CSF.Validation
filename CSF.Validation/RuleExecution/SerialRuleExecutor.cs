using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// An implementation of <see cref="IExecutesAllRules"/> which executed each rule in serial with no parallelisation.
    /// </summary>
    public class SerialRuleExecutor : IExecutesAllRules
    {
        /// <summary>
        /// Execute all of the specified validation rules and return their results.
        /// </summary>
        /// <param name="rules">The validation rules to be executed.</param>
        /// <param name="cancellationToken">An optional cancellation token.</param>
        /// <returns>A task containing a collection of the results from the executed validation rules.</returns>
        public Task<IReadOnlyCollection<ValidationRuleResult>> ExecuteAllRulesAsync(IReadOnlyList<ExecutableRuleAndDependencies> rules, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }
    }
}