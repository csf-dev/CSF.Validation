namespace CSF.Validation.Manifest
{
    /// <summary>
    /// An object which gets an <see cref="IValidator"/> from a <see cref="ValidationManifest"/>.
    /// </summary>
    public interface IGetsValidatorFromManifest
    {
        /// <summary>
        /// Gets a validator from a validation manifest.
        /// </summary>
        /// <param name="manifest">The validation manifest.</param>
        /// <returns>A validator.</returns>
        IValidator GetValidator(ValidationManifest manifest);
    }
}