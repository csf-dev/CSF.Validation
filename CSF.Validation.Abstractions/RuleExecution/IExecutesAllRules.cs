using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// An object which executes all of the validation rules using an async API and returns a collection of results.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="IRuleExecutionContext"/> provides a context/updating information which indicates which rules
    /// are ready to be executed at each given point in the process.
    /// Implementors of this interface should use <see cref="IRuleExecutionContext.GetRulesWhichMayBeExecuted"/> in
    /// order to retrieve a collection of rules which are ready for immediate execution.
    /// </para>
    /// <para>
    /// As each rule completes, regardless of its outcome, the
    /// <see cref="IRuleExecutionContext.HandleValidationRuleResult(ExecutableRule)"/> method should be executed
    /// with that rule in order to update the context, such that future usages of
    /// <see cref="IRuleExecutionContext.GetRulesWhichMayBeExecuted"/> may return additional rules which are newly-available
    /// for execution.
    /// The <see cref="IRuleExecutionContext.HandleValidationRuleResult(ExecutableRule)"/> method also ensures
    /// that alrady-executed rules will not be returned again from <see cref="IRuleExecutionContext.GetRulesWhichMayBeExecuted"/>.
    /// </para>
    /// <para>
    /// This process may be repeated more than once, getting available rules, executing them and updating them
    /// in the context.  The process should stop when all available rules have been executed (and updated in the
    /// context) and <see cref="IRuleExecutionContext.GetRulesWhichMayBeExecuted"/> returns an empty result.
    /// This is the point at which validation cannot proceed any further.  It might mean that validation is complete
    /// or it might mean that one or more rules have failed dependencies or that their validated values threw
    /// an exception when they were retrieved (if the <see cref="ValidationOptions.AccessorExceptionBehaviour"/> is
    /// set to <see cref="Manifest.ValueAccessExceptionBehaviour.TreatAsError"/>).
    /// </para>
    /// </remarks>
    public interface IExecutesAllRules
    {
        /// <summary>
        /// Execute all of the specified validation rules and return their results.
        /// </summary>
        /// <param name="executionContext">The validation rule execution context.</param>
        /// <param name="cancellationToken">An optional cancellation token.</param>
        /// <returns>A task containing a collection of the results from the executed validation rules.</returns>
        Task<IReadOnlyCollection<ValidationRuleResult>> ExecuteAllRulesAsync(IRuleExecutionContext executionContext,
                                                                             CancellationToken cancellationToken = default);
    }
}