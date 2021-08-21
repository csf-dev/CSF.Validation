using System.Threading;
using System.Threading.Tasks;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A generalised/non-generic interface for executing validation logic and getting the results.
    /// Implementors of this interface will generally be adapters/wrappers for interfaces such as
    /// <see cref="IRule{TValidated}"/> or <see cref="IValueRule{TValue, TValidated}"/>.
    /// </summary>
    public interface IValidationLogic
    {
        /// <summary>
        /// Executes the logic of the validation rule and returns the result.
        /// </summary>
        /// <param name="value">The value which is being validated by the current rule.</param>
        /// <param name="parentValue">
        /// An optional 'parent value' to the value being validated by the current rule.  This is
        /// typically the object from which the <paramref name="value"/> is accessed.
        /// </param>
        /// <param name="context">A validation rule context object.</param>
        /// <param name="token">An optional cancellation token to abort the validation process early.</param>
        /// <returns>A task which provides the rule result.</returns>
        Task<RuleResult> GetResultAsync(object value, object parentValue, RuleContext context, CancellationToken token = default);
    }
}