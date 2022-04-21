namespace CSF.Validation.Manifest
{
    /// <summary>
    /// A model which represents an item of a collection which is to be validated individually.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Instances of that type are similar to <see cref="ManifestValue"/> except that they describe items
    /// of a collection, indicated by <see cref="ManifestValueBase.CollectionItemValue"/>.
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
    /// <seealso cref="ValidationManifest"/>
    /// <seealso cref="ManifestRuleIdentifier"/>
    /// <seealso cref="ManifestValueBase"/>
    /// <seealso cref="ManifestValue"/>
    public class ManifestCollectionItem : ManifestValueBase
    {
        /// <summary>
        /// Where the current value represents a member access invocation (such as
        /// a property getter), this property gets or sets the name of that member.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Note that in <see cref="ManifestCollectionItem"/>, the member name is always <see langword="null" /> and
        /// may not be set to any other value.
        /// </para>
        /// </remarks>
        public override string MemberName
        {
            get => null;
            set { /* Intentional no-op */ }
        }

        /// <summary>
        /// Gets a string representation of the current instance.
        /// </summary>
        /// <returns>A string which represents the current instance.</returns>
        public override string ToString()
            => $"[{nameof(ManifestCollectionItem)}: Type = {ValidatedType.Name}]";
    }
}