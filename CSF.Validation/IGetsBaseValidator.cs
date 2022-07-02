using CSF.Validation.Manifest;

namespace CSF.Validation
{
    /// <summary>
    /// An object which can get an instance of <see cref="Validator{TValidated}"/>,
    /// the basic/fundamental part of the validator.
    /// </summary>
    public interface IGetsBaseValidator
    {
        /// <summary>
        /// Gets a strongly-typed generic validator, from a validator builder instance.
        /// </summary>
        /// <typeparam name="TValidated">The validated type.</typeparam>
        /// <param name="builder">The validator builder.</param>
        /// <returns>The validator.</returns>
        IValidator<TValidated> GetValidator<TValidated>(IBuildsValidator<TValidated> builder);

        /// <summary>
        /// Gets a validator from a validation manifest.
        /// </summary>
        /// <param name="manifest">The validation manifest.</param>
        /// <returns>The validator.</returns>
        IValidator GetValidator(ValidationManifest manifest);
    }
}