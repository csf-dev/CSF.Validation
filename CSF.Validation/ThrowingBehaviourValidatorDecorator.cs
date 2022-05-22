using System;
using System.Threading;
using System.Threading.Tasks;

namespace CSF.Validation
{
    /// <summary>
    /// A decorator for <see cref="IValidator{TValidated}"/> which throws an exception if the combination of
    /// validation result and <see cref="ValidationOptions.RuleThrowingBehaviour"/> indicate that an exception
    /// should be thrown.
    /// </summary>
    /// <typeparam name="TValidated"></typeparam>
    public class ThrowingBehaviourValidatorDecorator<TValidated> : IValidator<TValidated>, IValidator
    {
        readonly IValidator<TValidated> wrapped;

        /// <inheritdoc/>
        public Type ValidatedType => (wrapped as IValidator)?.ValidatedType ?? typeof(TValidated);

        /// <inheritdoc/>
        public async Task<ValidationResult> ValidateAsync(TValidated validatedObject, ValidationOptions options = null, CancellationToken cancellationToken = default)
        {
            var result = await wrapped.ValidateAsync(validatedObject, options, cancellationToken).ConfigureAwait(false);

            ValidationResultThrowHelper.ThrowForUnsuccessfulValidationIfApplicable(result, options);

            return result;
        }

        /// <inheritdoc/>
        public async Task<ValidationResult> ValidateAsync(object validatedObject, ValidationOptions options = null, CancellationToken cancellationToken = default)
        {
            var result = (wrapped is IValidator nonGenericValidator)
                ? await nonGenericValidator.ValidateAsync(validatedObject, options, cancellationToken).ConfigureAwait(false)
                : await ValidateAsync((TValidated)validatedObject, options, cancellationToken).ConfigureAwait(false);

            ValidationResultThrowHelper.ThrowForUnsuccessfulValidationIfApplicable(result, options);

            return result;
        }

        /// <summary>
        /// Initialises a new instance of <see cref="ThrowingBehaviourValidatorDecorator{TValidated}"/>.
        /// </summary>
        /// <param name="wrapped">The wrapped validator.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="wrapped"/> is <see langword="null" />.</exception>
        public ThrowingBehaviourValidatorDecorator(IValidator<TValidated> wrapped)
        {
            this.wrapped = wrapped ?? throw new ArgumentNullException(nameof(wrapped));
        }
    }
}