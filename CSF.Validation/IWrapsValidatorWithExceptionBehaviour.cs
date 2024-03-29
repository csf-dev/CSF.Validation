namespace CSF.Validation
{
    /// <summary>
    /// An object which wraps a validator instance with appropriate exception-throwing behaviour.
    /// </summary>
    /// <seealso cref="ResolvedValidationOptions.RuleThrowingBehaviour"/>
    /// <seealso cref="RuleThrowingBehaviour"/>
    public interface IWrapsValidatorWithExceptionBehaviour
    {
        /// <summary>
        /// Wraps a non-generic validator with the appropriate behaviour.
        /// </summary>
        /// <param name="validator">The validator instance to wrap.</param>
        /// <returns>A validator, with added exception-throwing behaviour.</returns>
        IValidator WrapValidator(IValidator validator);

        /// <summary>
        /// Wraps a generic validator with the appropriate behaviour.
        /// </summary>
        /// <typeparam name="TValidated">The validated type.</typeparam>
        /// <param name="validator">The validator instance to wrap.</param>
        /// <returns>A validator, with added exception-throwing behaviour.</returns>
        IValidator<TValidated> WrapValidator<TValidated>(IValidator<TValidated> validator);
    }
}