using CSF.Validation.Manifest;

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
    /// <xref href="ManifestModelIndexPage?text=Using+the+Manifest+Model"/>
    /// </para>
    /// </remarks>
    /// <seealso cref="Rule"/>
    /// <seealso cref="RelativeIdentifier"/>
    public class Value : ValueBase
    {
        /// <summary>
        /// Gets or sets an optional value which indicates the desired behaviour should the member accessor
        /// raise an exception.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This option will override the behaviour specified at <see cref="ValidationOptions.AccessorExceptionBehaviour"/>
        /// for the current value, if this property is set to any non-<see langword="null" /> value.
        /// </para>
        /// <para>
        /// If this property is set to <see langword="null" /> then the behaviour at <see cref="ValidationOptions.AccessorExceptionBehaviour"/>
        /// will be used.
        /// </para>
        /// </remarks>
        /// <seealso cref="ValidationOptions.AccessorExceptionBehaviour"/>
        public ValueAccessExceptionBehaviour? AccessorExceptionBehaviour { get; set; }
    }
}