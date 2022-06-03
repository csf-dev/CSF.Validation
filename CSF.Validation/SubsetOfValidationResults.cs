using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CSF.Validation.Manifest;
using CSF.Validation.Rules;

namespace CSF.Validation
{
    /// <summary>
    /// An implementation of <see cref="IQueryableValidationResult{TValidated}"/> which represents a
    /// subset of a larger validation result.  This is usually due to some form of filtering of the original result.
    /// </summary>
    /// <typeparam name="TValidated"></typeparam>
    public class SubsetOfValidationResults<TValidated> : IQueryableValidationResult<TValidated>
    {
        /// <inheritdoc/>
        public bool Passed  { get; }

        /// <inheritdoc/>
        public ManifestValueBase ManifestValue  { get; }

        /// <inheritdoc/>
        public IReadOnlyCollection<ValidationRuleResult> RuleResults { get; }

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
        public SerializableValidationResult ToSerializableResult() => ResultQueries.ToSerializableValidationResult(this);

        IQueryableValidationResult<T> IQueryableValidationResult.AsResultFor<T>() => (IQueryableValidationResult<T>)this;

        IEnumerator<ValidationRuleResult> IEnumerable<ValidationRuleResult>.GetEnumerator() => RuleResults.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => RuleResults.GetEnumerator();

        /// <summary>
        /// Initialises a new instance of <see cref="SubsetOfValidationResults{TValidated}"/>.
        /// </summary>
        /// <param name="ruleResults">The rule results.</param>
        /// <param name="manifestValue">The manifest value.</param>
        /// <exception cref="ArgumentNullException">If any parameter is <see langword="null" />.</exception>
        public SubsetOfValidationResults(IEnumerable<ValidationRuleResult> ruleResults, ManifestValueBase manifestValue)
        {
            RuleResults = ruleResults?.ToList() ?? throw new ArgumentNullException(nameof(ruleResults));
            ManifestValue = manifestValue ?? throw new ArgumentNullException(nameof(manifestValue));
            Passed = RuleResults.All(r => r.Outcome == RuleOutcome.Passed);
        }
    }
}