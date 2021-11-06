using System;
using System.Collections.Generic;
using System.Linq;
using CSF.Validation.Rules;

namespace CSF.Validation
{
    /// <summary>
    /// A model for the results of a validation process.
    /// </summary>
    public class ValidationResult
    {
        /// <summary>
        /// Gets a simple value which indicates whether or not this result represents passing validation
        /// without any errors.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If this property returns <see langword="true" /> then every one of the <see cref="RuleResults"/> has
        /// an <see cref="RuleResult.Outcome"/> of <see cref="RuleOutcome.Passed"/>.
        /// If it returns false then at least one of the rule results returns a different (non-passing) rule outcome.
        /// </para>
        /// </remarks>
        public bool Passed { get; }

        /// <summary>
        /// Gets a collection of the results of individual validation rules.
        /// </summary>
        public IReadOnlyCollection<ValidationRuleResult> RuleResults { get; }

        /// <summary>
        /// Initialises a new instance of <see cref="ValidationResult"/>.
        /// </summary>
        /// <param name="ruleResults">The rule results.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="ruleResults"/> is <see langword="null" />.</exception>
        public ValidationResult(IEnumerable<ValidationRuleResult> ruleResults)
        {
            if (ruleResults is null)
                throw new ArgumentNullException(nameof(ruleResults));

            RuleResults = ruleResults.ToList();
            Passed = RuleResults.All(r => r.Outcome == RuleOutcome.Passed);
        }
    }
}