using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A rule that passes if the validated value is <see langword="null" /> or if it derives from a specified type, and fails if it does not.
    /// </summary>
    [Parallelizable]
    public class DerivesFrom<T> : IRuleWithMessage<object>
    {
        /// <inheritdoc/>
        public Task<RuleResult> GetResultAsync(object validated, RuleContext context, CancellationToken token = default)
        {
            if(validated is null) return PassAsync();
            var actualType = validated.GetType();
            var data = new Dictionary<string, object> { { DerivesFrom.ActualTypeKey, actualType } };
            return typeof(T).IsAssignableFrom(actualType) ? PassAsync(data) : FailAsync(data);
        }

        /// <inheritdoc/>
        public Task<string> GetFailureMessageAsync(object value, ValidationRuleResult result, CancellationToken token = default)
            => Task.FromResult(DerivesFrom.GetFailureMessage(value, result, typeof(T)));
    }
}