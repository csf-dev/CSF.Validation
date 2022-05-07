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

        /// <summary>
        /// Gets the type of rule interface that is used by this rule logic.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This will be a closed-generic form of either <see cref="IRule{TValidated}"/> or
        /// <see cref="IRule{TValue, TParent}"/>.
        /// </para>
        /// </remarks>
        public Type RuleInterface => wrapped.RuleInterface;

        /// <summary>
        /// Gets a reference to the original/raw rule object instance.
        /// </summary>
        public object RuleObject => wrapped.RuleObject;

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