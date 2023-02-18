using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public TimeSpan? ValidationTime { get; }

        /// <inheritdoc/>
        public IQueryableValidationResult<T> AsResultFor<T>() => (IQueryableValidationResult<T>) this;

        /// <inheritdoc/>
        public abstract SerializableValidationResult ToSerializableResult();

        ManifestItem IQueryableValidationResult.ManifestValue => Manifest.RootValue;

        IEnumerator<ValidationRuleResult> IEnumerable<ValidationRuleResult>.GetEnumerator() => RuleResults.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => RuleResults.GetEnumerator();

        /// <summary>
        /// Gets a string representation of the current instance.
        /// </summary>
        /// <returns>A string representation.</returns>
        public override string ToString() => ToString(null);

        /// <summary>
        /// Gets a string representation of the current instance, possibly skipping some rule results with irrelevant outcomes.
        /// </summary>
        /// <param name="omittedOutcomes">An optional collection of rule outcomes to omit when displaying rule results.</param>
        /// <returns>A string representation.</returns>
        public string ToString(IEnumerable<RuleOutcome> omittedOutcomes)
        {
            omittedOutcomes = omittedOutcomes ?? Enumerable.Empty<RuleOutcome>();

            var builder = new StringBuilder();
            builder.AppendLine($"[{nameof(ValidationResult)}<{Manifest.ValidatedType}>: {nameof(ValidationResult.Passed)} = {Passed}]");
            if(omittedOutcomes.Any())
                builder.AppendLine($"Rule results, omitting those with outcomes {{ {String.Join(", ", omittedOutcomes)} }}:");
            else
                builder.AppendLine("Rule results:");

            foreach(var ruleResult in RuleResults)
            {
                if(omittedOutcomes.Contains(ruleResult.Outcome)) continue;
                builder.AppendLine($"    {ruleResult}");
            }

            return builder.ToString();
        }

        /// <summary>
        /// Initialises a new instance of <see cref="ValidationResult"/>.
        /// </summary>
        /// <param name="ruleResults">The rule results.</param>
        /// <param name="manifest">The validation manifest</param>
        /// <param name="validationTime">The time it has taken to perform validation.</param>
        /// <exception cref="ArgumentNullException">If either of the first two parameters is <see langword="null" />.</exception>
        protected ValidationResult(IEnumerable<ValidationRuleResult> ruleResults, ValidationManifest manifest, TimeSpan? validationTime = default)
        {
            RuleResults = ruleResults?.ToList() ?? throw new ArgumentNullException(nameof(ruleResults));
            Passed = RuleResults.All(r => r.Outcome == RuleOutcome.Passed);
            Manifest = manifest ?? throw new ArgumentNullException(nameof(manifest));
            ValidationTime = validationTime;
        }
    }
}