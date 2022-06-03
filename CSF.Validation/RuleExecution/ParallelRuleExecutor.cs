using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// An implementation of <see cref="IExecutesAllRules"/> which executes validation rules in parallel.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class uses a fairly naive algorithm to achieve parallelism.  It could be described as such:
    /// </para>
    /// <list type="number">
    /// <item><description>Get all of the available rules, via <see cref="IRuleExecutionContext.GetRulesWhichMayBeExecuted"/>.
    /// If no rules are found in this way then exit and return all of the results which have been found across all iterations of 
    /// this algorithm.</description></item>
    /// <item><description>Split those available rules into "can be run in parallel" and "cannot be run in parallel" TODO collections,
    /// using <see cref="ExecutableRule.IsEligibleToBeExecutedInParallel"/> to differentiate them.</description></item>
    /// <item><description>Add all of the rules which may be run in parallel to a collection of "in progress" tasks &amp;
    /// empty the TODO collection for rules which may be run in parallel.</description></item>
    /// <item><description>As each of the in-progress parallel rules completes remove it from the in-progress list and record its result.</description></item>
    /// <item><description>Once all in-progress list is empty, try again to find new rules which may executed in parallel, possibly refilling
    /// the "can be run in parallel" TODO collection.  If any rules eligible to be run in parallel are found in this way, return to step 3 with
    /// regard to these newly-found rules.  This is a process similar to steps 1 &amp; 2, except that a result of finding no new rules does not
    /// terminate the algorithm.  Any new non-parallelisable rules that are found are added to the existing "cannot be run in parallel" TODO
    /// collection</description></item>
    /// <item><description>Now that we have run out of available rules which may be executed in parallel, execute all rules in the
    /// "cannot be run in parallel" TODO collection in sequence &amp; record their results.  As this occurs, clear the "cannot be run in
    /// parallel" TODO collection</description></item>
    /// <item><description>Return to step 1 to attempt to find additional available rules to execute.</description></item>
    /// </list>
    /// <para>
    /// Step 5 (above) is not strictly neccesary but is included as an attempt at optimisation.  It ensures that rules which may be run in parallel
    /// are prioritised over rules which may not be run in parallel.
    /// This means that non-parallelisable rules will generally be left toward the end of each iteration, ensuring that as many rules (which were
    /// eligible to be run in parallel) as possible were run in parallel.
    /// </para>
    /// </remarks>
    public class ParallelRuleExecutor : IExecutesAllRules
    {
        readonly IExeucutesSingleRule ruleExecutor;
        readonly ResolvedValidationOptions options;

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<ValidationRuleResult>> ExecuteAllRulesAsync(IRuleExecutionContext executionContext,
                                                                                          CancellationToken cancellationToken = default)
        {
            var parallelContext = new ParallelExecutionContext(executionContext);
            var results = new List<ValidationRuleResult>();

            // See the XML 'remarks' comments on the class for a description of this algorithm.
            while(GetRulesTodo(parallelContext))
            {
                var parallelResults = await ExecuteAsManyParallelRulesAsPossibleAsync(parallelContext, cancellationToken).ConfigureAwait(false);
                results.AddRange(parallelResults);

                var nonParallelResults = await RuleExecutor.ExecuteRulesAsync(parallelContext.NonParallelTodo,
                                                                              ruleExecutor,
                                                                              parallelContext.ExecutionContext,
                                                                              cancellationToken).ConfigureAwait(false);
                results.AddRange(nonParallelResults);
                parallelContext.NonParallelTodo.Clear();
            }

            return results;
        }

        async Task<IEnumerable<ValidationRuleResult>> ExecuteAsManyParallelRulesAsPossibleAsync(ParallelExecutionContext parallelContext,
                                                                                                CancellationToken cancellationToken)
        {
            var results = new List<ValidationRuleResult>();

            do
            {
                var rulesInProgress = parallelContext.ParallelTodo
                    .Select(rule => RuleExecutor.ExecuteRuleAsync(rule, ruleExecutor, parallelContext.ExecutionContext, cancellationToken))
                    .ToList();
                parallelContext.ParallelTodo.Clear();

                while (rulesInProgress.Any())
                {
                    var resultTask = await Task.WhenAny(rulesInProgress).ConfigureAwait(false);
                    rulesInProgress.Remove(resultTask);
                    var result = await resultTask.ConfigureAwait(false);
                    results.Add(result);
                    cancellationToken.ThrowIfCancellationRequested();
                }
            } while (GetRulesTodo(parallelContext, returnTrueOnlyIfNewParallelRulesFound: true));

            return results;
        }

        static bool GetRulesTodo(ParallelExecutionContext parallelContext,
                                 bool returnTrueOnlyIfNewParallelRulesFound = false)
        {
            var newAvailableRules = parallelContext.ExecutionContext
                .GetRulesWhichMayBeExecuted()
                .Except(parallelContext.ParallelTodo)
                .Except(parallelContext.NonParallelTodo)
                .ToList();
            
            if(!newAvailableRules.Any()) return false;

            foreach (var rule in newAvailableRules.Where(x => x.IsEligibleToBeExecutedInParallel))
                parallelContext.ParallelTodo.Add(rule);
            foreach (var rule in newAvailableRules.Where(x => !x.IsEligibleToBeExecutedInParallel))
                parallelContext.NonParallelTodo.Add(rule);

            return returnTrueOnlyIfNewParallelRulesFound ? parallelContext.ParallelTodo.Any() : true;
        }

        /// <summary>
        /// Initialises a new instance of <see cref="ParallelRuleExecutor"/>.
        /// </summary>
        /// <param name="ruleExecutor">A service which executes rules.</param>
        /// <param name="options">Validation options.</param>
        /// <exception cref="ArgumentNullException">If any parameter value is <see langword="null" />.</exception>
        public ParallelRuleExecutor(IExeucutesSingleRule ruleExecutor, ResolvedValidationOptions options)
        {
            this.ruleExecutor = ruleExecutor ?? throw new System.ArgumentNullException(nameof(ruleExecutor));
            this.options = options ?? throw new ArgumentNullException(nameof(options));
        }

        class ParallelExecutionContext
        {
            public ICollection<ExecutableRule> ParallelTodo { get; } = new List<ExecutableRule>();

            public ICollection<ExecutableRule> NonParallelTodo { get; } = new List<ExecutableRule>();

            public IRuleExecutionContext ExecutionContext { get; }

            public ParallelExecutionContext(IRuleExecutionContext executionContext)
            {
                ExecutionContext = executionContext ?? throw new ArgumentNullException(nameof(executionContext));
            }
        }
    }
}