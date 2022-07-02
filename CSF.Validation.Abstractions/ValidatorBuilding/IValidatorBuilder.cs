namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// An object which implements all of <see cref="IConfiguresValidator{TValidated}"/>, <see cref="IGetsManifestValue"/> &amp;
    /// <see cref="IGetsValidationManifest"/>.
    /// </summary>
    /// <typeparam name="TValidated">The validated object type.</typeparam>
    public interface IValidatorBuilder<TValidated> : IConfiguresValidator<TValidated>, IGetsManifestValue, IGetsValidationManifest {}
}