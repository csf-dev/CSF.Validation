using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Messages;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A rule that passes if a validated collection or queryable is either <see langword="null" /> or has a count of items
    /// between inclusive minimum/maximums, configured in this rule.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Either of the <see cref="Min"/> or <see cref="Max"/> properties may be <see langword="null" />, in which case they are ignored
    /// and not used.  If either are null then this rule becomes effectively "fewer than" or "more than".
    /// If both are null then this rull becomes meaningless; it will always pass.
    /// This rule will also pass if the collection or queryable itself is <see langword="null" />.
    /// </para>
    /// <para>
    /// The logic of this rule does not verify that the Mininum count is not greater-than the Maximum count, or that neither
    /// of these values is negative.  Thus it is possible to set up scenarios where this rule will always return a failure
    /// result for any non-null collection/queryable, as the pass criteria cannot be satisfied.
    /// </para>
    /// <para>
    /// This rule will always return a synchronous result.
    /// </para>
    /// </remarks>
    [Parallelizable]
    public class CountInRange<T> : IRuleWithMessage<ICollection<T>>, IRuleWithMessage<IReadOnlyCollection<T>>, IRuleWithMessage<IQueryable<T>>
    {
        readonly IntegerInRange inRangeRule;

        /// <summary>
        /// Gets or sets the minimum inclusive count of items.
        /// </summary>
        public int? Min { get; set; }
        
        /// <summary>
        /// Gets or sets the maximum inclusive count of items.
        /// </summary>
        public int? Max { get; set; }

        /// <inheritdoc/>
        public ValueTask<RuleResult> GetResultAsync(ICollection<T> validated, RuleContext context, CancellationToken token = default)
        {
            if(validated is null) return PassAsync();
            inRangeRule.Min = Min;
            inRangeRule.Max = Max;
            return inRangeRule.GetResultAsync(validated.Count, context, token);
        }

        /// <inheritdoc/>
        public ValueTask<RuleResult> GetResultAsync(IQueryable<T> validated, RuleContext context, CancellationToken token = default)
        {
            if(validated is null) return PassAsync();
            // Operations upon the queryable might be expensive, so skip them if neither min or max is set.
            if(!Min.HasValue && !Max.HasValue) return PassAsync();
            inRangeRule.Min = Min;
            inRangeRule.Max = Max;
            return inRangeRule.GetResultAsync(validated.Count(), context, token);
        }

        ValueTask<RuleResult> IRule<IReadOnlyCollection<T>>.GetResultAsync(IReadOnlyCollection<T> validated, RuleContext context, CancellationToken token)
        {
            if(validated is null) return PassAsync();
            inRangeRule.Min = Min;
            inRangeRule.Max = Max;
            return inRangeRule.GetResultAsync(validated.Count, context, token);
        }

        ValueTask<string> GetFailureMessageAsync(ValidationRuleResult result)
        {
            if(Min.HasValue && Max.HasValue)
                return new ValueTask<string>(String.Format(Resources.FailureMessages.GetFailureMessage("CountInRangeRange"), Min, Max, result.Data[IntegerInRange.ActualKey]));
            if(Min.HasValue)
                return new ValueTask<string>(String.Format(Resources.FailureMessages.GetFailureMessage("CountInRangeMin"), Min, result.Data[IntegerInRange.ActualKey]));

            return new ValueTask<string>(String.Format(Resources.FailureMessages.GetFailureMessage("CountInRangeMax"), Max, result.Data[IntegerInRange.ActualKey]));
        }

        /// <inheritdoc/>
        public ValueTask<string> GetFailureMessageAsync(ICollection<T> value, ValidationRuleResult result, CancellationToken token = default)
            => GetFailureMessageAsync(result);

        ValueTask<string> IGetsFailureMessage<IReadOnlyCollection<T>>.GetFailureMessageAsync(IReadOnlyCollection<T> value, ValidationRuleResult result, CancellationToken token)
            => GetFailureMessageAsync(result);

        ValueTask<string> IGetsFailureMessage<IQueryable<T>>.GetFailureMessageAsync(IQueryable<T> value, ValidationRuleResult result, CancellationToken token)
            => GetFailureMessageAsync(result);
        
        /// <summary>
        /// Initialises a new instance of <see cref="CountInRange{T}"/>.
        /// </summary>
        /// <param name="inRangeRule">A number-in-range rule.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="inRangeRule"/> is <see langword="null" />.</exception>
        public CountInRange(IntegerInRange inRangeRule)
        {
            this.inRangeRule = inRangeRule ?? throw new ArgumentNullException(nameof(inRangeRule));
        }
    }
}