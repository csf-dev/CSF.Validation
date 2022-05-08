using CSF.Validation.Manifest;
using CSF.Validation.Rules;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// A model for a rule which may be executed upon a validated value.
    /// </summary>
    public class ExecutableRule
    {
        /// <summary>
        /// Gets the value to be validated by the current rule.
        /// </summary>
        public ValidatedValue ValidatedValue { get; set; }

        /// <summary>
        /// Gets the manifest rule which the current instance corresponds to.
        /// </summary>
        public ManifestRule ManifestRule { get; set; }

        /// <summary>
        /// Gets the executable rule logic.
        /// </summary>
        public IValidationLogic RuleLogic { get; set; }

        /// <summary>
        /// Gets or sets the rule's result.
        /// </summary>
        public RuleResult Result { get; set; }

        /// <summary>
        /// Gets or sets the identifier for this particular rule.
        /// </summary>
        public RuleIdentifier RuleIdentifier { get; set; }

        /// <summary>
        /// Gets a string which represents the current executable rule.
        /// </summary>
        /// <returns>A human-readable string.</returns>
        public override string ToString() => RuleIdentifier?.ToString() ?? "Unidentified executable rule";
    }
}