using System;
using static CSF.Validation.Resources.ExceptionMessages;

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
        /// Gets the type of the validation rule.
        /// </summary>
        public Type RuleType { get; }

        /// <summary>
        /// Gets the name of the member which the rule validates, where that rule validates a specific member value.
        /// </summary>
        public string MemberName { get; }

        /// <summary>
        /// Gets the name of the rule, where that rule has a name.
        /// </summary>
        public string RuleName { get; }

        /// <summary>
        /// Gets the number of levels of ancestry (within the parent/child validation hierarchy) which must be traversed
        /// from the 'current' rule, in order to reach the validator which contains the identified rule.
        /// </summary>
        public int? AncestorLevels { get; }

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
                                      int? ancestorLevels = default)
        {
            RuleType = ruleType ?? throw new ArgumentNullException(nameof(ruleType));
            MemberName = memberName;
            RuleName = ruleName;
            AncestorLevels = ancestorLevels;
        }
    }
}