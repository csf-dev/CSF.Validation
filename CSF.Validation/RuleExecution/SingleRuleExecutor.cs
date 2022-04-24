using System.Collections.Generic;
using System.Linq;
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
        /// <summary>
        /// Execute the validation rule and return its result.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method will also throw if the <paramref name="cancellationToken"/> requests cancellation.
        /// </para>
        /// </remarks>
        /// <param name="rule">The executable rule.</param>
        /// <param name="cancellationToken">An optional cancellation token.</param>
        /// <returns>A task containing the result of running the single rule.</returns>
        public async Task<ValidationRuleResult> ExecuteRuleAsync(ExecutableRule rule, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var result = await rule.RuleLogic.GetResultAsync(rule.ValidatedValue.ActualValue,
                                                             rule.ValidatedValue.ParentValue?.ActualValue,
                                                             GetRuleContext(rule),
                                                             cancellationToken)
                .ConfigureAwait(false);
            
            return ValidationRuleResult.FromRuleResult(result, rule.RuleIdentifier);
        }

        static RuleContext GetRuleContext(ExecutableRule rule)
            => new RuleContext(rule.ManifestRule, rule.RuleIdentifier, rule.ValidatedValue.ActualValue, GetAncestorContexts(rule).ToList(), rule.ValidatedValue.CollectionItemOrder);

        static IEnumerable<ValueContext> GetAncestorContexts(ExecutableRule rule)
        {
            var value = rule.ValidatedValue;

            while(!(value.ParentValue is null))
            {
                var current = value.ParentValue;
                yield return new ValueContext(current.ValueIdentity, current.ActualValue, current.ManifestValue, current.CollectionItemOrder);
                value = current;
            }
        }
    }
}
