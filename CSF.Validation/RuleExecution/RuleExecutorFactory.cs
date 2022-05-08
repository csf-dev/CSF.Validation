using System;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Rules;
using Microsoft.Extensions.DependencyInjection;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// A factory service which creates instances of <see cref="IExecutesAllRules"/> based upon the specified validation options.
    /// </summary>
    public class RuleExecutorFactory : IGetsRuleExecutor
    {
        readonly IServiceProvider resolver;

        /// <summary>
        /// Gets the rule-execution service using an async API.
        /// </summary>
        /// <param name="options">The validation options.</param>
        /// <param name="token">An optional cancellation token.</param>
        /// <returns>A task which contains the rule-execution service implementation.</returns>
        public Task<IExecutesAllRules> GetRuleExecutorAsync(ValidationOptions options, CancellationToken token = default)
        {
            IExecutesAllRules result;

            result = new SerialRuleExecutor(resolver.GetRequiredService<IGetsRuleDependencyTracker>(),
                                            resolver.GetRequiredService<IGetsSingleRuleExecutor>(),
                                            options,
                                            resolver.GetRequiredService<IGetsRuleContext>());
            
            return Task.FromResult(result);
        }

        /// <summary>
        /// Initialises a new instance of <see cref="RuleExecutorFactory"/>.
        /// </summary>
        /// <param name="resolver">A service resolver.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="resolver"/> is <see langword="null" />.</exception>
        public RuleExecutorFactory(IServiceProvider resolver)
        {
            this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
        }
    }
}