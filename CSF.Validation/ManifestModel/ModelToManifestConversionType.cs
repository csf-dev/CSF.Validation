using CSF.Validation.Manifest;

namespace CSF.Validation.ManifestModel
{
    /// <summary>
    /// Represents the types of manifest model value which may be converted to manifest values.
    /// </summary>
    public enum ModelToManifestConversionType
    {
        /// <summary>
        /// A <see cref="Value"/> which is to be converted to a <see cref="ManifestItem"/>.
        /// </summary>
        Manifest,

        /// <summary>
        /// A <see cref="Value"/> which is to be converted to a <see cref="ManifestItem"/> of type <see cref="ManifestItemTypes.CollectionItem"/>.
        /// </summary>
        CollectionItem,

        /// <summary>
        /// A <see cref="Value"/> which is to be converted to a <see cref="ManifestItem"/> of type <see cref="ManifestItemTypes.PolymorphicType"/>.
        /// </summary>
        PolymorphicType,

        /// <summary>
        /// A <see cref="Value"/> which is to be converted to a <see cref="ManifestItem"/> of either type <see cref="ManifestItemTypes.RecursiveValue"/>
        /// or <see cref="ManifestItemTypes.RecursiveCollectionItem"/>.
        /// </summary>
        RecursiveManifestValue,
    }
}