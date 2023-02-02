using System;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Messages;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A validation rule which passes if the validated floating-point number or <see cref="Nullable{T}"/> floating-point number is within
    /// an inclusive <see cref="Min"/> &amp; <see cref="Max"/> range.  This rule works with all of the CLR-standard floating-point number types.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Either of <see cref="Min"/> or <see cref="Max"/> may be <see langword="null" />.  If either is null then either the minimum or maximum
    /// of the range will not be used (which means that this becomes simply a "greater-then" or "less-than" validation rule).  If both ends of
    /// the range are null then this validation rule will always pass.
    /// </para>
    /// <para>
    /// This validation rule may additionally be used with <see cref="Nullable{T}"/> numbers, in which case this rule will pass
    /// if the validated value is <see langword="null" />.  Combine this rule with a <see cref="NotNull"/> rule if nulls are not
    /// permitted.
    /// </para>
    /// <para>
    /// The logic of this rule does not verify that the minimum is not greater-than the maximum.  Thus it is possible to set up scenarios where
    /// this rule will always return a failure result for any number, as the pass criteria cannot be satisfied.
    /// </para>
    /// <para>
    /// This rule supports both non-nullable and nullable instances of <see cref="float"/> &amp; <see cref="double"/>.
    /// </para>
    /// <para>
    /// This rule will always return a synchronous result.
    /// </para>
    /// </remarks>
    [Parallelizable]
    public class FloatInRange : IRuleWithMessage<float>, IRuleWithMessage<double>, IRuleWithMessage<float?>, IRuleWithMessage<double?>
    {
        /// <summary>
        /// Gets or sets the (inclusive) minimum for the validated value.
        /// </summary>
        public double? Min { get; set; }
        
        /// <summary>
        /// Gets or sets the (inclusive) maximum for the validated value.
        /// </summary>
        public double? Max { get; set; }

        /// <inheritdoc/>
        public Task<RuleResult> GetResultAsync(double validated, RuleContext context, CancellationToken token = default)
        {
            var result = (!Min.HasValue || Min.Value <= validated)
                      && (!Max.HasValue || validated <= Max.Value);
            return result ? PassAsync() : FailAsync();
        }

        /// <inheritdoc/>
        public Task<RuleResult> GetResultAsync(double? validated, RuleContext context, CancellationToken token = default)
            => validated.HasValue ? GetResultAsync(validated.Value, context, token) : PassAsync();

        /// <inheritdoc/>
        public Task<string> GetFailureMessageAsync(double? value, ValidationRuleResult result, CancellationToken token = default)
        {
            if(Min.HasValue && Max.HasValue)
                return Task.FromResult(String.Format(Resources.FailureMessages.GetFailureMessage("IntegerInRangeRange"), Min, Max, value));
            if(Min.HasValue)
                return Task.FromResult(String.Format(Resources.FailureMessages.GetFailureMessage("IntegerInRangeMin"), Min, value));

            return Task.FromResult(String.Format(Resources.FailureMessages.GetFailureMessage("IntegerInRangeMax"), Max, value));
        }

        Task<RuleResult> IRule<float>.GetResultAsync(float validated, RuleContext context, CancellationToken token)
            => GetResultAsync((double)validated, context, token);

        Task<RuleResult> IRule<float?>.GetResultAsync(float? validated, RuleContext context, CancellationToken token)
            => GetResultAsync((double?)validated, context, token);

        Task<string> IGetsFailureMessage<float>.GetFailureMessageAsync(float value, ValidationRuleResult result, CancellationToken token)
            => GetFailureMessageAsync((double?)value, result, token);

        Task<string> IGetsFailureMessage<double>.GetFailureMessageAsync(double value, ValidationRuleResult result, CancellationToken token)
            => GetFailureMessageAsync((double?)value, result, token);

        Task<string> IGetsFailureMessage<float?>.GetFailureMessageAsync(float? value, ValidationRuleResult result, CancellationToken token)
            => GetFailureMessageAsync((double?)value, result, token);
    }
}