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
        {
            var countOfNulls = (int) result.Data[CountOfNullsKey];
            if(countOfNulls == 1) return Task.FromResult(Resources.FailureMessages.GetFailureMessage("ContainsNoNullItemsOne"));
            return Task.FromResult(string.Format(Resources.FailureMessages.GetFailureMessage("ContainsNoNullItemsCount"), countOfNulls));
        }
    }
}