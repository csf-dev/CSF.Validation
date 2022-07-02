using System.Collections.Generic;
using CSF.Validation.Manifest;

namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// An object which can get a <see cref="ManifestValue"/> which may contain a
    /// collection of rules and/or descendent values.
    /// </summary>
    public interface IGetsManifestValue
    {
        /// <summary>
        /// Gets a manifest value from the current instance.
        /// </summary>
        /// <returns>A manifest value.</returns>
        IManifestItem GetManifestValue();
    }
}