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
        readonly IGetsRuleContext contextFactory;

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

            var context = contextFactory.GetRuleContext(rule);
            var result = await rule.RuleLogic.GetResultAsync(rule.ValidatedValue.ActualValue,
                                                             rule.ValidatedValue.ParentValue?.ActualValue,
                                                             context,
                                                             cancellationToken)
                .ConfigureAwait(false);

            return new ValidationRuleResult(result, context, rule.RuleLogic);
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
