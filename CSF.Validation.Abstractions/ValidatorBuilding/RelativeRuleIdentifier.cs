using System;
using CSF.Validation.Rules;

namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// A model for information which may uniquely identity a validation rule, based upon
    /// the logical 'position' (within a validator) of another rule.  In other words, it uniquely
    /// identifies a rule, based upon a current rule.
    /// </summary>
    public class RelativeRuleIdentifier
    {
        /// <summary>
        /// Gets the number of levels of ancestry (within the parent/child validation hierarchy) which must be traversed
        /// from the 'current' rule, in order to reach the validator which contains the identified rule.
        /// </summary>
        public int AncestorLevels { get; }

        /// <summary>
        /// Where the referenced rule is for a member of a value/object, this property gets the member name.
        /// </summary>
        public string MemberName { get; }

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
        /// Initializes a new instance of <see cref="RelativeRuleIdentifier"/>.
        /// </summary>
        /// <param name="ruleType">The rule type.</param>
        /// <param name="memberName">An optional member name.</param>
        /// <param name="ruleName">An optional rule name.</param>
        /// <param name="ancestorLevels">An optional number of ancestor levels.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="ruleType"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="ancestorLevels"/> is less than zero.</exception>
        public RelativeRuleIdentifier(Type ruleType,
                                      string memberName = default,
                                      string ruleName = default,
                                      int ancestorLevels = 0)
        {
            if(ancestorLevels < 0)
                throw new ArgumentOutOfRangeException(nameof(ancestorLevels));

            RuleType = ruleType ?? throw new ArgumentNullException(nameof(ruleType));
            RuleName = ruleName;
            MemberName = memberName;
            AncestorLevels = ancestorLevels;
        }
    }
}