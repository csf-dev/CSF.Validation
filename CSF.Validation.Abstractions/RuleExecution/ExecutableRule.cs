using System.Linq;
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
        /// Gets a value that indicates whether or not this rule may be executed in parallel.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This corresponds to whether or not the class that provides the rule logic is decorated
        /// with <see cref="ParallelizableAttribute"/>.
        /// In order to actually run validation with parallelisation, <see cref="ValidationOptions.EnableRuleParallelization"/>
        /// must also be set to <see langword="true" />.
        /// </para>
        /// </remarks>
        public bool IsEligibleToBeExecutedInParallel => RuleIdentifier.RuleType.CustomAttributes.OfType<ParallelizableAttribute>().Any();

        /// <summary>
        /// Gets a string which represents the current executable rule.
        /// </summary>
        /// <returns>A human-readable string.</returns>
        public override string ToString() => RuleIdentifier?.ToString() ?? "Unidentified executable rule";
    }
}