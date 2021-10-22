using System;
using System.Collections.Generic;
using System.Linq;

namespace CSF.Validation
{
    /// <summary>
    /// A model for the results of a validation process.
    /// </summary>
    public class ValidationResult
    {
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
        }
    }
}