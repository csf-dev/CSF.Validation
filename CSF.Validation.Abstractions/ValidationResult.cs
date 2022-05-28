using System;
using System.Collections.Generic;
using System.Linq;
using CSF.Validation.Manifest;
using CSF.Validation.Rules;

namespace CSF.Validation
{
    /// <summary>
    /// A model for the results of a validation process.
    /// </summary>
    public class ValidationResult
    {
        /// <summary>
        /// Gets a value that indicates whether or not the current instance represents passing validation.
        /// </summary>
        public bool Passed { get; }

        /// <summary>
        /// Gets a collection of the results of individual validation rules, making up
        /// the current validation result.
        /// </summary>
        public IReadOnlyCollection<ValidationRuleResult> RuleResults { get; }

        /// <summary>
        /// Gets a reference to the validation manifest to which the current validation result relates.
        /// </summary>
        public ValidationManifest Manifest { get; }

        /// <summary>
        /// Initialises a new instance of <see cref="ValidationResult"/>.
        /// </summary>
        /// <param name="ruleResults">The rule results.</param>
        /// <param name="manifest">The validation manifest</param>
        /// <exception cref="ArgumentNullException">If either parameter is <see langword="null" />.</exception>
        public ValidationResult(IEnumerable<ValidationRuleResult> ruleResults, ValidationManifest manifest)
        {
            if (ruleResults is null)
                throw new ArgumentNullException(nameof(ruleResults));

            RuleResults = ruleResults.ToList();
            Passed = RuleResults.All(r => r.Outcome == RuleOutcome.Passed);
            Manifest = manifest ?? throw new ArgumentNullException(nameof(manifest));
        }
    }
}