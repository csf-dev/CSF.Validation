using System;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Messages;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A validation rule which asserts that the validated value (which may be nullable) is a defined member
    /// of the specified enumerated type.
    /// </summary>
    /// <typeparam name="T">The type of the enum against which to verify that the validated value is a defined member.</typeparam>
    /// <remarks>
    /// <para>
    /// This validation rule is also compatible with nullable values of <typeparamref name="T"/>, and will return a passing
    /// result if the validated value is <see langword="null" />.
    /// </para>
    /// </remarks>
    [Parallelizable]
    public class MustBeDefinedEnumConstant<T> : IRuleWithMessage<T>, IRuleWithMessage<T?> where T : struct, Enum
    {
        /// <inheritdoc/>
        public ValueTask<RuleResult> GetResultAsync(T? validated, RuleContext context, CancellationToken token = default)
        {
            if(!validated.HasValue) return PassAsync();
            return Enum.IsDefined(typeof(T), validated) ? PassAsync() : FailAsync();
        }

        ValueTask<RuleResult> IRule<T>.GetResultAsync(T validated, RuleContext context, CancellationToken token)
            => GetResultAsync(validated, context, token);

        /// <inheritdoc/>
        public ValueTask<string> GetFailureMessageAsync(T? value, ValidationRuleResult result, CancellationToken token = default)
            => new ValueTask<string>(MustBeDefinedEnumConstant.GetFailureMessage(typeof(T)));

        ValueTask<string> IGetsFailureMessage<T>.GetFailureMessageAsync(T value, ValidationRuleResult result, CancellationToken token)
            => GetFailureMessageAsync(value, result, token);
    }
}