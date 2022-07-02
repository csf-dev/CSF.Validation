using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A rule that passes if a validated array, collection or string is either <see langword="null" /> or has a length/count of items
    /// between inclusive minimum/maximums, configured in this rule.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Either of the <see cref="Min"/> or <see cref="Max"/> properties may be <see langword="null" />, in which case they are ignored
    /// and not used.  If either are null then this rule becomes effectively "shorter/fewer than" or "longer/more than".
    /// If both are null then this rull becomes meaningless; it will always pass.
    /// This rule will also pass if the array, collection or string itself is <see langword="null" />.
    /// </para>
    /// <para>
    /// The logic of this rule does not verify that the Mininum length is not greater-than the Maximum length, or that neither
    /// of these values is negative.  Thus it is possible to set up scenarios where this rule will always return a failure
    /// result for any non-null collection, as the pass criteria cannot be satisfied.
    /// </para>
    /// <para>
    /// This rule will always return a synchronous result.
    /// </para>
    /// </remarks>
    [Parallelizable]
    public class LengthInRange : IRule<Array>, IRule<ICollection>, IRule<string>
    {
        readonly IntegerInRange inRangeRule;

        /// <summary>
        /// Gets or sets the minimum inclusive length (or count of items).
        /// </summary>
        public int? Min { get; set; }

        /// <summary>
        /// Gets or sets the maximum inclusive length (or count of items).
        /// </summary>
        public int? Max { get; set; }

        /// <summary>
        /// Performs the validation logic asynchronously and returns a task of <see cref="RuleResult"/>.
        /// </summary>
        /// <param name="validated">The object being validated</param>
        /// <param name="context">Contextual information about the validation</param>
        /// <param name="token">An object which may be used to cancel the process</param>
        /// <returns>A task which provides a result object, indicating the result of validation</returns>
        public Task<RuleResult> GetResultAsync(Array validated, RuleContext context, CancellationToken token = default)
        {
            if (validated is null) return PassAsync();
            inRangeRule.Min = Min;
            inRangeRule.Max = Max;
            return inRangeRule.GetResultAsync(validated.Length, context, token);
        }

        /// <summary>
        /// Performs the validation logic asynchronously and returns a task of <see cref="RuleResult"/>.
        /// </summary>
        /// <param name="validated">The object being validated</param>
        /// <param name="context">Contextual information about the validation</param>
        /// <param name="token">An object which may be used to cancel the process</param>
        /// <returns>A task which provides a result object, indicating the result of validation</returns>
        public Task<RuleResult> GetResultAsync(ICollection validated, RuleContext context, CancellationToken token = default)
        {
            if (validated is null) return PassAsync();
            inRangeRule.Min = Min;
            inRangeRule.Max = Max;
            return inRangeRule.GetResultAsync(validated.Count, context, token);
        }

        /// <summary>
        /// Performs the validation logic asynchronously and returns a task of <see cref="RuleResult"/>.
        /// </summary>
        /// <param name="validated">The object being validated</param>
        /// <param name="context">Contextual information about the validation</param>
        /// <param name="token">An object which may be used to cancel the process</param>
        /// <returns>A task which provides a result object, indicating the result of validation</returns>
        public Task<RuleResult> GetResultAsync(string validated, RuleContext context, CancellationToken token = default)
        {
            if (validated is null) return PassAsync();
            inRangeRule.Min = Min;
            inRangeRule.Max = Max;
            return inRangeRule.GetResultAsync(validated.Length, context, token);
        }

        /// <summary>
        /// Initialises a new instance of <see cref="LengthInRange"/>.
        /// </summary>
        /// <param name="inRangeRule">A number-in-range rule.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="inRangeRule"/> is <see langword="null" />.</exception>
        public LengthInRange(IntegerInRange inRangeRule)
        {
            this.inRangeRule = inRangeRule ?? throw new ArgumentNullException(nameof(inRangeRule));
        }
    }
}