using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Messages;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A validation rule which passes if the object being validated is both not <see langword="null" /> and
    /// not an empty collection.  The rule fails if it is either.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Internally, this rule combines the <see cref="NotNull"/> &amp; <see cref="NotEmpty{T}"/> rules into one.
    /// See the remarks to the <see cref="NotEmpty{T}"/> rule for information on why this rule is often the superior
    /// choice when working with <see cref="IQueryable{T}"/>, for example.
    /// </para>
    /// <para>
    /// This rule will always return a synchronous result.
    /// </para>
    /// </remarks>
    [Parallelizable]
    public class NotNullOrEmpty<T> : IRuleWithMessage<ICollection<T>>, IRuleWithMessage<IReadOnlyCollection<T>>, IRuleWithMessage<IQueryable<T>>
    {
        readonly NotNull notNull;
        readonly NotEmpty<T> notEmpty;

        /// <inheritdoc/>
        public async ValueTask<RuleResult> GetResultAsync(ICollection<T> validated, RuleContext context, CancellationToken token = default)
        {
            var notNullResult = await notNull.GetResultAsync(validated, context, token).ConfigureAwait(false);
            var notEmptyResult = await notEmpty.GetResultAsync(validated, context, token).ConfigureAwait(false);
            return notNullResult.IsPass && notEmptyResult.IsPass ? Pass() : Fail();
        }

        /// <inheritdoc/>
        public async ValueTask<RuleResult> GetResultAsync(IReadOnlyCollection<T> validated, RuleContext context, CancellationToken token = default)
        {
            var notNullResult = await notNull.GetResultAsync(validated, context, token).ConfigureAwait(false);
            var notEmptyResult = await ((IRule<IReadOnlyCollection<T>>) notEmpty).GetResultAsync(validated, context, token).ConfigureAwait(false);
            return notNullResult.IsPass && notEmptyResult.IsPass ? Pass() : Fail();
        }

        /// <inheritdoc/>
        public async ValueTask<RuleResult> GetResultAsync(IQueryable<T> validated, RuleContext context, CancellationToken token = default)
        {
            var notNullResult = await notNull.GetResultAsync(validated, context, token).ConfigureAwait(false);
            var notEmptyResult = await notEmpty.GetResultAsync(validated, context, token).ConfigureAwait(false);
            return notNullResult.IsPass && notEmptyResult.IsPass ? Pass() : Fail();
        }

        ValueTask<string> IGetsFailureMessage<ICollection<T>>.GetFailureMessageAsync(ICollection<T> value, ValidationRuleResult result, CancellationToken token)
            => new ValueTask<string>(Resources.FailureMessages.GetFailureMessage("NotNullOrEmpty"));

        ValueTask<string> IGetsFailureMessage<IReadOnlyCollection<T>>.GetFailureMessageAsync(IReadOnlyCollection<T> value, ValidationRuleResult result, CancellationToken token)
            => new ValueTask<string>(Resources.FailureMessages.GetFailureMessage("NotNullOrEmpty"));

        ValueTask<string> IGetsFailureMessage<IQueryable<T>>.GetFailureMessageAsync(IQueryable<T> value, ValidationRuleResult result, CancellationToken token)
            => new ValueTask<string>(Resources.FailureMessages.GetFailureMessage("NotNullOrEmpty"));

        /// <summary>
        /// Initialises a new instance of <see cref="NotNullOrEmpty{T}"/>.
        /// </summary>
        /// <param name="notNull">A not-null rule.</param>
        /// <param name="notEmpty">A not-empty rule.</param>
        /// <exception cref="System.ArgumentNullException">If either constructor parameter is <see langword="null" />.</exception>
        public NotNullOrEmpty(NotNull notNull, NotEmpty<T> notEmpty)
        {
            this.notNull = notNull ?? throw new System.ArgumentNullException(nameof(notNull));
            this.notEmpty = notEmpty ?? throw new System.ArgumentNullException(nameof(notEmpty));
        }
    }
}