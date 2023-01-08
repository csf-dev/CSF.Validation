using System;
using System.Threading;
using System.Threading.Tasks;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A rule that passes if the validated value is <see langword="null" /> or if it derives from a specified type, and fails if it does not.
    /// </summary>
    public class DerivesFrom<T> : IRule<object>
    {
        /// <inheritdoc/>
        public Task<RuleResult> GetResultAsync(object validated, RuleContext context, CancellationToken token = default)
        {
            if(validated is null) return PassAsync();
            return typeof(T).IsAssignableFrom(validated.GetType()) ? PassAsync() : FailAsync();
        }
    }
}