using System;
using System.Collections.Generic;
using System.Linq;
using CSF.Validation.Rules;

namespace CSF.Validation
{
    /// <summary>
    /// A model for the results of a validation process which may also
    /// include human-readable feedback messages for any failed rules.
    /// </summary>
    public class ValidationResultWithMessages
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
        public IReadOnlyCollection<ValidationRuleResultWithMessage> RuleResults { get; }

        /// <summary>
        /// Initialises a new instance of <see cref="ValidationResultWithMessages"/>.
        /// </summary>
        /// <param name="ruleResults">The rule results.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="ruleResults"/> is <see langword="null" />.</exception>
        public ValidationResultWithMessages(IEnumerable<ValidationRuleResultWithMessage> ruleResults)
        {
            if (ruleResults is null)
                throw new ArgumentNullException(nameof(ruleResults));

            RuleResults = ruleResults.ToList();
            Passed = RuleResults.All(r => r.Outcome == RuleOutcome.Passed);
        }
    }
}