using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Rules;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// A service that executes a single validation rule and gets its result.
    /// </summary>
    public class SingleRuleExecutor : IExeucutesSingleRule
    {
        readonly IGetsRuleContext contextFactory;
        
        /// <inheritdoc/>
        public async Task<ValidationRuleResult> ExecuteRuleAsync(ExecutableRule rule, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            var context = contextFactory.GetRuleContext(rule);
            var timeout = rule.RuleLogic.GetTimeout();

            using(var ruleTokenSource = timeout.HasValue ? new CancellationTokenSource(timeout.Value) : new CancellationTokenSource())
            using(var combinedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, ruleTokenSource.Token))
            {
                var ruleTask = rule.RuleLogic.GetResultAsync(rule.ValidatedValue.GetActualValue(),
                                                             rule.ValidatedValue.ParentValue?.GetActualValue(),
                                                             context,
                                                             combinedTokenSource.Token);
                var result = await WaitForResultAsync(ruleTask, combinedTokenSource.Token, timeout).ConfigureAwait(false);
                return new ValidationRuleResult(result, context, rule.RuleLogic);
            }
        }

        static async Task<RuleResult> WaitForResultAsync(Task<RuleResult> ruleTask, CancellationToken combinedToken, TimeSpan? timeout)
        {
            if (!timeout.HasValue)
                return await ruleTask.ConfigureAwait(false);

            if (await Task.WhenAny(ruleTask, Task.Delay(timeout.Value, combinedToken)).ConfigureAwait(false) == ruleTask)
                return await ruleTask.ConfigureAwait(false);

            return CommonResults.Error(data: new Dictionary<string,object> { { RuleResult.RuleTimeoutDataKey, timeout.Value } });
        }

        /// <summary>
        /// Initialises a new instance of <see cref="SingleRuleExecutor"/>.
        /// </summary>
        /// <param name="contextFactory">A factory for rule contexts.</param>
        /// <exception cref="System.ArgumentNullException">If <paramref name="contextFactory"/> is <see langword="null" />.</exception>
        public SingleRuleExecutor(IGetsRuleContext contextFactory)
        {
            this.contextFactory = contextFactory ?? throw new System.ArgumentNullException(nameof(contextFactory));
        }
    }
}
