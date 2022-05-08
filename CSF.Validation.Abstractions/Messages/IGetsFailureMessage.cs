using System.Threading;
using System.Threading.Tasks;

namespace CSF.Validation.Messages
{
    /// <summary>
    /// An object which can get a human-readable message relating to the failure of a single validation rule.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Such a message is intended to be suitable for display in a user interface, to an appropriate end-user.
    /// A good message tells the user what is invalid and indicates what steps should be taken to fix it.
    /// </para>
    /// </remarks>
    public interface IGetsFailureMessage
    {
        /// <summary>
        /// Gets the validation failure message for the specified result.
        /// </summary>
        /// <param name="result">A validation result, typically indicating failure.</param>
        /// <param name="token">An optional cancellation token.</param>
        /// <returns>A human-readable message.</returns>
        Task<string> GetFailureMessageAsync(ValidationRuleResult result, CancellationToken token = default);
    }
}