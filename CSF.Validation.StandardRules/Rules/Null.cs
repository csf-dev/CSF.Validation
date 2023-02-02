using System.Threading;
using System.Threading.Tasks;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A validation rule which passes if the value being validated is <see langword="null" /> and fails if it is not.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This rule uses the validated value's default equality comparer to test whether it is equal to <see langword="null" />
    /// or not.  If the validated value is of a type which has an overridden <see cref="System.Object.Equals(object)"/> method,
    /// that method will be used.
    /// </para>
    /// <para>
    /// This rule will always return a synchronous result.
    /// </para>
    /// </remarks>
    [Parallelizable]
    public class Null : IRuleWithMessage<object>
    {
        /// <inheritdoc/>
        public Task<RuleResult> GetResultAsync(object validated, RuleContext context, CancellationToken token = default)
            => Equals(validated, null) ? PassAsync() : FailAsync();

        /// <inheritdoc/>
        public Task<string> GetFailureMessageAsync(object value, ValidationRuleResult result, CancellationToken token = default)
             => Task.FromResult(Resources.FailureMessages.GetFailureMessage("Null"));
    }
}