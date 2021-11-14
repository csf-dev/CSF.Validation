using CSF.Validation.Manifest;

namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// An object which can get a <see cref="ValidationManifest"/>.
    /// </summary>
    public interface IGetsValidationManifest
    {
        /// <summary>
        /// Gets a manifest from the current instance.
        /// </summary>
        /// <returns>A validation manifest.</returns>
        ValidationManifest GetManifest();
    }
}