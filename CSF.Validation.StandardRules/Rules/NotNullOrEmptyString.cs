using System.Threading;
using System.Threading.Tasks;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A validation rule which passes if the string being validated is not <see langword="null" /> and
    /// not empty.  The rule fails if it is either.
    /// </summary>
    public class NotNullOrEmptyString : IRule<string>
    {
        /// <summary>
        /// Performs the validation logic asynchronously and returns a task of <see cref="RuleResult"/>.
        /// </summary>
        /// <param name="validated">The object being validated</param>
        /// <param name="context">Contextual information about the validation</param>
        /// <param name="token">An object which may be used to cancel the process</param>
        /// <returns>A task which provides a result object, indicating the result of validation</returns>
        public Task<RuleResult> GetResultAsync(string validated, RuleContext context, CancellationToken token = default)
            => string.IsNullOrEmpty(validated) ? FailAsync() : PassAsync();
    }
}