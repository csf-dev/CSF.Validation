using CSF.Validation.Manifest;

namespace CSF.Validation.ManifestModel
{
    /// <summary>
    /// An object which converts a <see cref="ModelToManifestConversionContext"/> into a <see cref="ManifestItem"/>.
    /// </summary>
    public interface IGetsManifestItemFromModelToManifestConversionContext
    {
        /// <summary>
        /// Gets a manifest item from the specified conversion context.
        /// </summary>
        /// <param name="context">A conversion context.</param>
        /// <returns>A manifest item.</returns>
        ManifestItem GetManifestItem(ModelToManifestConversionContext context);
    }
}