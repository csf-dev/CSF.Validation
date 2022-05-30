namespace CSF.Validation
{
    /// <summary>
    /// An object which can get the resolved/final/effective validation options, combining all of the ways
    /// in which they might be specified.
    /// </summary>
    public interface IGetsResolvedValidationOptions
    {
        /// <summary>
        /// Gets the resolved validation options.
        /// </summary>
        /// <param name="specifiedOptions">Options which have been specified by a developer via the API; may be <see langword="null" />.</param>
        /// <returns>The resolved (or effective) validation options.</returns>
        ResolvedValidationOptions GetResolvedValidationOptions(ValidationOptions specifiedOptions);
    }
}