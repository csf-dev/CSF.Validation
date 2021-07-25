using System;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A model for information which uniquely identifies a validation rule within the validation process.
    /// </summary>
    public class RuleIdentifier : RuleIdentifierBase
    {
        /// <summary>
        /// The unique identity of the object being validated.
        /// </summary>
        public object ObjectIdentity { get; }

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
                              string ruleName = default) : base(ruleType, memberName, ruleName)
        {
            ObjectIdentity = objectIdentity;
        }
    }
}