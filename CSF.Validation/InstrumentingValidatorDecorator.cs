using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace CSF.Validation
{
    /// <summary>
    /// Decorator for <see cref="IValidator{TValidated}"/> &amp; <see cref="IValidator"/> which instruments the result
    /// with timing data if applicable.
    /// </summary>
    /// <typeparam name="TValidated">The validated type.</typeparam>
    public class InstrumentingValidatorDecorator<TValidated> : IValidator<TValidated>, IValidator
    {
        readonly IValidator<TValidated> validator;
        readonly IGetsResolvedValidationOptions optionsResolver;
        readonly Stopwatch stopwatch = new Stopwatch();

        /// <inheritdoc/>
        public Type ValidatedType => typeof(TValidated);

        /// <inheritdoc/>
        public async Task<IQueryableValidationResult<TValidated>> ValidateAsync(TValidated validatedObject, ValidationOptions options = null, CancellationToken cancellationToken = default)
        {
            var resolvedOptions = optionsResolver.GetResolvedValidationOptions(options);
            if(!resolvedOptions.InstrumentRuleExecution)
                return await validator.ValidateAsync(validatedObject, options, cancellationToken).ConfigureAwait(false);

            stopwatch.Reset();
            stopwatch.Start();
            var result = await validator.ValidateAsync(validatedObject, options, cancellationToken).ConfigureAwait(false);
            stopwatch.Stop();

            return new ValidationResult<TValidated>(result.RuleResults, ((ValidationResult)result).Manifest, stopwatch.Elapsed);
        }

        async Task<ValidationResult> IValidator.ValidateAsync(object validatedObject, ValidationOptions options, CancellationToken cancellationToken)
        {
            var result = await ValidateAsync((TValidated) validatedObject, options, cancellationToken).ConfigureAwait(false);
            return (ValidationResult) result;
        }

        /// <summary>
        /// Initialises a new instance of <see cref="InstrumentingValidatorDecorator{TValidated}"/>.
        /// </summary>
        /// <param name="validator">The wrapped validator.</param>
        /// <param name="optionsResolver"></param>
        /// <exception cref="ArgumentNullException">If any parameter is <see langword="null" />.</exception>
        public InstrumentingValidatorDecorator(IValidator<TValidated> validator, IGetsResolvedValidationOptions optionsResolver)
        {
            this.validator = validator ?? throw new ArgumentNullException(nameof(validator));
            this.optionsResolver = optionsResolver ?? throw new ArgumentNullException(nameof(optionsResolver));
        }
    }
}