using System;
using System.Collections;
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
    /// This rule combines the <see cref="NotNull"/> &amp; <see cref="NotEmpty"/> rules into one.
    /// See the remarks to the <see cref="NotEmpty"/> rule for information on why it might be undesirable to use
    /// this rule; the same disadvantages apply here. It might be better to use the generic <see cref="NotNullOrEmpty{T}"/>
    /// instead.
    /// </para>
    /// <para>
    /// This rule will always return a synchronous result.
    /// </para>
    /// </remarks>
    [Parallelizable]
    public class NotNullOrEmpty : IRuleWithMessage<ICollection>, IRuleWithMessage<IEnumerable>, IRuleWithMessage<Array>, IRuleWithMessage<string>
    {
        readonly NotNull notNull;
        readonly NotEmpty notEmpty;

        /// <inheritdoc/>
        public ValueTask<RuleResult> GetResultAsync(ICollection validated, RuleContext context, CancellationToken token = default)
        {
            // Because both NotNull & NotEmpty are synchronous, it is safe to use .Result
            var notNullResult = notNull.GetResultAsync(validated, context, token).Result;
            var notEmptyResult = notEmpty.GetResultAsync(validated, context, token).Result;
            return notNullResult.IsPass && notEmptyResult.IsPass ? PassAsync() : FailAsync();
        }

        /// <inheritdoc/>
        public ValueTask<RuleResult> GetResultAsync(IEnumerable validated, RuleContext context, CancellationToken token = default)
        {
            // Because both NotNull & NotEmpty are synchronous, it is safe to use .Result
            var notNullResult = notNull.GetResultAsync(validated, context, token).Result;
            var notEmptyResult = notEmpty.GetResultAsync(validated, context, token).Result;
            return notNullResult.IsPass && notEmptyResult.IsPass ? PassAsync() : FailAsync();
        }

        /// <inheritdoc/>
        public ValueTask<RuleResult> GetResultAsync(Array validated, RuleContext context, CancellationToken token = default)
        {
            // Because both NotNull & NotEmpty are synchronous, it is safe to use .Result
            var notNullResult = notNull.GetResultAsync(validated, context, token).Result;
            var notEmptyResult = notEmpty.GetResultAsync(validated, context, token).Result;
            return notNullResult.IsPass && notEmptyResult.IsPass ? PassAsync() : FailAsync();
        }

        /// <inheritdoc/>
        public ValueTask<RuleResult> GetResultAsync(string validated, RuleContext context, CancellationToken token = default)
            => String.IsNullOrEmpty(validated) ? FailAsync() : PassAsync();

        ValueTask<string> IGetsFailureMessage<ICollection>.GetFailureMessageAsync(ICollection value, ValidationRuleResult result, CancellationToken token)
            => new ValueTask<string>(Resources.FailureMessages.GetFailureMessage("NotNullOrEmpty"));

        ValueTask<string> IGetsFailureMessage<IEnumerable>.GetFailureMessageAsync(IEnumerable value, ValidationRuleResult result, CancellationToken token)
            => new ValueTask<string>(Resources.FailureMessages.GetFailureMessage("NotNullOrEmpty"));

        ValueTask<string> IGetsFailureMessage<Array>.GetFailureMessageAsync(Array value, ValidationRuleResult result, CancellationToken token)
            => new ValueTask<string>(Resources.FailureMessages.GetFailureMessage("NotNullOrEmpty"));

        ValueTask<string> IGetsFailureMessage<string>.GetFailureMessageAsync(string value, ValidationRuleResult result, CancellationToken token)
            => new ValueTask<string>(Resources.FailureMessages.GetFailureMessage("NotNullOrEmpty"));

        /// <summary>
        /// Initialises a new instance of <see cref="NotNullOrEmpty"/>.
        /// </summary>
        /// <param name="notNull">A not-null rule.</param>
        /// <param name="notEmpty">A not-empty rule.</param>
        /// <exception cref="System.ArgumentNullException">If either constructor parameter is <see langword="null" />.</exception>
        public NotNullOrEmpty(NotNull notNull, NotEmpty notEmpty)
        {
            this.notNull = notNull ?? throw new System.ArgumentNullException(nameof(notNull));
            this.notEmpty = notEmpty ?? throw new System.ArgumentNullException(nameof(notEmpty));
        }
    }
}