using System;
using System.Threading;
using System.Threading.Tasks;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A validation rule which passes if the validated <see cref="DateTime"/> or <see cref="Nullable{T}"/> DateTime is within an inclusive
    /// <see cref="Start"/> &amp; <see cref="End"/> range.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Either of <see cref="Start"/> or <see cref="End"/> may be <see langword="null" />.  If either is null then either the start or end
    /// of the range will not be used (which means that this becomes simply a "before" or "after" validation rule).  If both ends of the range
    /// are null then this validation rule will always pass.
    /// </para>
    /// <para>
    /// This validation rule may additionally be used with <see cref="Nullable{T}"/> DateTime instances, in which case this rule will pass
    /// if the validated value is <see langword="null" />.  Combine this rule with a <see cref="NotNull"/> rule if null DateTimes are not
    /// permitted.
    /// </para>
    /// <para>
    /// The logic of this rule does not verify that the Start is not later-than the End.  Thus it is possible to set up scenarios where this rule
    /// will always return a failure result for any DateTime, as the pass criteria cannot be satisfied.
    /// </para>
    /// <para>
    /// This rule will always return a synchronous result.
    /// </para>
    /// </remarks>
    [Parallelizable]
    public class DateTimeInRange : IRule<DateTime>, IRule<DateTime?>
    {
        /// <summary>
        /// Gets or sets the (inclusive) start <see cref="DateTime"/> for validating DateTimes.
        /// </summary>
        public DateTime? Start { get; set; }

        /// <summary>
        /// Gets or sets the (inclusive) end <see cref="DateTime"/> for validating DateTimes.
        /// </summary>
        public DateTime? End { get; set; }

        /// <summary>
        /// Performs the validation logic asynchronously and returns a task of <see cref="RuleResult"/>.
        /// </summary>
        /// <param name="validated">The object being validated</param>
        /// <param name="context">Contextual information about the validation</param>
        /// <param name="token">An object which may be used to cancel the process</param>
        /// <returns>A task which provides a result object, indicating the result of validation</returns>
        public Task<RuleResult> GetResultAsync(DateTime? validated, RuleContext context, CancellationToken token = default)
            => !validated.HasValue ? PassAsync() : GetResultAsync(validated.Value, context, token);

        /// <summary>
        /// Performs the validation logic asynchronously and returns a task of <see cref="RuleResult"/>.
        /// </summary>
        /// <param name="validated">The object being validated</param>
        /// <param name="context">Contextual information about the validation</param>
        /// <param name="token">An object which may be used to cancel the process</param>
        /// <returns>A task which provides a result object, indicating the result of validation</returns>
        public Task<RuleResult> GetResultAsync(DateTime validated, RuleContext context, CancellationToken token = default)
        {
            var result = (!Start.HasValue || Start.Value <= validated)
                      && (!End.HasValue   || validated   <= End.Value);
            return result ? PassAsync() : FailAsync();
        }
    }
}