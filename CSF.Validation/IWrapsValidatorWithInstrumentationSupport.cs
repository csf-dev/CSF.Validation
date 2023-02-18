namespace CSF.Validation
{
    /// <summary>
    /// An object which wraps a validator instance with appropriate instrumentation behaviour.
    /// </summary>
    /// <seealso cref="ResolvedValidationOptions.InstrumentRuleExecution"/>
    public interface IWrapsValidatorWithInstrumentationSupport
    {
        /// <summary>
        /// Wraps a non-generic validator with the appropriate behaviour.
        /// </summary>
        /// <param name="validator">The validator instance to wrap.</param>
        /// <returns>A validator, with added instrumentation behaviour.</returns>
        IValidator WrapValidator(IValidator validator);

        /// <summary>
        /// Wraps a generic validator with the appropriate behaviour.
        /// </summary>
        /// <typeparam name="TValidated">The validated type.</typeparam>
        /// <param name="validator">The validator instance to wrap.</param>
        /// <returns>A validator, with added instrumentation behaviour.</returns>
        IValidator<TValidated> WrapValidator<TValidated>(IValidator<TValidated> validator);
    }
}