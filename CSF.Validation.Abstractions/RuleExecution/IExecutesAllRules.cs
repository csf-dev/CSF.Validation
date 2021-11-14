using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// An object which executes all of the validation rules using an async API and returns a collection of results.
    /// </summary>
    public interface IExecutesAllRules
    {
        /// <summary>
        /// Execute all of the specified validation rules and return their results.
        /// </summary>
        /// <param name="rules">The validation rules to be executed.</param>
        /// <param name="cancellationToken">An optional cancellation token.</param>
        /// <returns>A task containing a collection of the results from the executed validation rules.</returns>
        Task<IReadOnlyCollection<ValidationRuleResult>> ExecuteAllRulesAsync(IReadOnlyList<ExecutableRuleAndDependencies> rules,
                                                                             CancellationToken cancellationToken = default);
    }
}