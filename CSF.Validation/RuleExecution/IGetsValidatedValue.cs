using CSF.Validation.Manifest;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// An object which gets a <see cref="ValidatedValue"/> (including a hierarchy of descendent values)
    /// from a <see cref="ManifestValue"/> and the object to be validated.
    /// </summary>
    public interface IGetsValidatedValue
    {
        /// <summary>
        /// Gets the validated value from the specified manifest value and object to be validated.
        /// </summary>
        /// <param name="manifestValue">The manifest value.</param>
        /// <param name="objectToBeValidated">The object to be validated.</param>
        /// <param name="options">The validation options.</param>
        /// <returns>A validated value, including a hierarchy of descendent values and
        /// the rules which may be executed upon those values.</returns>
        ValidatedValue GetValidatedValue(ManifestValue manifestValue, object objectToBeValidated, ResolvedValidationOptions options);
    }
}