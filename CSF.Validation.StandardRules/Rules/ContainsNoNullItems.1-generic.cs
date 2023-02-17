using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A rule which asserts that a collection or a queryable value contains no items that are <see langword="null" />.
    /// </summary>
    /// <remarks>
    /// <para>
    /// When validating an <see cref="IEnumerable{T}"/>, this rule will result in enumerating the enumerable once.
    /// However, this rule has an overload specifically for validating an <see cref="IQueryable{T}"/> which does not
    /// enumerate the query.  This makes use of the Linq
    /// <see cref="Queryable.All{TSource}(IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, bool}})"/>
    /// extension method, which gives the query provider an opportunity to avoid a full enumeration.
    /// </para>
    /// <para>
    /// Using this rule to get a failure message does not result in eumerating the collection a second time.
    /// The number of null items is recorded and stored in <see cref="RuleResult.Data"/>, this is then reused
    /// when creating the message, if available.
    /// </para>
    /// </remarks>
    /// <typeparam name="T">The type of item contained within the collection.</typeparam>
    [Parallelizable]
    public class ContainsNoNullItems<T> : IRuleWithMessage<IEnumerable<T>>, IRuleWithMessage<IQueryable<T>>
    {
        /// <inheritdoc/>
        public ValueTask<RuleResult> GetResultAsync(IEnumerable<T> validated, RuleContext context, CancellationToken token = default)
        {
            if(validated is null) return PassAsync();
            var countOfNulls = validated.Count(x => Equals(x, null));
            var data = new Dictionary<string, object> { { ContainsNoNullItems.CountOfNullsKey, countOfNulls } };
            return countOfNulls == 0 ? PassAsync(data) : FailAsync(data);
        }

        /// <inheritdoc/>
        public ValueTask<RuleResult> GetResultAsync(IQueryable<T> validated, RuleContext context, CancellationToken token = default)
        {
            if(validated is null) return PassAsync();
            return validated.All(x => x != null) ? PassAsync() : FailAsync();
        }

        /// <inheritdoc/>
        public ValueTask<string> GetFailureMessageAsync(IQueryable<T> value, ValidationRuleResult result, CancellationToken token = default)
            => new ValueTask<string>(ContainsNoNullItems.GetFailureMessage(value, result));

        /// <inheritdoc/>
        public ValueTask<string> GetFailureMessageAsync(IEnumerable<T> value, ValidationRuleResult result, CancellationToken token = default)
            => new ValueTask<string>(ContainsNoNullItems.GetFailureMessage(value, result));
    }
}