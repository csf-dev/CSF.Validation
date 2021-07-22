using System;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// Represents information which uniquely identifies a validation rule within the validation process.
    /// </summary>
    /// <param name="ObjectIdentity">The unique identity of the object being validated.</param>
    /// <param name="RuleType">The type of the validation rule logic class</param>
    /// <param name="MemberName">
    /// Where applicable, the name of the member (such as a property or method) of the object being validated.
    /// This is often applicable when the rule derives from <see cref="IValueRule{TValue, TValidated}"/>, as it
    /// is the member which provides the "value" being validated.
    /// </param>
    /// <param name="RuleName">
    /// An optional rule name, which may be used to identify a rule where the same rule-type is used more than once
    /// (possibly for the same validated member).  The rule name may be used to provide some uniquely-identifying
    /// information about the rule, to distinguish it from other rules.
    /// </param>
    public record RuleIdentifier(object ObjectIdentity,
                                 Type RuleType,
                                 string MemberName = null,
                                 string RuleName = null);
}