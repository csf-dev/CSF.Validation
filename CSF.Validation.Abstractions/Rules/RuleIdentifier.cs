using System;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// Represents information which uniquely identifies a validation rule within the validation process.
    /// </summary>
    public class RuleIdentifier
    {
        /// <summary>
        /// The unique identity of the object being validated.
        /// </summary>
        public object ObjectIdentity { get; }

        /// <summary>
        /// The type of the validation rule logic class.
        /// </summary>
        public Type RuleType { get; }

        /// <summary>
        /// Where applicable, the name of the member (such as a property or method) of the object being validated.
        /// This is often applicable when the rule derives from <see cref="IValueRule{TValue, TValidated}"/>, as it
        /// is the member which provides the "value" being validated.
        /// </summary>
        public string MemberName { get; }

        /// <summary>
        /// An optional rule name, which may be used to identify a rule where the same rule-type is used more than once
        /// (possibly for the same validated member).  The rule name may be used to provide some uniquely-identifying
        /// information about the rule, to distinguish it from other rules.
        /// </summary>
        public string RuleName { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="RuleIdentifier"/>.
        /// </summary>
        /// <param name="objectIdentity">The object identity.</param>
        /// <param name="ruleType">The rule type.</param>
        /// <param name="memberName">An optional member name.</param>
        /// <param name="ruleName">An optional rule name.</param>
        public RuleIdentifier(object objectIdentity,
                              Type ruleType,
                              string memberName = default,
                              string ruleName = default)
        {
            ObjectIdentity = objectIdentity;
            RuleType = ruleType;
            MemberName = memberName;
            RuleName = ruleName;
        }
    }
}