using System.Threading;
using System.Threading.Tasks;

namespace CSF.Validation.Messages
{
    /// <summary>
    /// Adapter which allows a <see cref="IGetsFailureMessage{TValidated}"/> to be used as a <see cref="IGetsFailureMessage"/>.
    /// </summary>
    /// <typeparam name="TValidated">The validated type.</typeparam>
    public class FailureMessageProviderAdapter<TValidated> : IGetsFailureMessage, IWrapsFailureMessageProvider
    {
        readonly IGetsFailureMessage<TValidated> wrapped;

        /// <summary>
        /// Gets the wrapped message provider instance.
        /// </summary>
        public object WrappedProvider => wrapped;

        /// <summary>
        /// Gets the validation failure message for the specified result.
        /// </summary>
        /// <param name="result">A validation result, typically indicating failure.</param>
        /// <param name="token">An optional cancellation token.</param>
        /// <returns>A human-readable message.</returns>
        public Task<string> GetFailureMessageAsync(ValidationRuleResult result, CancellationToken token = default)
            => wrapped.GetFailureMessageAsync((TValidated)result.ValidatedValue, result, token);

        /// <summary>
        /// Initialises a new instance of <see cref="FailureMessageProviderAdapter{TValidated}"/>.
        /// </summary>
        /// <param name="wrapped">The wrapped service.</param>
        /// <exception cref="System.ArgumentNullException">If <paramref name="wrapped"/> is <see langword="null" />.</exception>
        public FailureMessageProviderAdapter(IGetsFailureMessage<TValidated> wrapped)
        {
            this.wrapped = wrapped ?? throw new System.ArgumentNullException(nameof(wrapped));
        }
    }
}