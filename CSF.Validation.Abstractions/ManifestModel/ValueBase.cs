using System;
using System.Collections.Generic;

namespace CSF.Validation.ManifestModel
{
    /// <summary>
    /// Abstract base class used for values which are validated.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This type roughly corresponds to <see cref="CSF.Validation.Manifest.IManifestItem"/>.
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
    /// <seealso cref="Rule"/>
    /// <seealso cref="RelativeIdentifier"/>
    /// <seealso cref="Value"/>
    /// <seealso cref="CollectionItemValue"/>
    public abstract class ValueBase
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
        /// Gets or sets an optional value object which indicates how items within a collection are to be validated.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If the value representd by the current instance is a collection/enumerable of items then these items may
        /// be validated individually.  In this scenario, the type of object represented by the current value must be a
        /// type that implements <see cref="System.Collections.Generic.IEnumerable{T}"/> for at least one generic type.
        /// </para>
        /// <para>
        /// If this property has a non-null value, then the <see cref="CollectionItemValue"/> will be used to validate
        /// each item within that collection.
        /// </para>
        /// <para>
        /// If the current value does not represent a collection of items to be validated individually then this
        /// property must by <see langword="null" />.
        /// </para>
        /// </remarks>
        public CollectionItemValue CollectionItemValue { get; set; }

        /// <summary>
        /// Gets or sets a value which indicates that the current value should represent recursive validation as an ancestor value.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If <see langword="null" /> then this value is treated as a normal manifest value.  If not null then this value
        /// will be treated as a recursive (or re-entrant) validation manifest value.  It will be converted as a
        /// <see cref="CSF.Validation.Manifest.RecursiveManifestValue"/> rather than a normal value.
        /// </para>
        /// <para>
        /// The numeric value of this property (which must be a positive integer if non-null) indicates which level of ancestor
        /// <see cref="Value"/> is used to provide the recursive validation.  For example a value of 1 indicates that the immediate
        /// parent value should be used as the wrapped value for the recursive manifest value.  A value of 2 would indicate the
        /// grandparent value.
        /// </para>
        /// </remarks>
        public int? ValidateRecursivelyAsAncestor { get; set; }
    }
}