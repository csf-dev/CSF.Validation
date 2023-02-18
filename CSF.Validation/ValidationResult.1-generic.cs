using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CSF.Validation.Manifest;

namespace CSF.Validation
{
    /// <summary>
    /// A strongly-typed validation result.
    /// </summary>
    /// <typeparam name="TValidated"></typeparam>
    public class ValidationResult<TValidated> : ValidationResult, IQueryableValidationResult<TValidated>
    {
        /// <inheritdoc/>
        public IQueryableValidationResult<TItem> ForMember<TItem>(Expression<Func<TValidated, TItem>> memberExpression)
            => ResultQueries.ForMember(this, memberExpression);

        /// <inheritdoc/>
        public IQueryableValidationResult<TItem> ForMatchingMemberItem<TItem>(Expression<Func<TValidated, IEnumerable<TItem>>> memberExpression, TItem item)
            => ResultQueries.ForMatchingMemberItem(this, memberExpression, item);

        /// <inheritdoc/>
        public IQueryableValidationResult<TValidated> ForOnlyThisValue() => ResultQueries.ForOnlyTheSameValue(this);

        /// <inheritdoc/>
        public IQueryableValidationResult<TValidated> WithoutSuccesses() => ResultQueries.WithoutSuccesses(this);

        /// <inheritdoc/>
        public override SerializableValidationResult ToSerializableResult() => ResultQueries.ToSerializableValidationResult(this);

        /// <inheritdoc/>
        public IQueryableValidationResult<TDerived> PolymorphicAs<TDerived>() where TDerived : TValidated
            => ResultQueries.PolymorphicAs<TValidated, TDerived>(this);

        /// <summary>
        /// Initialises a new generic instance of <see cref="ValidationResult{TValidated}"/>.
        /// </summary>
        /// <param name="ruleResults">The rule results.</param>
        /// <param name="manifest">The validation manifest.</param>
        /// <param name="validationTime">The time it has taken to perform validation.</param>
        /// <exception cref="ArgumentException">If <paramref name="manifest"/> is for a type that is not compatible with <typeparamref name="TValidated"/>.</exception>
        /// <exception cref="ArgumentNullException">If either parameter is <see langword="null" />.</exception>
        public ValidationResult(IEnumerable<ValidationRuleResult> ruleResults,
                                ValidationManifest manifest,
                                TimeSpan? validationTime = default) : base(ruleResults, manifest, validationTime)
        {
            if(!typeof(TValidated).IsAssignableFrom(manifest.ValidatedType))
            {
                var message = String.Format(Resources.ExceptionMessages.GetExceptionMessage("ValidationResultMustBeOfCorrectType"),
                                            typeof(TValidated).FullName,
                                            manifest.ValidatedType.FullName);
                throw new ArgumentException(message, nameof(manifest));
            }
        }
    }
}