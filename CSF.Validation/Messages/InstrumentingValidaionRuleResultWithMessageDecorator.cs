using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace CSF.Validation.Messages
{
    /// <summary>
    /// Decorator for <see cref="IGetsValidationRuleResultWithMessage"/> which adds instrumentation data to the result.
    /// </summary>
    public class InstrumentingValidaionRuleResultWithMessageDecorator : IGetsValidationRuleResultWithMessage
    {
        readonly IGetsValidationRuleResultWithMessage wrapped;
        readonly Stopwatch stopwatch = new Stopwatch();

        /// <inheritdoc/>
        public async ValueTask<ValidationRuleResult> GetRuleResultWithMessageAsync(ValidationRuleResult ruleResult, CancellationToken cancellationToken = default)
        {
            stopwatch.Reset();
            stopwatch.Start();
            var result = await wrapped.GetRuleResultWithMessageAsync(ruleResult, cancellationToken).ConfigureAwait(false);
            stopwatch.Stop();
            
            var instrumentationData = new RuleInstrumentationData((result.InstrumentationData?.ParallelizationEnabled).GetValueOrDefault(),
                                                                  (result.InstrumentationData?.RuleExecutionTime).GetValueOrDefault(),
                                                                  stopwatch.Elapsed);
            return new ValidationRuleResult(result,
                                            result.RuleContext,
                                            result.ValidationLogic,
                                            result.Message,
                                            instrumentationData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wrapped"></param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public InstrumentingValidaionRuleResultWithMessageDecorator(IGetsValidationRuleResultWithMessage wrapped)
        {
            this.wrapped = wrapped ?? throw new System.ArgumentNullException(nameof(wrapped));
        }
    }
}