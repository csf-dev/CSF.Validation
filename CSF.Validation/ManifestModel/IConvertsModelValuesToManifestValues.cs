namespace CSF.Validation.ManifestModel
{
    /// <summary>
    /// An object which can convert a <see cref="ModelToManifestConversionContext"/> (containing a <see cref="Value"/>)
    /// into a hierarchy of <see cref="CSF.Validation.Manifest.ManifestValue"/>, contained within a <see cref="ModelToManifestValueConversionResult"/>.
    /// </summary>
    public interface IConvertsModelValuesToManifestValues
    {
        /// <summary>
        /// Converts all of the hierarchy of <see cref="Value"/> instances within the specified context into
        /// an equivalent hierarchy of <see cref="CSF.Validation.Manifest.ManifestValue"/>, which are returned
        /// as a result object.
        /// </summary>
        /// <param name="context">A conversion context.</param>
        /// <returns>A result object containing the converted manifest values.</returns>
        /// <exception cref="ValidatorBuildingException">If the input value(s) are not valid for creating a validation manifest.</exception>
        /// <exception cref="System.ArgumentNullException">If the <paramref name="context"/> is <see langword="null"/>.</exception>
        ModelToManifestValueConversionResult ConvertAllValues(ModelToManifestConversionContext context);
    }
}