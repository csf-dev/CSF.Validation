using System;
using System.Threading;
using System.Threading.Tasks;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// An adapter class for instances of <see cref="IRule{TValue, TValidated}"/>, which
    /// allows their logic to be executed as if they were instances of the
    /// non-generic <see cref="IValidationLogic"/> interface.
    /// </summary>
    /// <typeparam name="TValue">The type of value being validated by the current instance.</typeparam>
    /// <typeparam name="TValidated">The type of parent object being validated by the current instance.</typeparam>
    public class RuleWithParentValueAdapter<TValue,TValidated> : IValidationLogic
    {
        readonly IRule<TValue,TValidated> wrapped;

        /// <inheritdoc/>
        public Type RuleInterface => typeof(IRule<TValue, TValidated>);

        /// <inheritdoc/>
        public object RuleObject => wrapped;

        /// <inheritdoc/>
        public TimeSpan? GetTimeout() => wrapped is IHasRuleTimeout timeout ? timeout.GetTimeout() : null;

        /// <inheritdoc/>
        public ValueTask<RuleResult> GetResultAsync(object value, object parentValue, RuleContext context, CancellationToken token = default)
            => wrapped.GetResultAsync((TValue)value, (TValidated)parentValue, context, token);

        /// <summary>
        /// Initialises an instance of <see cref="RuleWithParentValueAdapter{TValue,TValidated}"/> from a specified rule.
        /// </summary>
        /// <param name="wrapped">The rule logic to be wrapped by the current instance.</param>
        public RuleWithParentValueAdapter(IRule<TValue,TValidated> wrapped)
        {
            this.wrapped = wrapped ?? throw new System.ArgumentNullException(nameof(wrapped));
        }
    }
}