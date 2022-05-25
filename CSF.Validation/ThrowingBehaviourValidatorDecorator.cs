using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Rules;

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

            ThrowForUnsuccessfulValidationIfApplicable(result, options);

            return result;
        }

        /// <inheritdoc/>
        public async Task<ValidationResult> ValidateAsync(object validatedObject, ValidationOptions options = null, CancellationToken cancellationToken = default)
        {
            var result = (wrapped is IValidator nonGenericValidator)
                ? await nonGenericValidator.ValidateAsync(validatedObject, options, cancellationToken).ConfigureAwait(false)
                : await ValidateAsync((TValidated)validatedObject, options, cancellationToken).ConfigureAwait(false);

            ThrowForUnsuccessfulValidationIfApplicable(result, options);

            return result;
        }

        static void ThrowForUnsuccessfulValidationIfApplicable(ValidationResult result, ValidationOptions options)
        {
            options = options ?? new ValidationOptions();
            if(ShouldThrowForError(options) && result.RuleResults.Any(x => x.Outcome == Rules.RuleOutcome.Errored))
            {
                var message = String.Format(Resources.ExceptionMessages.GetExceptionMessage("ErrorInValidation"),
                                            nameof(ValidationOptions),
                                            nameof(ValidationOptions.RuleThrowingBehaviour),
                                            nameof(RuleThrowingBehaviour),
                                            options.RuleThrowingBehaviour,
                                            nameof(ValidationResult),
                                            nameof(RuleOutcome.Errored));
                throw new ValidationException(message, result);
            }

            if(ShouldThrowForFailure(options) && result.RuleResults.Any(x => x.Outcome == Rules.RuleOutcome.Failed))
            {
                var message = String.Format(Resources.ExceptionMessages.GetExceptionMessage("FailureInValidation"),
                                            nameof(ValidationOptions),
                                            nameof(ValidationOptions.RuleThrowingBehaviour),
                                            nameof(RuleThrowingBehaviour),
                                            options.RuleThrowingBehaviour,
                                            nameof(ValidationResult),
                                            nameof(RuleOutcome.Errored),
                                            nameof(RuleOutcome.Failed));
                throw new ValidationException(message, result);
            }
        }

        static bool ShouldThrowForError(ValidationOptions options)
            => new[] { RuleThrowingBehaviour.OnError, RuleThrowingBehaviour.OnFailure }.Contains(options.RuleThrowingBehaviour);

        static bool ShouldThrowForFailure(ValidationOptions options)
            => options.RuleThrowingBehaviour == RuleThrowingBehaviour.OnFailure;

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