using System.Threading;
using System.Threading.Tasks;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// An object which can execute a single validation rule and get its result.
    /// </summary>
    public interface IExeucutesSingleRule
    {
        /// <summary>
        /// Executes the validation rule and returns its result.
        /// </summary>
        /// <param name="rule">The rule to execute.</param>
        /// <param name="cancellationToken">An optional cancellation token.</param>
        /// <returns>A task containing the rule's result.</returns>
        Task<ValidationRuleResult> ExecuteRuleAsync(ExecutableRule rule, CancellationToken cancellationToken = default);
    }
}