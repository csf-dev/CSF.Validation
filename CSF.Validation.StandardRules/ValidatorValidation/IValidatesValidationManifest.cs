using System;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Manifest;
using CSF.Validation.ManifestModel;

namespace CSF.Validation.ValidatorValidation
{
    /// <summary>
    /// An object which may be used to assert that a validation manifest is valid.
    /// Essentially this object may serve as "a validator validator".
    /// </summary>
    /// <remarks>
    /// <para>
    /// You may use the default implementation of this object within your application's integration tests
    /// in order to verify that your use of the validation framework is itself valid.  This object can detect
    /// a number of mistakes within validation manifests and hilight them at test-time rather than allowing
    /// them to become runtime errors.
    /// </para>
    /// <para>
    /// This interface has overloads of the <c>ValidateAsync</c> method to validate any of:
    /// </para>
    /// <list type="bullet">
    /// <item><description>A validation manifest</description></item>
    /// <item><description>A validator builder</description></item>
    /// <item><description>A validation manifest model</description></item>
    /// </list>
    /// </remarks>
    public interface IValidatesValidationManifest
    {
        /// <summary>
        /// Validates the validation manifest and returns a validation result.
        /// </summary>
        /// <param name="manifest">The validation manifest to be validated.</param>
        /// <param name="options">An optional collection of validation options.</param>
        /// <param name="cancellationToken">An optional cancellation token.</param>
        /// <returns>A queryable validation result.</returns>
        Task<IQueryableValidationResult<ValidationManifest>> ValidateAsync(ValidationManifest manifest,
                                                                           ValidationOptions options = default,
                                                                           CancellationToken cancellationToken = default);

        /// <summary>
        /// Validates a validator builder and returns a validation result.
        /// </summary>
        /// <typeparam name="T">The generic type validated by the builder.</typeparam>
        /// <param name="builder">The validation builder to be validated.</param>
        /// <param name="options">An optional collection of validation options.</param>
        /// <param name="cancellationToken">An optional cancellation token.</param>
        /// <returns>A queryable validation result.</returns>
        Task<IQueryableValidationResult<ValidationManifest>> ValidateAsync<T>(IBuildsValidator<T> builder,
                                                                              ValidationOptions options = default,
                                                                              CancellationToken cancellationToken = default);

        /// <summary>
        /// Validates a validation manifest model and returns a validation result.
        /// </summary>
        /// <param name="model">The validation manifest model to be validated.</param>
        /// <param name="validatedType">The type validated by the <paramref name="model"/>.</param>
        /// <param name="options">An optional collection of validation options.</param>
        /// <param name="cancellationToken">An optional cancellation token.</param>
        /// <returns>A queryable validation result.</returns>
        Task<IQueryableValidationResult<ValidationManifest>> ValidateAsync(Value model,
                                                                           Type validatedType,
                                                                           ValidationOptions options = default,
                                                                           CancellationToken cancellationToken = default);
    }
}