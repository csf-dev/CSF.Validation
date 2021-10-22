using System.Threading;
using System.Threading.Tasks;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// A factory service which creates instances of <see cref="IExecutesAllRules"/> based upon the specified validation options.
    /// </summary>
    public class RuleExecutorFactory : IGetsRuleExecutor
    {
        /// <summary>
        /// Gets the rule-execution service using an async API.
        /// </summary>
        /// <param name="options">The validation options.</param>
        /// <param name="token">An optional cancellation token.</param>
        /// <returns>A task which contains the rule-execution service implementation.</returns>
        public Task<IExecutesAllRules> GetRuleExecutorAsync(ValidationOptions options, CancellationToken token = default)
        {
            throw new System.NotImplementedException();
        }
    }
}