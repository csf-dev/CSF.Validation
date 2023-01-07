using System;
using System.Collections.Generic;
using System.Linq;

namespace CSF.Validation.Manifest
{
    /// <summary>
    /// Base class used for values which are validated.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The validation manifest is the model by which validators are described, including how they should
    /// validate objects and values.
    /// </para>
    /// <para>
    /// The validation manifest objects are not particularly suited to serialization,
    /// as they support the use of types that cannot be easily serialized.
    /// If you are looking for a way to create/define a validator using serialized data then please read the
    /// article <xref href="ManifestModelIndexPage?text=Using+the+Manifest+Model"/>.
    /// </para>
    /// </remarks>
    /// <seealso cref="ManifestRule"/>
    /// <seealso cref="ManifestRuleIdentifier"/>
    /// <seealso cref="ValidationManifest"/>
    /// <seealso cref="ManifestValue"/>
    /// <seealso cref="ManifestCollectionItem"/>
    /// <seealso cref="ManifestPolymorphicType"/>
    /// <seealso cref="RecursiveManifestValue"/>
    public abstract class ManifestItem
    {
        ICollection<ManifestValue> children = new List<ManifestValue>();
        ICollection<ManifestRule> rules = new List<ManifestRule>();
        ICollection<ManifestPolymorphicType> polymorphicTypes = new HashSet<ManifestPolymorphicType>();

        /// <summary>
        /// Gets or sets the type of the object which the current manifest value describes.
        /// </summary>
        public virtual Type ValidatedType { get; set; }

        /// <summary>
        /// Gets or sets an optional parent manifest value.
        /// Where this is <see langword="null"/> that indicates that this model is the root of the validation hierarchy.
        /// If it is non-<see langword="null"/> then it is a descendent of the root of the hierarchy.
        /// </summary>
        public virtual ManifestItem Parent { get; set; }

        /// <summary>
        /// Gets or sets a function which retrieves a unique identity of the object being
        /// validated, given a reference to that object being validated.
        /// </summary>
        public virtual Func<object, object> IdentityAccessor { get; set; }

        /// <summary>
        /// Gets or sets an optional value object which indicates how items within a collection are to be validated.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If the value representd by the current instance is a collection/enumerable of items then these items may
        /// be validated individually.  In this scenario, the <see cref="ManifestItem.ValidatedType"/> must be a
        /// type that implements <see cref="System.Collections.Generic.IEnumerable{T}"/> for at least one generic type.
        /// </para>
        /// <para>
        /// If this property has a non-null value, then the <see cref="ManifestCollectionItem"/> will be used to validate
        /// each item within that collection.
        /// </para>
        /// <para>
        /// If the current manifest value does not represent a collection of items to be validated individually then this
        /// property must be <see langword="null" />.
        /// </para>
        /// </remarks>
        public virtual ManifestCollectionItem CollectionItemValue { get; set; }

        /// <summary>
        /// Gets or sets a collection of the immediate descendents of the current manifest value.
        /// </summary>
        public virtual ICollection<ManifestValue> Children
        {
            get => children;
            set => children = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets or sets a collection of the rules associated with the current value.
        /// </summary>
        public virtual ICollection<ManifestRule> Rules
        {
            get => rules;
            set => rules = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets or sets a mapping of the runtime types to polymorphic validation manifest definitions for those types.
        /// </summary>
        public virtual ICollection<ManifestPolymorphicType> PolymorphicTypes
        {
            get => polymorphicTypes;
            set => polymorphicTypes = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets a string representation of the current instance.
        /// </summary>
        /// <returns>A string which represents the current instance.</returns>
        public override string ToString()
        {
            var propertyStrings = GetPropertyValuesForToString()
                .Where(x => !(x.Value is null))
                .Select(x => $"{x.Key} = {x.Value}").ToList();
            
            return $"[{GetType().Name}: {String.Join(", ",  propertyStrings)}]";
        }

        /// <summary>
        /// Gets a collection of property values which will be used for the <see cref="ToString()"/> method.
        /// </summary>
        /// <returns>The property names &amp; values</returns>
        protected virtual IDictionary<string, string> GetPropertyValuesForToString()
        {
            return new Dictionary<string, string>
            {
                { nameof(Type), ValidatedType?.Name },
            };
        }
    }
}