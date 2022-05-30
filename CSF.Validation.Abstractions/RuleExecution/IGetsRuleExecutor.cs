using System.Threading;
using System.Threading.Tasks;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// An object which gets a service that executes validation rules.
    /// </summary>
    /// <remarks>
    /// <para>
    /// There may be more than one implementation of <see cref="IExecutesAllRules"/> available, and this object
    /// allows the validation framework to choose between them, based upon the specified validation options.
    /// </para>
    /// </remarks>
    public interface IGetsRuleExecutor
    {
        /// <summary>
        /// Gets the rule-execution service using an async API.
        /// </summary>
        /// <param name="options">The validation options.</param>
        /// <param name="token">An optional cancellation token.</param>
        /// <returns>A task which contains the rule-execution service implementation.</returns>
        Task<IExecutesAllRules> GetRuleExecutorAsync(ResolvedValidationOptions options, CancellationToken token = default);
    }
}