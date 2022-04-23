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
    public class Null : IRule<object>
    {
        /// <summary>
        /// Performs the validation logic asynchronously and returns a task of <see cref="RuleResult"/>.
        /// </summary>
        /// <param name="validated">The object being validated</param>
        /// <param name="context">Contextual information about the validation</param>
        /// <param name="token">An object which may be used to cancel the process</param>
        /// <returns>A task which provides a result object, indicating the result of validation</returns>
        public Task<RuleResult> GetResultAsync(object validated, RuleContext context, CancellationToken token = default)
            => Equals(validated, null) ? PassAsync() : FailAsync();
    }
}