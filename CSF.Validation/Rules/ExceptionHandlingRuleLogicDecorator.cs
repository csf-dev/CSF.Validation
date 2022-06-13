using System;
using System.Threading;
using System.Threading.Tasks;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A decorator class for the <see cref="IValidationLogic"/> interface which catches
    /// unhandled exceptions raised by the wrapped rule logic and instead returns a <see cref="RuleResult"/>
    /// with <see cref="RuleOutcome.Errored"/>.
    /// </summary>
    public class ExceptionHandlingRuleLogicDecorator : IValidationLogic
    {
        readonly IValidationLogic wrapped;

        /// <inheritdoc/>
        public Type RuleInterface => wrapped.RuleInterface;

        /// <inheritdoc/>
        public object RuleObject => wrapped.RuleObject;

        /// <inheritdoc/>
        public TimeSpan? GetTimeout() => wrapped.GetTimeout();

        /// <inheritdoc/>
        public async Task<RuleResult> GetResultAsync(object value, object parentValue, RuleContext context, CancellationToken token = default)
        {
            try
            {
                return await wrapped.GetResultAsync(value, parentValue, context, token).ConfigureAwait(false);
            }
            catch(Exception e)
            {
                return Error(e);
            }
        }

        /// <summary>
        /// Initialises an instance of <see cref="ExceptionHandlingRuleLogicDecorator"/>.
        /// </summary>
        /// <param name="wrapped">The rule logic to be wrapped by the current instance.</param>
        public ExceptionHandlingRuleLogicDecorator(IValidationLogic wrapped)
        {
            this.wrapped = wrapped ?? throw new ArgumentNullException(nameof(wrapped));
        }
    }
}