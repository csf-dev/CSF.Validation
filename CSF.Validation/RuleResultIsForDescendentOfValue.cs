using System;
using System.Linq.Expressions;
using System.Linq;
using CSF.Specifications;
using CSF.Validation.Manifest;

namespace CSF.Validation
{
    /// <summary>
    /// A specification for validation rule results which must be for a specified value, or a descendent of that value.
    /// </summary>
    public class RuleResultIsForDescendentOfValue : ISpecificationExpression<ValidationRuleResult>
    {
        readonly ValueContextIsMatchForItem valueConextIsMatch;
        readonly bool allowAncestors;

        /// <summary>
        /// Gets an expression which represents the current specification instance.
        /// </summary>
        /// <returns>The specification expression.</returns>
        public Expression<Func<ValidationRuleResult, bool>> GetExpression()
        {
            return result => valueConextIsMatch.Matches(result.RuleContext)
                          || (allowAncestors && result.RuleContext.AncestorContexts.Any(valueConextIsMatch));
        }

        /// <summary>
        /// Initialises a new instance of <see cref="RuleResultIsForDescendentOfValue"/> without a validated item instance.
        /// </summary>
        /// <param name="value">A manifest value which rules must be a descendent of in order to match the spec.</param>
        /// <param name="allowAncestors">If <see langword="true" /> then rule results will match if the value appears in any
        /// ancestor context, as well as the rule's own context.  If <see langword="false" /> then only the rule's immediate
        /// context will be searched, indicating that this is a specification for immediate children only.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="value"/> is <see langword="null" />.</exception>
        public RuleResultIsForDescendentOfValue(ManifestValueBase value, bool allowAncestors = true)
        {
            valueConextIsMatch = new ValueContextIsMatchForItem(value ?? throw new ArgumentNullException(nameof(value)));
            this.allowAncestors = allowAncestors;
        }

        /// <summary>
        /// Initialises a new instance of <see cref="RuleResultIsForDescendentOfValue"/> with a validated item instance..
        /// </summary>
        /// <param name="value">A manifest value which rules must be a descendent of in order to match the spec.</param>
        /// <param name="item">An optional item (of the type indicated by the <paramref name="value"/>) which the rule must be a descendent of.</param>
        /// <param name="allowAncestors">If <see langword="true" /> then rule results will match if the value appears in any
        /// ancestor context, as well as the rule's own context.  If <see langword="false" /> then only the rule's immediate
        /// context will be searched, indicating that this is a specification for immediate children only.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="value"/> is <see langword="null" />.</exception>
        public RuleResultIsForDescendentOfValue(ManifestValueBase value, object item, bool allowAncestors = true)
        {
            valueConextIsMatch = new ValueContextIsMatchForItem(value ?? throw new ArgumentNullException(nameof(value)), item);
            this.allowAncestors = allowAncestors;
        }
    }
}