namespace CSF.Validation.Manifest
{
    /// <summary>
    /// A service which gets a <see cref="ValidationManifest"/> from an object that implements <see cref="IBuildsValidator{TValidated}"/>.
    /// </summary>
    public interface IGetsManifestFromBuilder
    {
        /// <summary>
        /// Gets a validation manifest from the validator-builder.
        /// </summary>
        /// <typeparam name="TValidated">The validated object type.</typeparam>
        /// <param name="builder">The validation builder instance.</param>
        /// <returns>A validation manifest, created using the validation-builder.</returns>
        /// <exception cref="System.ArgumentNullException">If <paramref name="builder"/> is <see langword="null"/>.</exception>
        ValidationManifest GetManifest<TValidated>(IBuildsValidator<TValidated> builder);
    }
}