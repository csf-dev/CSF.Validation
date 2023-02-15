using System.Threading;
using System.Threading.Tasks;

namespace CSF.Validation.Messages
{
    /// <summary>
    /// An object which can get a copy of a single <see cref="ValidationRuleResult"/>, possibly with an added validation feedback message.
    /// </summary>
    public interface IGetsValidationRuleResultWithMessage
    {
        /// <summary>
        /// Gets rule result based upon a specified result, possibly with an added feedback message.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method might return a task with a reference to the same result that was specified as the
        /// <paramref name="ruleResult"/> parameter, or it might be a copy of that result with an added feedback
        /// message.
        /// </para>
        /// </remarks>
        /// <param name="ruleResult">The rule result for which to enrich with a message.</param>
        /// <param name="cancellationToken">An optional cancellation token.</param>
        /// <returns>A task containing a validation rule result.</returns>
        Task<ValidationRuleResult> GetRuleResultWithMessageAsync(ValidationRuleResult ruleResult, CancellationToken cancellationToken = default);
    }
}