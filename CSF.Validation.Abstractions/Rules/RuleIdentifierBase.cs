using System;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// Base model for rule identifiers.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The reason for this base class is because a manifest needs a function to get an object
    /// identity, wheras an actual rule simply needs a concrete identity.  All other properties
    /// are shared though.
    /// </para>
    /// </remarks>
    public abstract class RuleIdentifierBase
    {
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
        /// Initializes a new instance of <see cref="RuleIdentifierBase"/>.
        /// </summary>
        /// <param name="ruleType">The rule type.</param>
        /// <param name="memberName">An optional member name.</param>
        /// <param name="ruleName">An optional rule name.</param>
        protected RuleIdentifierBase(Type ruleType,
                                     string memberName = default,
                                     string ruleName = default)
        {
            RuleType = ruleType;
            MemberName = memberName;
            RuleName = ruleName;
        }
    }
}