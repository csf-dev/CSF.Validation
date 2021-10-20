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
    }
}