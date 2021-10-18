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
        public ValidatedValue ValidatedValue { get; }

        /// <summary>
        /// Gets the manifest rule which the current instance corresponds to.
        /// </summary>
        public ManifestRule ManifestRule { get; }

        /// <summary>
        /// Gets the executable rule logic.
        /// </summary>
        public IValidationLogic RuleLogic { get; }

        /// <summary>
        /// Initialises a new instance of <see cref="ExecutableRule"/>.
        /// </summary>
        /// <param name="validatedValue">The validated value.</param>
        /// <param name="manifestRule">The corresponding manifest rule.</param>
        /// <param name="ruleLogic">The executable rule logic.</param>
        public ExecutableRule(ValidatedValue validatedValue, ManifestRule manifestRule, IValidationLogic ruleLogic)
        {
            this.ValidatedValue = validatedValue ?? throw new System.ArgumentNullException(nameof(validatedValue));
            this.ManifestRule = manifestRule ?? throw new System.ArgumentNullException(nameof(manifestRule));
            this.RuleLogic = ruleLogic ?? throw new System.ArgumentNullException(nameof(ruleLogic));
        }
    }
}