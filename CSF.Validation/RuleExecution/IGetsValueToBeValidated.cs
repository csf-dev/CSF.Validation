using CSF.Validation.Manifest;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// An object which gets the value to be validated: the "actual" value.
    /// </summary>
    public interface IGetsValueToBeValidated
    {
        /// <summary>
        /// Attempts to get the value to be validatded and returns a value indicating whether or not this was successful.
        /// </summary>
        /// <param name="manifestValue">The manifest value describing the value.</param>
        /// <param name="parentValue">The previous/parent value, from which the validated value should be derived.</param>
        /// <param name="validationOptions">Validation options.</param>
        /// <param name="valueToBeValidated">If this method returns <see langword="true" /> then this parameter
        /// exposes the validated value.  This parameter must be ignored if the method returns <see langword="false" />.</param>
        /// <returns><see langword="true" /> if getting the validated value is a success; <see langword="false" /> otherwise.</returns>
        bool TryGetValueToBeValidated(ManifestValue manifestValue,
                                      object parentValue,
                                      ValidationOptions validationOptions,
                                      out object valueToBeValidated);
    }
}