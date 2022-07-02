using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CSF.Validation.RuleExecution
{
    internal static class RuleExecutor
    {
        internal static async Task<IEnumerable<ValidationRuleResult>> ExecuteRulesAsync(IEnumerable<ExecutableRule> availableRules,
                                                                                        IExeucutesSingleRule ruleExecutor,
                                                                                        IRuleExecutionContext executionContext,
                                                                                        CancellationToken cancellationToken)
        {
            var results = new List<ValidationRuleResult>();

            foreach(var rule in availableRules)
            {
                var result = await ExecuteRuleAsync(rule, ruleExecutor, executionContext, cancellationToken).ConfigureAwait(false);
                results.Add(result);
            }

            return results;
        }

        internal static async Task<ValidationRuleResult> ExecuteRuleAsync(ExecutableRule rule,
                                                                          IExeucutesSingleRule ruleExecutor,
                                                                          IRuleExecutionContext executionContext,
                                                                          CancellationToken cancellationToken)
        {
            var result = await ruleExecutor.ExecuteRuleAsync(rule, cancellationToken).ConfigureAwait(false);
            rule.Result = result;
            executionContext.HandleValidationRuleResult(rule);
            return result;
        }
    }
}