namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// An object which creates instances of <see cref="ValidatorBuilder{TValidated}"/>.
    /// </summary>
    public interface IGetsValidatorBuilder
    {
        /// <summary>
        /// Gets the validator builder object, optionally for a specified validator builder context.
        /// </summary>
        /// <typeparam name="TValidated">The type of object to be validated.</typeparam>
        /// <param name="context">An optional validator builder context; if <see langword="null"/> then a new/empty context will be created.</param>
        /// <returns>A validator builder.</returns>
        ValidatorBuilder<TValidated> GetValidatorBuilder<TValidated>(ValidatorBuilderContext context = default);
    }
}