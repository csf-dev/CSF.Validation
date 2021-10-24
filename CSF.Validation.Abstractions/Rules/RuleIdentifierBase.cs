using System;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// Base model for rule identifiers.
    /// </summary>
    public abstract class RuleIdentifierBase
    {
        /// <summary>
        /// The type of the validation rule logic class.
        /// </summary>
        public Type RuleType { get; }

        /// <summary>
        /// An optional rule name, to uniquely identify this rule where other identifying information might be ambiguous.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Any rule may have a name, whether it needs one or not.  Rules only require a name when the other identifying
        /// information would not uniquely identify the rule.  That being:
        /// </para>
        /// <list type="bullet">
        /// <item><description>The rule type: <see cref="RuleType"/></description></item>
        /// <item><description>The object identifier (where applicable)</description></item>
        /// <item><description>The member name which provides the value being validated (where applicable)</description></item>
        /// </list>
        /// <para>
        /// In these cases where the three pieces of information above are insufficient to uniquely identify a rule, then
        /// the rule must be named in order for its identity to be unambiguous.  This is most common when the same rule type
        /// is applied more than once to the same validated value (but each time with different configuration).
        /// </para>
        /// </remarks>
        public string RuleName { get; }

        /// <summary>
        /// Gets the <see cref="Type"/> of value that the rule validates.
        /// </summary>
        public Type ValidatedType { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="RuleIdentifierBase"/>.
        /// </summary>
        /// <param name="ruleType">The rule type.</param>
        /// <param name="validatedType">The type of value that the rule validates.</param>
        /// <param name="ruleName">An optional rule name.</param>
        protected RuleIdentifierBase(Type ruleType, Type validatedType, string ruleName = default)
        {
            RuleType = ruleType ?? throw new ArgumentNullException(nameof(ruleType));
            ValidatedType = validatedType ?? throw new ArgumentNullException(nameof(validatedType));
            RuleName = ruleName;
        }
    }
}