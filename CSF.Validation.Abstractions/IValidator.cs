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
        /// Gets the type of object that this validator is intended to validate.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The first parameter to <see cref="ValidateAsync(object, ValidationOptions, CancellationToken)"/> must be of either
        /// the type indicated by this property, or a type that is derived from the type indicated by this property.
        /// </para>
        /// </remarks>
        Type ValidatedType { get; }

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
        /// If the validation process fails or errors and the <see cref="ResolvedValidationOptions.RuleThrowingBehaviour"/>
        /// of the resolved options indicate that an exception should be thrown.  See <see cref="IGetsResolvedValidationOptions"/>
        /// for more info about how the options are resolved from the defaults configured with DI and the options specified here.
        /// </exception>
        Task<ValidationResult> ValidateAsync(object validatedObject, ValidationOptions options = default, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// An object which may be used to validate instances of a specified object type and get a validation result. 
    /// </summary>
    /// <typeparam name="TValidated">The type of object which will be validated.</typeparam>
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
        /// If the validation process fails or errors and the <see cref="ResolvedValidationOptions.RuleThrowingBehaviour"/>
        /// of the resolved options indicate that an exception should be thrown.  See <see cref="IGetsResolvedValidationOptions"/>
        /// for more info about how the options are resolved from the defaults configured with DI and the options specified here.
        /// </exception>
        Task<IQueryableValidationResult<TValidated>> ValidateAsync(TValidated validatedObject, ValidationOptions options = default, CancellationToken cancellationToken = default);
    }
}