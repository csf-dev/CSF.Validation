using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Rules;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// A decorator for <see cref="IExecutesAllRules"/> which adds additional results to the output for
    /// any validated values which raised an error when they were retrieved.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This decorator is relevant when the <see cref="ValidatedValue.ValueResponse"/> for any of the
    /// <see cref="ExecutableRule.ValidatedValue"/> of any of the available
    /// rules is an instance of <see cref="ErrorGetValueToBeValidatedResponse"/>.
    /// These value responses represent values which were not successfully retrieved &amp; threw an exception.
    /// </para>
    /// <para>
    /// This class adds additional result items to the overall list of results, to indicate that these validated
    /// values caused errors.
    /// </para>
    /// <para>
    /// This would only occur if the <see cref="ValidationOptions.AccessorExceptionBehaviour"/> is set to
    /// <see cref="Manifest.ValueAccessExceptionBehaviour.TreatAsError"/>.
    /// </para>
    /// </remarks>
    public class ResultsForErroredValuesExecutionDecorator : IExecutesAllRules
    {
        readonly IExecutesAllRules wrapped;
        
        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<ValidationRuleResult>> ExecuteAllRulesAsync(IRuleExecutionContext executionContext, CancellationToken cancellationToken = default)
        {
            var results = (await wrapped.ExecuteAllRulesAsync(executionContext, cancellationToken).ConfigureAwait(false)).ToList();
            cancellationToken.ThrowIfCancellationRequested();

            var extraResultsForFailedValues = executionContext.AllRules
                .Select(x => x.ExecutableRule.ValidatedValue.ValueResponse)
                .OfType<ErrorGetValueToBeValidatedResponse>()
                .Distinct()
                .Select(x => new ValidationRuleResult(new RuleResult(RuleOutcome.Errored, exception: x.Exception), null, null))
                .ToList();
            results.AddRange(extraResultsForFailedValues);

            return results;
        }

        /// <summary>
        /// Initialises a new instance of <see cref="ResultsForErroredValuesExecutionDecorator"/>.
        /// </summary>
        /// <param name="wrapped">The wrapped rule executor.</param>
        /// <exception cref="System.ArgumentNullException">If <paramref name="wrapped"/> is <see langword="null" />.</exception>
        public ResultsForErroredValuesExecutionDecorator(IExecutesAllRules wrapped)
        {
            this.wrapped = wrapped ?? throw new System.ArgumentNullException(nameof(wrapped));
        }
    }
}