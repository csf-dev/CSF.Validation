using CSF.Validation.Manifest;
using CSF.Validation.Rules;
using CSF.Validation.ValidatorBuilding;

namespace CSF.Validation.ValidatorValidation
{
    /// <summary>
    /// A builder which configures rules for a validator which validates instances of <see cref="ManifestRule"/>.
    /// </summary>
    public class ManifestRuleValidatorBuilder : IBuildsValidator<ManifestRule>
    {
        /// <inheritdoc/>
        public void ConfigureValidator(IConfiguresValidator<ManifestRule> config)
        {
            config.AddRule<RuleMustImplementCompatibleValidationLogic>();
        }
    }
}