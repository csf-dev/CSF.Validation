using System.Collections.Generic;

namespace CSF.Validation
{
    /// <summary>
    /// An object which can provide a collection of validation rule results.
    /// </summary>
    public interface IHasValidationRuleResults
    {
        /// <summary>
        /// Gets a simple value which indicates whether or not this result represents passing validation
        /// without any errors.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If this property returns <see langword="true" /> then every one of the <see cref="RuleResults"/> has
        /// an <see cref="Rules.RuleResult.Outcome"/> of <see cref="Rules.RuleOutcome.Passed"/>.
        /// If it returns false then at least one of the rule results returns a different (non-passing) rule outcome.
        /// </para>
        /// </remarks>
        bool Passed { get; }

        /// <summary>
        /// Gets a collection of the results of individual validation rules.
        /// </summary>
        IReadOnlyCollection<ValidationRuleResult> RuleResults { get; }
    }
}