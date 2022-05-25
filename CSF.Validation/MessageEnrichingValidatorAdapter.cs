using System;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Messages;

namespace CSF.Validation
{
    /// <summary>
    /// A decorator service which wraps a <see cref="IValidator{TValidated}"/> and extends it to be usable as either
    /// a <see cref="IValidator{TValidated}"/> or <see cref="IValidator"/>.
    /// </summary>
    /// <typeparam name="TValidated">The type of the object which is validated.</typeparam>
    public class MessageEnrichingValidatorDecorator<TValidated> : IValidator<TValidated>, IValidator
    {
        readonly IValidator<TValidated> validator;
        readonly IAddsFailureMessagesToResult failureMessageEnricher;

        /// <summary>
        /// Gets the type of object that this validator is intended to validate.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The first parameter to <see cref="IValidator.ValidateAsync(object, ValidationOptions, CancellationToken)"/> must be of either
        /// the type indicated by this property, or a type that is derived from the type indicated by this property.
        /// </para>
        /// </remarks>
        public Type ValidatedType => typeof(TValidated);

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
        public async Task<ValidationResult> ValidateAsync(TValidated validatedObject, ValidationOptions options = null, CancellationToken cancellationToken = default)
        {
            var result = await validator.ValidateAsync(validatedObject, options, cancellationToken).ConfigureAwait(false);
            return await failureMessageEnricher.GetResultWithMessagesAsync(result, cancellationToken).ConfigureAwait(false);
        }

        Task<ValidationResult> IValidator.ValidateAsync(object validatedObject, ValidationOptions options, CancellationToken cancellationToken)
            => ValidateAsync((TValidated)validatedObject, options, cancellationToken);

        /// <summary>
        /// Initialises a new instance of <see cref="MessageEnrichingValidatorDecorator{TValidated}"/>.
        /// </summary>
        /// <param name="validator">A validator instance.</param>
        /// <param name="failureMessageEnricher">A service which adds failure messages to validation results.</param>
        /// <exception cref="System.ArgumentNullException">If any parameter is <see langword="null" />.</exception>
        public MessageEnrichingValidatorDecorator(IValidator<TValidated> validator, IAddsFailureMessagesToResult failureMessageEnricher)
        {
            this.validator = validator ?? throw new System.ArgumentNullException(nameof(validator));
            this.failureMessageEnricher = failureMessageEnricher ?? throw new System.ArgumentNullException(nameof(failureMessageEnricher));
        }
    }
}