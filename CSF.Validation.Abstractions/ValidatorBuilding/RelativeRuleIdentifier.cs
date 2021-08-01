using System;
using CSF.Validation.Rules;

namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// A model for information which may uniquely identity a validation rule, based upon
    /// the logical 'position' (within a validator) of another rule.  In other words, it uniquely
    /// identifies a rule, based upon a current rule.
    /// </summary>
    public class RelativeRuleIdentifier : RuleIdentifierBase
    {
        /// <summary>
        /// Gets the number of levels of ancestry (within the parent/child validation hierarchy) which must be traversed
        /// from the 'current' rule, in order to reach the validator which contains the identified rule.
        /// </summary>
        public int? AncestorLevels { get; }

        /// <summary>
        /// Where the referenced rule is for a member of a value/object, this property gets the member name.
        /// </summary>
        public string MemberName { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="RelativeRuleIdentifier"/>.
        /// </summary>
        /// <param name="ruleType">The rule type.</param>
        /// <param name="memberName">An optional member name.</param>
        /// <param name="ruleName">An optional rule name.</param>
        /// <param name="ancestorLevels">An optional number of ancestor levels.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="ruleType"/> is <see langword="null"/>.</exception>
        public RelativeRuleIdentifier(Type ruleType,
                                      string memberName = default,
                                      string ruleName = default,
                                      int? ancestorLevels = default) : base(ruleType, ruleName)
        {
            MemberName = memberName;
            AncestorLevels = ancestorLevels;
        }
    }
}