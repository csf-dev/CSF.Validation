using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// Decorator for <see cref="IExeucutesSingleRule"/> which adds instrumentation data to the rule result if applicable.
    /// </summary>
    public class InstrumentingSingleRuleExecutorDecorator : IExeucutesSingleRule
    {
        readonly IExeucutesSingleRule wrapped;
        readonly Stopwatch stopwatch = new Stopwatch();

        /// <inheritdoc/>
        public async Task<ValidationRuleResult> ExecuteRuleAsync(ExecutableRule rule, CancellationToken cancellationToken = default)
        {
            stopwatch.Reset();
            stopwatch.Start();
            var result = await wrapped.ExecuteRuleAsync(rule, cancellationToken).ConfigureAwait(false);
            stopwatch.Stop();
            
            var instrumentationData = new RuleInstrumentationData(rule.IsEligibleToBeExecutedInParallel, stopwatch.Elapsed);
            return new ValidationRuleResult(result,
                                            result.RuleContext,
                                            result.ValidationLogic,
                                            result.Message,
                                            instrumentationData);
        }

        /// <summary>
        /// Initialises a new instance of <see cref="InstrumentingSingleRuleExecutorDecorator"/>.
        /// </summary>
        /// <param name="wrapped">The wrapped service</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public InstrumentingSingleRuleExecutorDecorator(IExeucutesSingleRule wrapped)
        {
            this.wrapped = wrapped ?? throw new System.ArgumentNullException(nameof(wrapped));
        }
    }
}