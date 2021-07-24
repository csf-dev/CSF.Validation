using System.Threading;
using System.Threading.Tasks;
using CSF.Validation;

namespace CSF.ValidationMessages
{
    /// <summary>
    /// A service object which adds huaman-readable feedback messages (as appropriate) to
    /// a <see cref="ValidationResult"/>.
    /// </summary>
    public interface IGetsResultWithMessages
    {
        /// <summary>
        /// Gets a copy of the <paramref name="result"/> but with human-readable feedback feedback messages
        /// added to the individual rule-results where applicable.
        /// </summary>
        /// <param name="result">The validation result.</param>
        /// <param name="cancellationToken">An optional token which may be used to cancel the process prematurely.</param>
        /// <returns>A task which exposes a copy of the validation result, but with additional feedback messages.</returns>
        Task<ValidationResultWithMessages> GetResultWithMessagesAsync(ValidationResult result, CancellationToken cancellationToken = default);
    }
}