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
        /// A <see cref="CollectionItemValue"/> which is to be converted to a <see cref="ManifestItem"/>.
        /// </summary>
        CollectionItem,

        /// <summary>
        /// A <see cref="PolymorphicValue"/> which is to be converted to a <see cref="ManifestItem"/>.
        /// </summary>
        PolymorphicType,

        /// <summary>
        /// A <see cref="Value"/> or <see cref="CollectionItemValue"/> which is to be converted to a <see cref="ManifestItem"/>.
        /// </summary>
        RecursiveManifestValue,
    }
}