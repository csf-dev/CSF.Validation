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

        /// <summary>
        /// Gets the type of rule interface that is used by this rule logic.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This will be a closed-generic form of either <see cref="IRule{TValidated}"/> or
        /// <see cref="IRule{TValue, TParent}"/>.
        /// </para>
        /// </remarks>
        public Type RuleInterface => typeof(IRule<TValidated>);

        /// <summary>
        /// Gets a reference to the original/raw rule object instance.
        /// </summary>
        public object RuleObject => wrapped;

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