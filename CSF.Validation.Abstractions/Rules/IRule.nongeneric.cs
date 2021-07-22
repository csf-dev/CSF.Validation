using System.Threading;
using System.Threading.Tasks;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A validation rule which may validate any object.
    /// Generally-speaking custom validation rules should not implement this interface, preferring either
    /// <see cref="IRule{TValidated}"/> or <see cref="IValueRule{TValue, TValidated}"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This interface is primarily used internally by the framework when it needs to hold collections of
    /// rules using only a single interface.
    /// </para>
    /// </remarks>
    public interface IRule
    {
        /// <summary>
        /// Performs the validation logic asynchronously and returns a task of <see cref="RuleResult"/>.
        /// </summary>
        /// <param name="validated">The object being validated</param>
        /// <param name="context">Contextual information about the validation</param>
        /// <param name="token">An object which may be used to cancel the process</param>
        /// <returns>A task which provides a result object, indicating the result of validation</returns>
        Task<RuleResult> GetResultAsync(object validated, RuleContext context, CancellationToken token = default);
    }
}