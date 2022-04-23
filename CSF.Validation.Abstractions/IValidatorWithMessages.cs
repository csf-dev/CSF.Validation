using System;
using System.Threading;
using System.Threading.Tasks;

namespace CSF.Validation
{
    /// <summary>
    /// An object which may be used to validate object instances and get a validation result.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This interface may be used similarly to <see cref="IValidator"/>.  The difference is that when validation rules fail,
    /// each failed rule will be processed for a failure message.
    /// </para>
    /// </remarks>
    public interface IValidatorWithMessages
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
        /// If the validation process fails or errors and the <see cref="ValidationOptions.RuleThrowingBehaviour"/>
        /// of the <paramref name="options"/> indicate that an exception should be thrown.
        /// </exception>
        Task<ValidationResultWithMessages> ValidateAsync(object validatedObject, ValidationOptions options = default, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// An object which may be used to validate instances of a specified object type and get a validation result. 
    /// </summary>
    /// <remarks>
    /// <para>
    /// This interface may be used similarly to <see cref="IValidator{TValidated}"/>.  The difference is that when validation rules fail,
    /// each failed rule will be processed for a failure message.
    /// </para>
    /// </remarks>
    /// <typeparam name="TValidated">The type of object which will be validated.</typeparam>
    public interface IValidatorWithMessages<TValidated>
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
        Task<ValidationResultWithMessages> ValidateAsync(TValidated validatedObject, ValidationOptions options = default, CancellationToken cancellationToken = default);
    }
}