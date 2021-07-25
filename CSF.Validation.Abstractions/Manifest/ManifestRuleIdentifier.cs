using System;
using CSF.Validation.Rules;

namespace CSF.Validation.Manifest
{
    /// <summary>
    /// A model for information which uniquely identifies a validation rule within a validation manifest.
    /// </summary>
    public class ManifestRuleIdentifier : RuleIdentifierBase
    {
        /// <summary>
        /// A function which retrieves a unique identity of the object being
        /// validated, given a reference to that object being validated.
        /// </summary>
        public Func<object,object> ObjectIdentityAccessor { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="RuleIdentifier"/>.
        /// </summary>
        /// <param name="objectIdentityAccessor">The function to get the object identity.</param>
        /// <param name="ruleType">The rule type.</param>
        /// <param name="memberName">An optional member name.</param>
        /// <param name="ruleName">An optional rule name.</param>
        public ManifestRuleIdentifier(Func<object,object> objectIdentityAccessor,
                                      Type ruleType,
                                      string memberName = default,
                                      string ruleName = default) : base(ruleType, memberName, ruleName)
        {
            ObjectIdentityAccessor = objectIdentityAccessor;
        }
    }
}