using System;
using System.Collections.Generic;
using System.Linq;
using CSF.Validation.Manifest;
using CSF.Validation.Rules;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// A model for a rule which may be executed upon a validated value.
    /// </summary>
    public class ExecutableRule
    {
        /// <summary>
        /// Gets the value to be validated by the current rule.
        /// </summary>
        public ValidatedValue ValidatedValue { get; set; }

        /// <summary>
        /// Gets the manifest rule which the current instance corresponds to.
        /// </summary>
        public ManifestRule ManifestRule { get; set; }

        /// <summary>
        /// Gets the executable rule logic.
        /// </summary>
        public IValidationLogic RuleLogic { get; set; }

        /// <summary>
        /// Gets or sets the rule's result.
        /// </summary>
        public RuleResult Result { get; set; }

        /// <summary>
        /// Gets a string which represents the current executable rule.
        /// </summary>
        /// <returns>A human-readable string.</returns>
        public override string ToString()
        {
            var properties = new Dictionary<string, string>
                {
                    { "Type", ManifestRule?.Identifier?.RuleType.FullName },
                    { "Name", ManifestRule?.Identifier?.RuleName },
                    { "Validated type", ValidatedValue?.ActualValue?.GetType().FullName ?? "null" },
                    { "Validated identity", ValidatedValue?.ValueIdentity?.ToString() ?? null },
                }
                .Where(x => x.Value != null)
                .ToDictionary(x => x.Key, x => x.Value);

            var longestKey = GetLongestKey(properties);
            return String.Join(Environment.NewLine, properties.Select(prop => String.Concat(FormatKey(prop.Key, longestKey), prop.Value)));
        }

        static string FormatKey(string key, int width) => key.PadRight(width) + " = ";

        static int GetLongestKey(Dictionary<string, string> properties)
            => (from prop in properties orderby prop.Key.Length descending select prop.Key).First().Length;
    }
}