using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
    public class NotEmpty<T> : IRule<ICollection<T>>, IRule<IReadOnlyCollection<T>>, IRule<IQueryable<T>>
    {
        /// <summary>
        /// Performs the validation logic asynchronously and returns a task of <see cref="RuleResult"/>.
        /// </summary>
        /// <param name="validated">The object being validated</param>
        /// <param name="context">Contextual information about the validation</param>
        /// <param name="token">An object which may be used to cancel the process</param>
        /// <returns>A task which provides a result object, indicating the result of validation</returns>
        public Task<RuleResult> GetResultAsync(ICollection<T> validated, RuleContext context, CancellationToken token = default)
        {
            if(validated is null) return PassAsync();
            return validated.Count > 0 ? PassAsync() : FailAsync();
        }

        /// <summary>
        /// Performs the validation logic asynchronously and returns a task of <see cref="RuleResult"/>.
        /// </summary>
        /// <param name="validated">The object being validated</param>
        /// <param name="context">Contextual information about the validation</param>
        /// <param name="token">An object which may be used to cancel the process</param>
        /// <returns>A task which provides a result object, indicating the result of validation</returns>
        public Task<RuleResult> GetResultAsync(IReadOnlyCollection<T> validated, RuleContext context, CancellationToken token = default)
        {
            if(validated is null) return PassAsync();
            return validated.Count > 0 ? PassAsync() : FailAsync();
        }

        /// <summary>
        /// Performs the validation logic asynchronously and returns a task of <see cref="RuleResult"/>.
        /// </summary>
        /// <param name="validated">The object being validated</param>
        /// <param name="context">Contextual information about the validation</param>
        /// <param name="token">An object which may be used to cancel the process</param>
        /// <returns>A task which provides a result object, indicating the result of validation</returns>
        public Task<RuleResult> GetResultAsync(IQueryable<T> validated, RuleContext context, CancellationToken token = default)
        {
            if(validated is null) return PassAsync();
            return validated.Any() ? PassAsync() : FailAsync();
        }
    }
}