using System.Collections.Generic;
using System.Linq;
using CSF.Validation.RuleExecution;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A factory service that creates instances of <see cref="RuleContext"/> from executable rules.
    /// </summary>
    public class RuleContextFactory : IGetsRuleContext
    {
        /// <summary>
        /// Gets a rule context for the specified executable rule.
        /// </summary>
        /// <param name="rule">The executable rule.</param>
        /// <returns>A rule context.</returns>
        public RuleContext GetRuleContext(ExecutableRule rule)
            => new RuleContext(rule.ManifestRule,
                               rule.RuleIdentifier,
                               rule.ValidatedValue.ActualValue,
                               GetAncestorContexts(rule).ToList(),
                               rule.ValidatedValue.CollectionItemOrder);

        static IEnumerable<ValueContext> GetAncestorContexts(ExecutableRule rule)
        {
            var value = rule.ValidatedValue;

            while(!(value.ParentValue is null))
            {
                var current = value.ParentValue;
                yield return new ValueContext(current.ValueIdentity, current.ActualValue, current.ManifestValue, current.CollectionItemOrder);
                value = current;
            }
        }
    }
}