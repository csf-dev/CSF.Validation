using System;
using static CSF.Validation.Resources.ExceptionMessages;

namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// Represents information which may uniquely identity a validation rule, based upon
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
        /// <remarks>
        /// <para>
        /// This property is mutually exclusive with <see cref="ObjectIdentity"/>.  Either this or object identity may
        /// have a non-null value, but never both.  It is also permitted for neither to have a value.
        /// </para>
        /// </remarks>
        public int? AncestorLevels { get; }

        /// <summary>
        /// Gets the identity for the validated-object for which the validator contains the identified rule.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property is mutually exclusive with <see cref="AncestorLevels"/>.  Either this or ancestor levels may
        /// have a non-null value, but never both.  It is also permitted for neither to have a value.
        /// </para>
        /// </remarks>
        public object ObjectIdentity { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="RelativeRuleIdentifier"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The <paramref name="ancestorLevels"/> and <paramref name="objectIdentity"/> parameters are mutually
        /// exclusive if specified.  If one is specified and not-null then the other must be <see langword="null"/>.
        /// It is also acceptable for neither to be specified (or for both to be set to <see langword="null"/>).
        /// </para>
        /// </remarks>
        /// <param name="ruleType">The rule type.</param>
        /// <param name="memberName">An optional member name.</param>
        /// <param name="ruleName">An optional rule name.</param>
        /// <param name="ancestorLevels">An optional number of ancestor levels.</param>
        /// <param name="objectIdentity">An optional object identity.</param>
        public RelativeRuleIdentifier(Type ruleType,
                                      string memberName = default,
                                      string ruleName = default,
                                      int? ancestorLevels = default,
                                      object objectIdentity = default)
        {
            if(!(objectIdentity is null) && ancestorLevels.HasValue)
            {
                var message = String.Format(GetExceptionMessage("RelativeIdentityMayNotHaveBothObjectIdentityAndAncestorLevels"),
                                            nameof(RelativeRuleIdentifier));
                throw new ArgumentException(message, nameof(objectIdentity));
            }

            RuleType = ruleType ?? throw new ArgumentNullException(nameof(ruleType));
            MemberName = memberName;
            RuleName = ruleName;
            AncestorLevels = ancestorLevels;
            ObjectIdentity = objectIdentity;
        }
    }
}