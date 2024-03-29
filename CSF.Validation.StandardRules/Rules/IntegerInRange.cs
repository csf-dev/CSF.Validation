using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Messages;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A validation rule which passes if the validated integer or <see cref="Nullable{T}"/> integer is within an inclusive
    /// <see cref="Min"/> &amp; <see cref="Max"/> range.  This rule works with all of the CLR-standard integer types.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Either of <see cref="Min"/> or <see cref="Max"/> may be <see langword="null" />.  If either is null then either the minimum or maximum
    /// of the range will not be used (which means that this becomes simply a "greater-then" or "less-than" validation rule).  If both ends of
    /// the range are null then this validation rule will always pass.
    /// </para>
    /// <para>
    /// This validation rule may additionally be used with <see cref="Nullable{T}"/> integers, in which case this rule will pass
    /// if the validated value is <see langword="null" />.  Combine this rule with a <see cref="NotNull"/> rule if nulls are not
    /// permitted.
    /// </para>
    /// <para>
    /// The logic of this rule does not verify that the minimum is not greater-than the maximum.  Thus it is possible to set up scenarios where
    /// this rule will always return a failure result for any number, as the pass criteria cannot be satisfied.
    /// </para>
    /// <para>
    /// This rule supports both non-nullable and nullable instances of <see cref="byte"/>, <see cref="short"/>, <see cref="int"/> &amp; <see cref="long"/>.
    /// </para>
    /// <para>
    /// This rule will always return a synchronous result.
    /// </para>
    /// </remarks>
    [Parallelizable]
    public class IntegerInRange   : IRuleWithMessage<byte>,
                                    IRuleWithMessage<short>,
                                    IRuleWithMessage<int>,
                                    IRuleWithMessage<long>,
                                    IRuleWithMessage<byte?>,
                                    IRuleWithMessage<short?>,
                                    IRuleWithMessage<int?>,
                                    IRuleWithMessage<long?>
    {
        internal const string MinKey = "Minimum", MaxKey = "Maximum", ActualKey = "Actual";

        /// <summary>
        /// Gets or sets the (inclusive) minimum for the validated value.
        /// </summary>
        public long? Min { get; set; }
        
        /// <summary>
        /// Gets or sets the (inclusive) maximum for the validated value.
        /// </summary>
        public long? Max { get; set; }

        /// <inheritdoc/>
        public ValueTask<RuleResult> GetResultAsync(long validated, RuleContext context, CancellationToken token = default)
        {
            var result = (!Min.HasValue || Min.Value <= validated)
                      && (!Max.HasValue || validated <= Max.Value);
            var data = new Dictionary<string, object> { { MinKey, Min }, { MaxKey, Max }, { ActualKey, validated } };
            return result ? PassAsync(data) : FailAsync(data);
        }

        /// <inheritdoc/>
        public ValueTask<RuleResult> GetResultAsync(long? validated, RuleContext context, CancellationToken token = default)
            => validated.HasValue ? GetResultAsync(validated.Value, context, token) : PassAsync(new Dictionary<string, object> { { MinKey, Min }, { MaxKey, Max }, { ActualKey, validated } });

        /// <inheritdoc/>
        public ValueTask<string> GetFailureMessageAsync(long? value, ValidationRuleResult result, CancellationToken token = default)
            => new ValueTask<string>(GetFailureMessage<long>(value, result, Min, Max));

        ValueTask<string> IGetsFailureMessage<byte>.GetFailureMessageAsync(byte value, ValidationRuleResult result, CancellationToken token)
            => GetFailureMessageAsync(value, result, token);

        ValueTask<string> IGetsFailureMessage<short>.GetFailureMessageAsync(short value, ValidationRuleResult result, CancellationToken token)
            => GetFailureMessageAsync(value, result, token);

        ValueTask<string> IGetsFailureMessage<int>.GetFailureMessageAsync(int value, ValidationRuleResult result, CancellationToken token)
            => GetFailureMessageAsync(value, result, token);

        ValueTask<string> IGetsFailureMessage<long>.GetFailureMessageAsync(long value, ValidationRuleResult result, CancellationToken token)
            => GetFailureMessageAsync(value, result, token);

        ValueTask<string> IGetsFailureMessage<byte?>.GetFailureMessageAsync(byte? value, ValidationRuleResult result, CancellationToken token)
            => GetFailureMessageAsync(value, result, token);

        ValueTask<string> IGetsFailureMessage<short?>.GetFailureMessageAsync(short? value, ValidationRuleResult result, CancellationToken token)
            => GetFailureMessageAsync(value, result, token);

        ValueTask<string> IGetsFailureMessage<int?>.GetFailureMessageAsync(int? value, ValidationRuleResult result, CancellationToken token)
            => GetFailureMessageAsync(value, result, token);

        ValueTask<RuleResult> IRule<byte>.GetResultAsync(byte validated, RuleContext context, CancellationToken token)
            => GetResultAsync((long)validated, context, token);

        ValueTask<RuleResult> IRule<short>.GetResultAsync(short validated, RuleContext context, CancellationToken token)
            => GetResultAsync((long)validated, context, token);

        ValueTask<RuleResult> IRule<int>.GetResultAsync(int validated, RuleContext context, CancellationToken token)
            => GetResultAsync((long)validated, context, token);

        ValueTask<RuleResult> IRule<byte?>.GetResultAsync(byte? validated, RuleContext context, CancellationToken token)
            => GetResultAsync((long?)validated, context, token);

        ValueTask<RuleResult> IRule<short?>.GetResultAsync(short? validated, RuleContext context, CancellationToken token)
            => GetResultAsync((long?)validated, context, token);

        ValueTask<RuleResult> IRule<int?>.GetResultAsync(int? validated, RuleContext context, CancellationToken token)
            => GetResultAsync((long?)validated, context, token);

        internal static string GetFailureMessage<TNumeric>(TNumeric? value, ValidationRuleResult result, TNumeric? min, TNumeric? max) where TNumeric : struct
        {
            if(min.HasValue && max.HasValue)
                return String.Format(Resources.FailureMessages.GetFailureMessage("IntegerInRangeRange"), min, max, value);
            if(min.HasValue)
                return String.Format(Resources.FailureMessages.GetFailureMessage("IntegerInRangeMin"), min, value);

            return String.Format(Resources.FailureMessages.GetFailureMessage("IntegerInRangeMax"), max, value);
        }
    }
}