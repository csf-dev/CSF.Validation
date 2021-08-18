using System;
using System.Collections.Generic;

namespace CSF.Validation.ManifestModel
{
    /// <summary>
    /// A simple model to describe a single validation rule.
    /// The models in this namespace provide a serialization-friendly mechanism by which to describe
    /// a validation manifest.
    /// </summary>
    public class Rule
    {
        IDictionary<string, object> rulePropertyValues = new Dictionary<string, object>();
        ICollection<RelativeIdentifier> dependencies = new List<RelativeIdentifier>();

        /// <summary>
        /// Gets or sets the type name of the validation rule logic class.
        /// </summary>
        public string RuleTypeName { get; set; }

        /// <summary>
        /// Gets or sets an optional name for the rule, which uniquely identifies it
        /// from other rules of the same type upon the same validated value.
        /// </summary>
        public string RuleName { get; set; }

        /// <summary>
        /// Gets or sets a dictionary which indicates property values to be set upon
        /// the rule instance after it is constructed.  This allows simple configuration/parameterisation
        /// of rule types.
        /// </summary>
        public IDictionary<string, object> RulePropertyValues
        {
            get => rulePropertyValues;
            set => rulePropertyValues = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets or sets a collection of identifiers of dependencies for the current rule.
        /// </summary>
        public ICollection<RelativeIdentifier> Dependencies
        {
            get => dependencies;
            set => dependencies = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}