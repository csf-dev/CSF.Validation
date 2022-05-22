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
    public class ValidationResultWithMessages : IHasValidationRuleResults
    {
        /// <inheritdoc/>
        public bool Passed { get; }

        /// <summary>
        /// Gets a collection of the results of individual validation rules.
        /// </summary>
        public IReadOnlyCollection<ValidationRuleResultWithMessage> RuleResults { get; }

        IReadOnlyCollection<ValidationRuleResult> IHasValidationRuleResults.RuleResults => RuleResults;

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