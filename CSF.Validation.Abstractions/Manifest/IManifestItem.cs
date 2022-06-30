using System;
using System.Collections.Generic;

namespace CSF.Validation.Manifest
{
    /// <summary>
    /// An object which behaves like a value within a validation manifest.
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
    /// <seealso cref="ValidationManifest"/>
    /// <seealso cref="ManifestRuleIdentifier"/>
    /// <seealso cref="ManifestValue"/>
    /// <seealso cref="ManifestCollectionItem"/>
    public interface IManifestItem
    {
        /// <summary>
        /// Gets  the type of the object which the current manifest value describes.
        /// </summary>
        Type ValidatedType { get; }

        /// <summary>
        /// Gets an optional parent manifest value.
        /// Where this is <see langword="null"/> that indicates that this model is the root of the validation hierarchy.
        /// If it is non-<see langword="null"/> then it is a descendent of the root of the hierarchy.
        /// </summary>
        IManifestItem Parent { get; }

        /// <summary>
        /// Gets a function which retrieves a unique identity of the object being
        /// validated, given a reference to that object being validated.
        /// </summary>
        Func<object, object> IdentityAccessor { get; }

        /// <summary>
        /// Where the current value represents a member access invocation (such as
        /// a property getter), this property gets the name of that member.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Note that in some derived types, such as <see cref="ManifestCollectionItem"/>, this property will always
        /// return <see langword="null" /> and may not be set to any other value.
        /// </para>
        /// </remarks>
        string MemberName { get; }

        /// <summary>
        /// Gets an optional value object which indicates how items within a collection are to be validated.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If the value representd by the current instance is a collection/enumerable of items then these items may
        /// be validated individually.  In this scenario, the <see cref="ValidatedType"/> must be a
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
        ManifestCollectionItem CollectionItemValue { get; }

        /// <summary>
        /// Gets a collection of the immediate descendents of the current manifest value.
        /// </summary>
        ICollection<ManifestValue> Children { get; }

        /// <summary>
        /// Gets a collection of the rules associated with the current value.
        /// </summary>
        ICollection<ManifestRule> Rules { get; }
    }
}