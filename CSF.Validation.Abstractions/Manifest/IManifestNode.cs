using System;

namespace CSF.Validation.Manifest
{
    /// <summary>
    /// A node in the validation manifest.  Typically either a <see cref="ValidationManifest"/>
    /// itself or a <see cref="ManifestItem"/>.
    /// </summary>
    public interface IManifestNode
    {
        /// <summary>
        /// Gets the type of the object which the current manifest node describes.
        /// </summary>
        Type ValidatedType { get; }
    }
}