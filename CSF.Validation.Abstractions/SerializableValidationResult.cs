using System;
using System.Linq;
using System.Runtime.Serialization;
using CSF.Validation.Rules;

namespace CSF.Validation
{
    /// <summary>
    /// A model for a validation result which is easy to serialize/deserialize to/from data formats.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Whilst this model lacks some of the functionality of a full validation result, it is easy to consume across API
    /// boundaries or mediums like JSON, XML or tables of a relational database.
    /// </para>
    /// <para>
    /// This class conceptually corresponds to <see cref="ValidationResult"/>.
    /// </para>
    /// </remarks>
    [Serializable]
    public sealed class SerializableValidationResult : ISerializable
    {
        internal const string PropertySuffix = "Property";

        SerializableValidationRuleResult[] ruleResults = Array.Empty<SerializableValidationRuleResult>();

        /// <summary>
        /// Gets or sets an array of the individual rule results which make up the current validation result.
        /// </summary>
        public SerializableValidationRuleResult[] RuleResults
        {
            get => ruleResults;
            set => ruleResults = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets a value which indicates whether or not the current instance represents a passing validation result.
        /// </summary>
        public bool IsPass() => RuleResults.All(x => x.Outcome == RuleOutcome.Passed);

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue($"{nameof(RuleResults)}{PropertySuffix}", ruleResults, typeof(SerializableValidationRuleResult[]));
        }

        /// <summary>
        /// Initialises a new instance of <see cref="SerializableValidationResult"/>.
        /// </summary>
        public SerializableValidationResult() { }

        SerializableValidationResult(SerializationInfo info, StreamingContext context)
        {
            ruleResults = (SerializableValidationRuleResult[]) info
                .GetValue($"{nameof(RuleResults)}{PropertySuffix}", typeof(SerializableValidationRuleResult[]));
        }
    }
}