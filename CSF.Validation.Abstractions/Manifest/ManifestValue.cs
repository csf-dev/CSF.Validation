using System;

namespace CSF.Validation.Manifest
{
    /// <summary>
    /// A model which represents a value which is validated within a validation hierarchy.
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
    /// <seealso cref="ManifestValueBase"/>
    /// <seealso cref="ManifestCollectionItem"/>
    public class ManifestValue : ManifestValueBase
    {
        /// <summary>
        /// Gets or sets a function which gets (from the object represented by the <see cref="ManifestValueBase.Parent"/>)
        /// the value for the current instance.
        /// </summary>
        public Func<object, object> AccessorFromParent { get; set; }

        /// <summary>
        /// Indicates that the validator should ignore any exceptions encountered whilst getting the value from
        /// the <see cref="AccessorFromParent"/>.
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

        /// <summary>
        /// Gets or sets an optional value object which indicates how items within a collection are to be validated.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If the value representd by the current instance is a collection/enumerable of items then these items may
        /// be validated individually.  In this scenario, the <see cref="ManifestValueBase.ValidatedType"/> must be a
        /// type that implements <see cref="System.Collections.Generic.IEnumerable{T}"/> for at least one generic type.
        /// </para>
        /// <para>
        /// If this property has a non-null value, then the <see cref="ManifestCollectionItem"/> will be used to validate
        /// each item within that collection.
        /// </para>
        /// <para>
        /// If the current manifest value does not represent a collection of items to be validated individually then this
        /// property must by <see langword="null" />.
        /// </para>
        /// </remarks>
        public ManifestCollectionItem CollectionItemValue { get; set; }
        
        
    }
}