using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Messages;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A validation rule which passes if the value being validated is either <see langword="null" /> or is a collection
    /// that has at least one item.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This rule is very similar to the non-generic <see cref="NotEmpty"/> rule in that it passes if the collection itself
    /// is either <see langword="null" /> or has at least one item.
    /// This generic rule avoids some of the disadvantages of the non-generic not-empty rule.
    /// The disadvantage of this rule is that the generic type of the collection items must be specified.
    /// This can make this rule more difficult to use it with The Manifest Model, which requires all rule types to be
    /// specified as a string (where it is more difficult to consisely specify generic types).
    /// </para>
    /// <para>
    /// This rule will never enumerate the collection when operating upon any validated value which implements any of the
    /// following interfaces.
    /// </para>
    /// <list type="bullet">
    /// <item><description><see cref="ICollection{T}"/></description></item>
    /// <item><description><see cref="IReadOnlyCollection{T}"/></description></item>
    /// <item><description><see cref="IList{T}"/></description> (by virtue of implementing <see cref="ICollection{T}"/>)</item>
    /// <item><description><see cref="IReadOnlyList{T}"/> (by virtue of implementing <see cref="IReadOnlyCollection{T}"/>)</description></item>
    /// </list>
    /// <para>
    /// This rule may also be used with a Linq <see cref="IQueryable{T}"/> without treating it as simply
    /// <see cref="System.Collections.IEnumerable"/> (and thus enumerating it).
    /// When operating upon a queryable, this rule makes use of the Linq <c>Any()</c> extension method.
    /// This makes it friendly to systems such as ORMs and other implementations of queryable, without causing undesirable
    /// side-effects such as performance degradation.
    /// </para>
    /// <para>
    /// If you do not wish to (or cannot) specify the generic type of the validated value then you may instead use the non-generic
    /// <see cref="NotEmpty"/> rule.
    /// </para>
    /// <para>
    /// This rule will always return a synchronous result.
    /// </para>
    /// </remarks>
    [Parallelizable]
    public class NotEmpty<T> : IRuleWithMessage<ICollection<T>>, IRuleWithMessage<IReadOnlyCollection<T>>, IRuleWithMessage<IQueryable<T>>
    {
        /// <inheritdoc/>
        public ValueTask<RuleResult> GetResultAsync(ICollection<T> validated, RuleContext context, CancellationToken token = default)
        {
            if(validated is null) return PassAsync();
            return validated.Count > 0 ? PassAsync() : FailAsync();
        }

        /// <inheritdoc/>
        public ValueTask<RuleResult> GetResultAsync(IQueryable<T> validated, RuleContext context, CancellationToken token = default)
        {
            if(validated is null) return PassAsync();
            return validated.Any() ? PassAsync() : FailAsync();
        }

        static ValueTask<string> GetFailureMessageAsync() => new ValueTask<string>(Resources.FailureMessages.GetFailureMessage("NotEmpty"));

        ValueTask<string> IGetsFailureMessage<IReadOnlyCollection<T>>.GetFailureMessageAsync(IReadOnlyCollection<T> value, ValidationRuleResult result, CancellationToken token)
            => GetFailureMessageAsync();

        ValueTask<string> IGetsFailureMessage<ICollection<T>>.GetFailureMessageAsync(ICollection<T> value, ValidationRuleResult result, CancellationToken token)
            => GetFailureMessageAsync();

        ValueTask<string> IGetsFailureMessage<IQueryable<T>>.GetFailureMessageAsync(IQueryable<T> value, ValidationRuleResult result, CancellationToken token)
            => GetFailureMessageAsync();

        ValueTask<RuleResult> IRule<IReadOnlyCollection<T>>.GetResultAsync(IReadOnlyCollection<T> validated, RuleContext context, CancellationToken token)
        {
            if(validated is null) return PassAsync();
            return validated.Count > 0 ? PassAsync() : FailAsync();
        }
    }
}