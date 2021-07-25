using CSF.Validation.Manifest;

namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// An object which can get a manifest rule.
    /// </summary>
    public interface IGetsManifestRule
    {
        /// <summary>
        /// Gets a manifest rule from the state of the current instance.
        /// </summary>
        /// <returns>A manifest rule.</returns>
        ManifestRule GetManifestRule();
    }
}