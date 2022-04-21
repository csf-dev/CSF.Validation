namespace CSF.Validation.ManifestModel
{
    /// <summary>
    /// A model which represents an item of a collection which is to be validated individually.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Instances of that type are similar to <see cref="Value"/> except that they describe items
    /// of a collection, indicated by <see cref="ValueBase.CollectionItemValue"/>.
    /// </para>
    /// <para>
    /// This type roughly corresponds to <see cref="CSF.Validation.Manifest.ManifestCollectionItem"/>.
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
    /// <seealso cref="ValueBase"/>
    public class CollectionItemValue : ValueBase {}
}