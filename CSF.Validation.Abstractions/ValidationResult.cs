using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CSF.Validation.Manifest;
using CSF.Validation.Rules;

namespace CSF.Validation
{
    /// <summary>
    /// A model for the results of a validation process.
    /// </summary>
    public abstract class ValidationResult : IQueryableValidationResult
    {
        /// <inheritdoc/>
        public bool Passed { get; }

        /// <inheritdoc/>
        public IReadOnlyCollection<ValidationRuleResult> RuleResults { get; }

        /// <inheritdoc/>
        public ValidationManifest Manifest { get; }

        /// <inheritdoc/>
        public IQueryableValidationResult<T> AsResultFor<T>() => (IQueryableValidationResult<T>) this;

        /// <inheritdoc/>
        public abstract SerializableValidationResult ToSerializableResult();

        ManifestItem IQueryableValidationResult.ManifestValue => Manifest.RootValue;

        IEnumerator<ValidationRuleResult> IEnumerable<ValidationRuleResult>.GetEnumerator() => RuleResults.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => RuleResults.GetEnumerator();

        /// <summary>
        /// Initialises a new instance of <see cref="ValidationResult"/>.
        /// </summary>
        /// <param name="ruleResults">The rule results.</param>
        /// <param name="manifest">The validation manifest</param>
        /// <exception cref="ArgumentNullException">If either parameter is <see langword="null" />.</exception>
        protected ValidationResult(IEnumerable<ValidationRuleResult> ruleResults, ValidationManifest manifest)
        {
            if (ruleResults is null)
                throw new ArgumentNullException(nameof(ruleResults));

            RuleResults = ruleResults.ToList();
            Passed = RuleResults.All(r => r.Outcome == RuleOutcome.Passed);
            Manifest = manifest ?? throw new ArgumentNullException(nameof(manifest));
        }
    }
}