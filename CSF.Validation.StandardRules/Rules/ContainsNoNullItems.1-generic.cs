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
    /// <typeparam name="T">The type of item contained within the collection.</typeparam>
    public class ContainsNoNullItems<T> : IRuleWithMessage<IEnumerable<T>>, IRuleWithMessage<IQueryable<T>>
    {
        /// <inheritdoc/>
        public Task<RuleResult> GetResultAsync(IEnumerable<T> validated, RuleContext context, CancellationToken token = default)
        {
            if(validated is null) return PassAsync();
            var countOfNulls = validated.Count(x => Equals(x, null));
            var data = new Dictionary<string, object> { { ContainsNoNullItems.CountOfNullsKey, countOfNulls } };
            return countOfNulls == 0 ? PassAsync(data) : FailAsync(data);
        }

        /// <inheritdoc/>
        public Task<RuleResult> GetResultAsync(IQueryable<T> validated, RuleContext context, CancellationToken token = default)
        {
            if(validated is null) return PassAsync();
            return validated.All(x => x != null) ? PassAsync() : FailAsync();
        }

        /// <inheritdoc/>
        public Task<string> GetFailureMessageAsync(IEnumerable<T> value, ValidationRuleResult result, CancellationToken token = default)
        {
            var countOfNulls = (int) result.Data[ContainsNoNullItems.CountOfNullsKey];
            if(countOfNulls == 1) return Task.FromResult(Resources.FailureMessages.GetFailureMessage("ContainsNoNullItemsOne"));
            return Task.FromResult(string.Format(Resources.FailureMessages.GetFailureMessage("ContainsNoNullItemsCount"), countOfNulls));
        }

        /// <inheritdoc/>
        public Task<string> GetFailureMessageAsync(IQueryable<T> value, ValidationRuleResult result, CancellationToken token = default)
            => Task.FromResult(Resources.FailureMessages.GetFailureMessage("ContainsNoNullItems"));
    }
}