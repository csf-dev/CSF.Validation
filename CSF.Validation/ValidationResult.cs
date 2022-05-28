using System;
using System.Collections.Generic;
using CSF.Validation.Manifest;

namespace CSF.Validation
{
    /// <summary>
    /// A strongly-typed validation result.
    /// </summary>
    /// <typeparam name="TValidated"></typeparam>
    public class ValidationResult<TValidated> : ValidationResult, IQueryableValidationResult<TValidated>
    {
        /// <summary>
        /// Initialises a new generic instance of <see cref="ValidationResult{TValidated}"/>.
        /// </summary>
        /// <param name="ruleResults">The rule results.</param>
        /// <param name="manifest">The validation manifest.</param>
        /// <exception cref="ArgumentException">If <paramref name="manifest"/> is for a type that is not compatible with <typeparamref name="TValidated"/>.</exception>
        /// <exception cref="ArgumentNullException">If either parameter is <see langword="null" />.</exception>
        public ValidationResult(IEnumerable<ValidationRuleResult> ruleResults, ValidationManifest manifest) : base(ruleResults, manifest)
        {
            if(!manifest.ValidatedType.IsAssignableFrom(typeof(TValidated)))
            {
                var message = String.Format(Resources.ExceptionMessages.GetExceptionMessage("ValidationResultMustBeOfCorrectType"),
                                            typeof(TValidated).FullName,
                                            manifest.ValidatedType.FullName);
                throw new ArgumentException(message, nameof(manifest));
            }
        }
    }
}