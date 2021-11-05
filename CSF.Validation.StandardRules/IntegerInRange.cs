using System;
using System.Threading;
using System.Threading.Tasks;
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
    /// </remarks>
    public class IntegerInRange : IRule<byte>, IRule<short>, IRule<int>, IRule<long>, IRule<byte?>, IRule<short?>, IRule<int?>, IRule<long?>
    {
        /// <summary>
        /// Gets or sets the (inclusive) minimum for the validated value.
        /// </summary>
        public long? Min { get; set; }
        
        /// <summary>
        /// Gets or sets the (inclusive) maximum for the validated value.
        /// </summary>
        public long? Max { get; set; }

        /// <summary>
        /// Performs the validation logic asynchronously and returns a task of <see cref="RuleResult"/>.
        /// </summary>
        /// <param name="validated">The object being validated</param>
        /// <param name="context">Contextual information about the validation</param>
        /// <param name="token">An object which may be used to cancel the process</param>
        /// <returns>A task which provides a result object, indicating the result of validation</returns>
        public Task<RuleResult> GetResultAsync(long validated, RuleContext context, CancellationToken token = default)
        {
            var result = (Min.HasValue ? Min.Value <= validated : true)
                      && (Max.HasValue ? validated <= Max.Value : true);
            return result ? PassAsync() : FailAsync();
        }

        /// <summary>
        /// Performs the validation logic asynchronously and returns a task of <see cref="RuleResult"/>.
        /// </summary>
        /// <param name="validated">The object being validated</param>
        /// <param name="context">Contextual information about the validation</param>
        /// <param name="token">An object which may be used to cancel the process</param>
        /// <returns>A task which provides a result object, indicating the result of validation</returns>
        public Task<RuleResult> GetResultAsync(long? validated, RuleContext context, CancellationToken token = default)
            => validated.HasValue ? GetResultAsync(validated.Value, context, token) : PassAsync();

        Task<RuleResult> IRule<byte>.GetResultAsync(byte validated, RuleContext context, CancellationToken token)
            => GetResultAsync((long)validated, context, token);

        Task<RuleResult> IRule<short>.GetResultAsync(short validated, RuleContext context, CancellationToken token)
            => GetResultAsync((long)validated, context, token);

        Task<RuleResult> IRule<int>.GetResultAsync(int validated, RuleContext context, CancellationToken token)
            => GetResultAsync((long)validated, context, token);

        Task<RuleResult> IRule<byte?>.GetResultAsync(byte? validated, RuleContext context, CancellationToken token)
            => GetResultAsync((long?)validated, context, token);

        Task<RuleResult> IRule<short?>.GetResultAsync(short? validated, RuleContext context, CancellationToken token)
            => GetResultAsync((long?)validated, context, token);

        Task<RuleResult> IRule<int?>.GetResultAsync(int? validated, RuleContext context, CancellationToken token)
            => GetResultAsync((long?)validated, context, token);
    }
}