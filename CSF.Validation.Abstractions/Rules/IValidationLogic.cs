using System;
using System.Threading;
using System.Threading.Tasks;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A generalised/non-generic interface for executing validation logic and getting the results.
    /// It is not expected that developers would want to implement this interface for their own rules.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Instead of implementing this interface in order to create custom rules, developers should instead
    /// implement one of <see cref="IRule{TValidated}"/> or <see cref="IRule{TValue, TValidated}"/> in their
    /// own logic.
    /// </para>
    /// <para>
    /// The validation framework comes with implementations of this interface which serve as adapters/wrappers
    /// for those two interfaces noted above, allowing this interface to be used as a generalised mechanism for
    /// executing rule logic, regardless of the precise interface to which that logic were originally written.
    /// </para>
    /// </remarks>
    public interface IValidationLogic
    {
        /// <summary>
        /// Gets the type of rule interface that is used by this rule logic.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This will be a closed-generic form of either <see cref="IRule{TValidated}"/> or
        /// <see cref="IRule{TValue, TParent}"/>.
        /// </para>
        /// </remarks>
        Type RuleInterface { get; }

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