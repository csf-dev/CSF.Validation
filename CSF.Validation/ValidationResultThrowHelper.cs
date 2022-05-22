using System;
using System.Linq;
using CSF.Validation.Rules;

namespace CSF.Validation
{
    internal static class ValidationResultThrowHelper
    {
        internal static void ThrowForUnsuccessfulValidationIfApplicable(IHasValidationRuleResults result, ValidationOptions options)
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
    }
}