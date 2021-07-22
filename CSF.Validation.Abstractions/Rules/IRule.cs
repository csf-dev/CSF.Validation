using System.Threading;
using System.Threading.Tasks;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A validation rule for an instance of a specified object type.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The generic type of this interface is contravariant.  That means that (for example) an <c>IRule&lt;Animal&gt;</c>
    /// may be used as if it were an <c>IRule&lt;Dog&gt;</c>.
    /// </para>
    /// </remarks>
    /// <typeparam name="TValidated">The type of the object which this rule validates</typeparam>
    public interface IRule<in TValidated>
    {
        /// <summary>
        /// Performs the validation logic asynchronously and returns a task of <see cref="RuleResult"/>.
        /// </summary>
        /// <param name="validated">The object being validated</param>
        /// <param name="context">Contextual information about the validation</param>
        /// <param name="token">An object which may be used to cancel the process</param>
        /// <returns>A task which provides a result object, indicating the result of validation</returns>
        Task<RuleResult> GetResultAsync(TValidated validated, RuleContext context, CancellationToken token = default);
    }
}