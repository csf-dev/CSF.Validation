namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// An object which can get a <see cref="ValidatedValue"/> from a <see cref="ValidatedValueBasis"/>.
    /// </summary>
    public interface IGetsValidatedValueFromBasis
    {
        /// <summary>
        /// Gets the validated value.
        /// </summary>
        /// <param name="basis">A validated-value basis.</param>
        /// <returns>A validated value.</returns>
        ValidatedValue GetValidatedValue(ValidatedValueBasis basis);
    }
}