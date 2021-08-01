using System;
using System.Threading;
using System.Threading.Tasks;

namespace CSF.Validation
{
    /// <summary>
    /// The validator service, which validates instances of <typeparamref name="TValidated"/>
    /// and returns a result.
    /// </summary>
    /// <typeparam name="TValidated">The type of the object which is validated.</typeparam>
    public class Validator<TValidated> : IValidator<TValidated>, IValidator
    {
        /// <summary>
        /// Validate the specified object instance asynchronously and get a validation result.
        /// </summary>
        /// <param name="validatedObject">The object to be validated.</param>
        /// <param name="options">An optional object containing configuration options related to the validation process.</param>
        /// <param name="cancellationToken">An optional object which enables premature cancellation of the validation process.</param>
        /// <returns>A task containing the result of the validation process.</returns>
        /// <exception cref="ValidationException">
        /// If the validation process fails or errors and the <see cref="ValidationOptions.ThrowingBehaviour"/>
        /// of the <paramref name="options"/> indicate that an exception should be thrown.
        /// </exception>
        public Task<ValidationResult> ValidateAsync(TValidated validatedObject, ValidationOptions options = null, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        Task<ValidationResult> IValidator.ValidateAsync(object validatedObject, ValidationOptions options, CancellationToken cancellationToken)
            => ValidateAsync((TValidated)validatedObject, options, cancellationToken);
    }
}