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
        readonly IGetsResolvedValidationOptions optionsResolver;

        /// <inheritdoc/>
        public Type ValidatedType => typeof(TValidated);
        
        /// <inheritdoc/>
        public async Task<IQueryableValidationResult<TValidated>> ValidateAsync(TValidated validatedObject, ValidationOptions options = null, CancellationToken cancellationToken = default)
        {
            var result = await validator.ValidateAsync(validatedObject, options, cancellationToken).ConfigureAwait(false);
            if(options?.EnableMessageGeneration != true) return result;

            var resolvedOptions = optionsResolver.GetResolvedValidationOptions(options);
            return await failureMessageEnricher.GetResultWithMessagesAsync(result, resolvedOptions, cancellationToken).ConfigureAwait(false);
        }

        async Task<ValidationResult> IValidator.ValidateAsync(object validatedObject, ValidationOptions options, CancellationToken cancellationToken)
        {
            var result = await ValidateAsync((TValidated) validatedObject, options, cancellationToken).ConfigureAwait(false);
            return (ValidationResult) result;
        }

        /// <summary>
        /// Initialises a new instance of <see cref="MessageEnrichingValidatorDecorator{TValidated}"/>.
        /// </summary>
        /// <param name="validator">A validator instance.</param>
        /// <param name="failureMessageEnricher">A service which adds failure messages to validation results.</param>
        /// <param name="optionsResolver">An options resolving service.</param>
        /// <exception cref="System.ArgumentNullException">If any parameter is <see langword="null" />.</exception>
        public MessageEnrichingValidatorDecorator(IValidator<TValidated> validator,
                                                  IAddsFailureMessagesToResult failureMessageEnricher,
                                                  IGetsResolvedValidationOptions optionsResolver)
{
            this.validator = validator ?? throw new System.ArgumentNullException(nameof(validator));
            this.failureMessageEnricher = failureMessageEnricher ?? throw new System.ArgumentNullException(nameof(failureMessageEnricher));
            this.optionsResolver = optionsResolver ?? throw new ArgumentNullException(nameof(optionsResolver));
        }
    }
}