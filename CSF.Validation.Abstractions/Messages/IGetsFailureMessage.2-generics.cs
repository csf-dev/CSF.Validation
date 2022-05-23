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
    /// <para>
    /// This interface differs from the non-generic <see cref="IGetsFailureMessage"/> because its method
    /// <see cref="GetFailureMessageAsync(TValidated, TParent, ValidationRuleResult, CancellationToken)"/> receives
    /// two strongly typed parameters which contains the object under validation and also the parent value.
    /// </para>
    /// </remarks>
    /// <typeparam name="TValidated">The type of the validated value.</typeparam>
    /// <typeparam name="TParent">The type of the parent validated value.</typeparam>
    public interface IGetsFailureMessage<in TValidated,in TParent>
    {
        /// <summary>
        /// Gets the validation failure message for the specified result.
        /// </summary>
        /// <param name="value">The value under validation.</param>
        /// <param name="parentValue">The parent value under validation.</param>
        /// <param name="result">A validation result, typically indicating failure.</param>
        /// <param name="token">An optional cancellation token.</param>
        /// <returns>A human-readable message.</returns>
        Task<string> GetFailureMessageAsync(TValidated value, TParent parentValue, ValidationRuleResult result, CancellationToken token = default);
    }
}