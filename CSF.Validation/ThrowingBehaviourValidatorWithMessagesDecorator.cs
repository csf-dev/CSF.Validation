using System;
using System.Threading;
using System.Threading.Tasks;

namespace CSF.Validation
{
    /// <summary>
    /// A decorator for <see cref="IValidatorWithMessages{TValidated}"/> which throws an exception if the combination of
    /// validation result and <see cref="ValidationOptions.RuleThrowingBehaviour"/> indicate that an exception
    /// should be thrown.
    /// </summary>
    /// <typeparam name="TValidated"></typeparam>
    public class ThrowingBehaviourValidatorWithMessagesDecorator<TValidated> : IValidatorWithMessages<TValidated>, IValidatorWithMessages
    {
        readonly IValidatorWithMessages<TValidated> wrapped;

        /// <inheritdoc/>
        public Type ValidatedType => (wrapped as IValidator)?.ValidatedType ?? typeof(TValidated);

        /// <inheritdoc/>
        public async Task<ValidationResultWithMessages> ValidateAsync(TValidated validatedObject, ValidationOptions options = null, CancellationToken cancellationToken = default)
        {
            var result = await wrapped.ValidateAsync(validatedObject, options, cancellationToken).ConfigureAwait(false);

            ValidationResultThrowHelper.ThrowForUnsuccessfulValidationIfApplicable(result, options);

            return result;
        }

        /// <inheritdoc/>
        public async Task<ValidationResultWithMessages> ValidateAsync(object validatedObject, ValidationOptions options = null, CancellationToken cancellationToken = default)
        {
            var result = (wrapped is IValidatorWithMessages nonGenericValidator)
                ? await nonGenericValidator.ValidateAsync(validatedObject, options, cancellationToken).ConfigureAwait(false)
                : await ValidateAsync((TValidated)validatedObject, options, cancellationToken).ConfigureAwait(false);

            ValidationResultThrowHelper.ThrowForUnsuccessfulValidationIfApplicable(result, options);

            return result;
        }

        /// <summary>
        /// Initialises a new instance of <see cref="ThrowingBehaviourValidatorWithMessagesDecorator{TValidated}"/>.
        /// </summary>
        /// <param name="wrapped">The wrapped validator.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="wrapped"/> is <see langword="null" />.</exception>
        public ThrowingBehaviourValidatorWithMessagesDecorator(IValidatorWithMessages<TValidated> wrapped)
        {
            this.wrapped = wrapped ?? throw new ArgumentNullException(nameof(wrapped));
        }
    }
}