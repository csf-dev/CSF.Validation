using System;
using System.Threading;
using System.Threading.Tasks;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// An adapter class for instances of <see cref="IRule{TValidated}"/>, which
    /// allows their logic to be executed as if they were instances of the
    /// non-generic <see cref="IValidationLogic"/> interface.
    /// </summary>
    /// <typeparam name="TValidated">The type of object being validated by the current instance.</typeparam>
    public class RuleAdapter<TValidated> : IValidationLogic
    {
        readonly IRule<TValidated> wrapped;

        /// <inheritdoc/>
        public Type RuleInterface => typeof(IRule<TValidated>);

        /// <inheritdoc/>
        public object RuleObject => wrapped;

        /// <inheritdoc/>
        public TimeSpan? GetTimeout() => wrapped is IHasRuleTimeout timeout ? timeout.GetTimeout() : null;

        /// <inheritdoc/>
        public Task<RuleResult> GetResultAsync(object value, object parentValue, RuleContext context, CancellationToken token = default)
            => wrapped.GetResultAsync((TValidated)value, context, token);

        /// <summary>
        /// Initialises an instance of <see cref="RuleAdapter{TValidated}"/> from a specified rule.
        /// </summary>
        /// <param name="wrapped">The rule logic to be wrapped by the current instance.</param>
        public RuleAdapter(IRule<TValidated> wrapped)
        {
            this.wrapped = wrapped ?? throw new System.ArgumentNullException(nameof(wrapped));
        }
    }
}