using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A rule which asserts that a collection contains no items that are <see langword="null" />.
    /// </summary>
    public class ContainsNoNullItems : IRuleWithMessage<IEnumerable>
    {
        internal const string CountOfNullsKey = "Count of nulls";

        /// <inheritdoc/>
        public Task<RuleResult> GetResultAsync(IEnumerable validated, RuleContext context, CancellationToken token = default)
        {
            if(validated is null) return PassAsync();
            var countOfNulls = validated.Cast<object>().Count(x => Equals(x, null));
            var data = new Dictionary<string, object> { { CountOfNullsKey, countOfNulls } };
            return countOfNulls == 0 ? PassAsync(data) : FailAsync(data);
        }

        /// <inheritdoc/>
        public Task<string> GetFailureMessageAsync(IEnumerable value, ValidationRuleResult result, CancellationToken token = default)
            => Task.FromResult(GetFailureMessage(value, result));

        internal static string GetFailureMessage(IEnumerable value, ValidationRuleResult result)
        {
            if(!result.Data.TryGetValue(CountOfNullsKey, out var count))
                return Resources.FailureMessages.GetFailureMessage("ContainsNoNullItems");

            var countOfNulls = (int) count;
            return (countOfNulls == 1)
                ? Resources.FailureMessages.GetFailureMessage("ContainsNoNullItemsOne")
                : String.Format(Resources.FailureMessages.GetFailureMessage("ContainsNoNullItemsCount"), countOfNulls);
        }
    }
}