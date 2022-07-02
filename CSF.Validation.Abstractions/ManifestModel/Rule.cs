using System;
using System.Collections.Generic;

namespace CSF.Validation.ManifestModel
{
    /// <summary>
    /// A manifest model class that represents a validation rule for a <see cref="Value"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This type roughly corresponds to <see cref="CSF.Validation.Manifest.ManifestRule"/>.
    /// The manifest model classes are simplified when compared with the validation manifest
    /// and offer only a subset of functionality.  Importantly though, manifest model classes
    /// such as this are suitable for easy serialization to/from various data formats, such as
    /// JSON or relational database tables.
    /// </para>
    /// <para>
    /// For more information about when and how to use the manifest model, see the article
    /// <xref href="ManifestModelIndexPage?text=Using+the+Manifest+Model"/>
    /// </para>
    /// </remarks>
    /// <seealso cref="Value"/>
    /// <seealso cref="RelativeIdentifier"/>
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