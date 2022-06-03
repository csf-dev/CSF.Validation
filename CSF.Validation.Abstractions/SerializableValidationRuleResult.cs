using System;
using System.Runtime.Serialization;
using CSF.Validation.Rules;

namespace CSF.Validation
{
    /// <summary>
    /// A model for a validation rule result which is easy to serialize/deserialize to/from data formats.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Whilst this model lacks some of the functionality of a full validation result, it is easy to consume across API
    /// boundaries or mediums like JSON, XML or tables of a relational database.
    /// </para>
    /// <para>
    /// This class conceptually corresponds to <see cref="ValidationRuleResult"/>.
    /// </para>
    /// </remarks>
    [Serializable]
    public sealed class SerializableValidationRuleResult : ISerializable
    {
        /// <summary>
        /// Gets or sets an optional string representation of the unique identity of the object being validated.
        /// </summary>
        public string ObjectIdentityString { get; set; }

        /// <summary>
        /// Gets or sets the name of the member whose value is being validated by this rule.
        /// </summary>
        public string MemberName { get; set; }

        /// <summary>
        /// Gets or sets name of the type of the validation rule logic class.
        /// </summary>
        public string RuleTypeName { get; set; }

        /// <summary>
        /// Gets or sets an optional rule name, to uniquely identify this rule where other identifying information might be ambiguous.
        /// </summary>
        public string RuleName { get; set; }

        /// <summary>
        /// Gets or sets the name of the type of value that the rule validates.
        /// </summary>
        public string ValidatedTypeName { get; set; }

        /// <summary>
        /// Gets or sets the outcome of the validation rule.
        /// </summary>
        public RuleOutcome Outcome { get; set; }

        /// <summary>
        /// Gets or sets a human-readable feedback message to be associated with this rule result.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets a string representation of an exception which occured, leading to an error.
        /// Typically this will be <see langword="null" /> except when <see cref="Outcome"/> is set
        /// to <see cref="RuleOutcome.Errored"/>.
        /// </summary>
        public string ExceptionString { get; set; }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            var suffix = SerializableValidationResult.PropertySuffix;

            info.AddValue($"{nameof(ObjectIdentityString)}{suffix}", ObjectIdentityString);
            info.AddValue($"{nameof(MemberName)}{suffix}", MemberName);
            info.AddValue($"{nameof(RuleTypeName)}{suffix}", RuleTypeName);
            info.AddValue($"{nameof(RuleName)}{suffix}", RuleName);
            info.AddValue($"{nameof(ValidatedTypeName)}{suffix}", ValidatedTypeName);
            info.AddValue($"{nameof(Outcome)}{suffix}", Outcome, typeof(RuleOutcome));
            info.AddValue($"{nameof(Message)}{suffix}", Message);
            info.AddValue($"{nameof(ExceptionString)}{suffix}", ExceptionString);
        }

        /// <summary>
        /// Initialises a new instance of <see cref="SerializableValidationRuleResult"/>.
        /// </summary>
        public SerializableValidationRuleResult() {}

        SerializableValidationRuleResult(SerializationInfo info, StreamingContext context)
        {
            var suffix = SerializableValidationResult.PropertySuffix;

            ObjectIdentityString = info.GetString($"{nameof(ObjectIdentityString)}{suffix}");
            MemberName = info.GetString($"{nameof(MemberName)}{suffix}");
            RuleTypeName = info.GetString($"{nameof(RuleTypeName)}{suffix}");
            RuleName = info.GetString($"{nameof(RuleName)}{suffix}");
            ValidatedTypeName = info.GetString($"{nameof(ValidatedTypeName)}{suffix}");
            Outcome = (RuleOutcome) info.GetValue($"{nameof(Outcome)}{suffix}", typeof(RuleOutcome));
            Message = info.GetString($"{nameof(Message)}{suffix}");
            ExceptionString = info.GetString($"{nameof(ExceptionString)}{suffix}");
        }
    }
}