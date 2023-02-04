using System;
using System.Collections.Generic;
using System.Linq;
using CSF.Validation.Manifest;

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
        /// Gets the name of the member whose value is being validated by this rule.
        /// </summary>
        public string MemberName { get; }

        /// <summary>
        /// Gets a string which represents the current rule identifier.
        /// </summary>
        /// <returns>A human-readable string.</returns>
        public override string ToString()
        {
            var properties = new Dictionary<string, string>
                {
                    { "Type", RuleType.FullName },
                    { "Name", RuleName },
                    { "Validated type", ValidatedType.FullName },
                    { "Validated identity", ObjectIdentity?.ToString() },
                }
                .Where(x => x.Value != null)
                .Select(x => $"{x.Key} = {x.Value}");
            return $"[{nameof(RuleIdentifier)}: {String.Join(", ", properties)}]";
        }

        /// <summary>
        /// Initializes a new instance of <see cref="RuleIdentifier"/>.
        /// </summary>
        /// <param name="ruleType">The rule type.</param>
        /// <param name="validatedValueType">The type of value that the rule validates.</param>
        /// <param name="objectIdentity">The object identity.</param>
        /// <param name="memberName">An optional member name.</param>
        /// <param name="ruleName">An optional rule name.</param>
        public RuleIdentifier(Type ruleType,
                              Type validatedValueType,
                              object objectIdentity,
                              string memberName = default,
                              string ruleName = default) : base(ruleType, validatedValueType, ruleName)
        {
            ObjectIdentity = objectIdentity;
            MemberName = memberName;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="RuleIdentifier"/> based upon an instance of <see cref="ManifestRule"/>.
        /// </summary>
        /// <param name="manifestRule">The manifest rule.</param>
        /// <param name="objectIdentity">The identity of the validated object.</param>
        public RuleIdentifier(ManifestRule manifestRule,
                              object objectIdentity) : this(manifestRule.Identifier.RuleType,
                                                            manifestRule.Identifier.ValidatedType,
                                                            objectIdentity,
                                                            (manifestRule.ManifestValue is ManifestValue val)? val.MemberName : null,
                                                            manifestRule.Identifier.RuleName) {}
    }
}