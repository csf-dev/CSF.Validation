using CSF.Validation.Manifest;
using CSF.Validation.Rules;
using CSF.Validation.ValidatorBuilding;

namespace CSF.Validation.ValidatorValidation
{
    /// <summary>
    /// A validator-builder for instances of <see cref="ManifestItem"/>.
    /// </summary>
    public class ManifestItemValidatorBuilder : IBuildsValidator<ManifestItem>
    {
        /// <inheritdoc/>
        public void ConfigureValidator(IConfiguresValidator<ManifestItem> config)
        {
            config.ForMember(x => x.ValidatedType, m => m.AddRule<NotNull>());
        }
    }
}