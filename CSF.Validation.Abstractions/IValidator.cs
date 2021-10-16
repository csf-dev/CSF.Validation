using System;
using System.Threading;
using System.Threading.Tasks;

namespace CSF.Validation
{
    /// <summary>
    /// An object which may be used to validate object instances and get a validation result.
    /// </summary>
    public interface IValidator
    {
        /// <summary>
        /// Validate the specified object instance asynchronously and get a validation result.
        /// </summary>
        /// <param name="validatedObject">The object to be validated.</param>
        /// <param name="options">An optional object containing configuration options related to the validation process.</param>
        /// <param name="cancellationToken">An optional object which enables premature cancellation of the validation process.</param>
        /// <returns>A task containing the result of the validation process.</returns>
        /// <exception cref="InvalidCastException">
        /// If the <paramref name="validatedObject"/> is of an inappropriate type to be validated by the validator.
        /// </exception>
        /// <exception cref="ValidationException">
        /// If the validation process fails or errors and the <see cref="ValidationOptions.RuleThrowingBehaviour"/>
        /// of the <paramref name="options"/> indicate that an exception should be thrown.
        /// </exception>
        Task<ValidationResult> ValidateAsync(object validatedObject, ValidationOptions options = default, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// An object which may be used to validate instances of a specified object type and get a validation result. 
    /// </summary>
    /// <typeparam name="TValidated"></typeparam>
    public interface IValidator<TValidated>
    {
        /// <summary>
        /// Validate the specified object instance asynchronously and get a validation result.
        /// </summary>
        /// <param name="validatedObject">The object to be validated.</param>
        /// <param name="options">An optional object containing configuration options related to the validation process.</param>
        /// <param name="cancellationToken">An optional object which enables premature cancellation of the validation process.</param>
        /// <returns>A task containing the result of the validation process.</returns>
        /// <exception cref="ValidationException">
        /// If the validation process fails or errors and the <see cref="ValidationOptions.RuleThrowingBehaviour"/>
        /// of the <paramref name="options"/> indicate that an exception should be thrown.
        /// </exception>
        Task<ValidationResult> ValidateAsync(TValidated validatedObject, ValidationOptions options = default, CancellationToken cancellationToken = default);
    }
}