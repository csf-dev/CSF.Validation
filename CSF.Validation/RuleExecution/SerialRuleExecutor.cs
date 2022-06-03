using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Rules;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// An implementation of <see cref="IExecutesAllRules"/> which executes each rule in serial with no parallelisation.
    /// </summary>
    public class SerialRuleExecutor : IExecutesAllRules
    {
        readonly IGetsSingleRuleExecutor ruleExecutorFactory;
        readonly ResolvedValidationOptions options;
        readonly IGetsRuleContext contextFactory;
        
        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<ValidationRuleResult>> ExecuteAllRulesAsync(IRuleExecutionContext executionContext, CancellationToken cancellationToken = default)
        {
            var ruleExecutor = ruleExecutorFactory.GetRuleExecutor(options);
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
                                                                                        IRuleExecutionContext dependencyTracker,
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
        /// <param name="ruleExecutorFactory">The factory for an object which executes rules.</param>
        /// <param name="options">The validation options.</param>
        /// <param name="contextFactory">A factory for rule contexts.</param>
        /// <exception cref="ArgumentNullException">If any parameter value is <see langword="null" />.</exception>
        public SerialRuleExecutor(IGetsSingleRuleExecutor ruleExecutorFactory,
                                  ResolvedValidationOptions options,
                                  IGetsRuleContext contextFactory)
        {
            this.ruleExecutorFactory = ruleExecutorFactory ?? throw new ArgumentNullException(nameof(ruleExecutorFactory));
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        }
    }
}