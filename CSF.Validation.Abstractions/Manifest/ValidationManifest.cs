using System;

namespace CSF.Validation.Manifest
{
    /// <summary>
    /// The root object of a validation manifest.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This type serves as the root a model that describes how a validator should operate, such as which
    /// rules it should execute and how they should be configured.
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
    /// <seealso cref="ManifestItem"/>
    public class ValidationManifest : IManifestNode
    {
        /// <summary>
        /// Gets or sets the type of object which the validator should validate.
        /// </summary>
        public Type ValidatedType { get; set; }

        /// <summary>
        /// Gets or sets the root value for the current manifest.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The root item (referenced by this property) must be of <see cref="ManifestItem.ItemType"/>
        /// <see cref="ManifestItemTypes.Value"/> and nothing else.
        /// </para>
        /// </remarks>
        public ManifestItem RootValue { get; set; }
    }
}