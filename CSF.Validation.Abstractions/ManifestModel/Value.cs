using System;
using System.Collections.Generic;

namespace CSF.Validation.ManifestModel
{
    /// <summary>
    /// A manifest model class representing a value to be validated.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This type roughly corresponds to <see cref="CSF.Validation.Manifest.ManifestValue"/>.
    /// The manifest model classes are simplified when compared with the validation manifest
    /// and offer only a subset of functionality.  Importantly though, manifest model classes
    /// such as this are suitable for easy serialization to/from various data formats, such as
    /// JSON or relational database tables.
    /// </para>
    /// <para>
    /// For more information about when and how to use the manifest model, see the article
    /// @ManifestModelIndexPage
    /// </para>
    /// </remarks>
    /// <seealso cref="Rule"/>
    /// <seealso cref="RelativeIdentifier"/>
    public class Value
    {
        IDictionary<string,Value> children = new Dictionary<string,Value>();
        ICollection<Rule> rules = new List<Rule>();

        /// <summary>
        /// Gets or sets a collection of the child values from the current instance.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Each value that is directly derived from a member of the current instance
        /// is stored in this collection.  The collection key is the member name (typically
        /// a property name) by which the value is accessed.
        /// </para>
        /// <para>
        /// This property may not be set to <see langword="null" />, and will raise <see cref="ArgumentNullException"/>
        /// if an attempt is made to do so.
        /// </para>
        /// </remarks>
        /// <example>
        /// <para>
        /// If the current <see cref="Value"/> instance represents an object which has a property named <c>Age</c>
        /// which should be validated, then the Value instance for the Age property would be stored at
        /// <c>Children["Age"]</c>.
        /// </para>
        /// </example>
        public IDictionary<string,Value> Children
        {
            get => children;
            set => children = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets or sets a collection of rules for the current value.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Each rule validates the object represented by the current value instance.  To validate values
        /// that are accessed via members of that object, add new value instances to the <see cref="Children"/>
        /// property of this instance and add rules to those child value instances.
        /// </para>
        /// </remarks>
        public ICollection<Rule> Rules
        {
            get => rules;
            set => rules = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets or sets an optional member name which provides the identity value for the current object.
        /// </summary>
        public string IdentityMemberName { get; set; }
        
        /// <summary>
        /// This should be set to <see langword="true"/> if the object represented by this value is an <see cref="IEnumerable{T}"/> which
        /// should be enumerated and each of its items validated independently.
        /// This should be set to <see langword="false"/> if not.
        /// </summary>
        public bool EnumerateItems { get; set; }

        /// <summary>
        /// Indicates that the validator should ignore any exceptions encountered whilst getting the value from
        /// the member accessor for this value.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This option is irrelevant if <see cref="ValidationOptions.IgnoreValueAccessExceptions"/> is set to <see langword="true"/>,
        /// because that option ignores all value-access exceptions globally.
        /// </para>
        /// <para>
        /// If the global validation options are not configured to globally-ignore value access exceptions then this option may be
        /// used to ignore exceptions on an accessor-by-accessor basis.  This is not recommended because it can lead to the
        /// hiding of logic errors within the accessor.
        /// </para>
        /// <para>
        /// See the information about the global setting for more information about what it means to ignore exceptions for
        /// value accessors.
        /// </para>
        /// </remarks>
        /// <seealso cref="ValidationOptions.IgnoreValueAccessExceptions"/>
        public bool IgnoreAccessorExceptions { get; set; }
    }
}