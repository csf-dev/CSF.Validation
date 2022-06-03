using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// An implementation of <see cref="IExecutesAllRules"/> which executes each rule in serial with no parallelisation.
    /// </summary>
    public class SerialRuleExecutor : IExecutesAllRules
    {
        readonly IExeucutesSingleRule ruleExecutor;
        
        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<ValidationRuleResult>> ExecuteAllRulesAsync(IRuleExecutionContext executionContext, CancellationToken cancellationToken = default)
        {
            var results = new List<ValidationRuleResult>();

            for (var availableRules = executionContext.GetRulesWhichMayBeExecuted();
                 availableRules.Any();
                 availableRules = executionContext.GetRulesWhichMayBeExecuted())
            {
                var ruleResults = await ExecuteAvailableRulesAsync(availableRules, ruleExecutor, executionContext, cancellationToken)
                    .ConfigureAwait(false);
                results.AddRange(ruleResults);
            }

            return results;
        }

        static async Task<IEnumerable<ValidationRuleResult>> ExecuteAvailableRulesAsync(IEnumerable<ExecutableRule> availableRules,
                                                                                        IExeucutesSingleRule ruleExecutor,
                                                                                        IRuleExecutionContext executionContext,
                                                                                        CancellationToken cancellationToken)
        {
            var results = new List<ValidationRuleResult>();

            foreach(var rule in availableRules)
            {
                var result = await ruleExecutor.ExecuteRuleAsync(rule, cancellationToken).ConfigureAwait(false);
                rule.Result = result;
                executionContext.HandleValidationRuleResult(rule);
                results.Add(result);
            }

            return results;
        }

        /// <summary>
        /// Initialises a new instance of <see cref="SerialRuleExecutor"/>.
        /// </summary>
        /// <param name="ruleExecutor">A service which executes rules.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="ruleExecutor"/> is <see langword="null" />.</exception>
        public SerialRuleExecutor(IExeucutesSingleRule ruleExecutor)
        {
            this.ruleExecutor = ruleExecutor ?? throw new ArgumentNullException(nameof(ruleExecutor));
        }
    }
}