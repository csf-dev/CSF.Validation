namespace CSF.Validation.Manifest
{
    /// <summary>
    /// A model which represents an item of a collection which is to be validated individually.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Instances of that type are similar to <see cref="ManifestValue"/> except that they describe items
    /// of a collection, indicated by <see cref="ManifestItem.CollectionItemValue"/>.
    /// </para>
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
    /// <seealso cref="ManifestItem"/>
    /// <seealso cref="Manifest.ManifestValue"/>
    /// <seealso cref="ManifestPolymorphicType"/>
    /// <seealso cref="RecursiveManifestValue"/>
    public class ManifestCollectionItem : ManifestItem {}
}