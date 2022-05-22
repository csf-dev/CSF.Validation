namespace CSF.Validation
{
    /// <summary>
    /// An object which wraps a validator instance with the capability to show human-readable feedback
    /// messages for failde validation rules.
    /// </summary>
    public interface IWrapsValidatorWithMessageSupport
    {
        /// <summary>
        /// Wraps a non-generic validator with message-enriching behaviour.
        /// </summary>
        /// <param name="validator">The validator instance to wrap.</param>
        /// <returns>The validator, with added message support.</returns>
        IValidatorWithMessages GetValidatorWithMessageSupport(IValidator validator);

        /// <summary>
        /// Wraps a generic validator with message-enriching behaviour.
        /// </summary>
        /// <typeparam name="TValidated">The validated type.</typeparam>
        /// <param name="validator">The validator instance to wrap.</param>
        /// <returns>The validator, with added message support.</returns>
        IValidatorWithMessages<TValidated> GetValidatorWithMessageSupport<TValidated>(IValidator<TValidated> validator);
    }
}