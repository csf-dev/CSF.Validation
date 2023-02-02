using CSF.Validation.Manifest;
using CSF.Validation.Rules;
using CSF.Validation.ValidatorBuilding;

namespace CSF.Validation.ValidatorValidation
{
    /// <summary>
    /// A validator-builder for instances of <see cref="ValidationManifest"/>.
    /// </summary>
    public class ValidationManifestValidatorBuilder : IBuildsValidator<ValidationManifest>
    {
        internal const string RootValueOfManifestMustNotBeRecursive = nameof(RootValueOfManifestMustNotBeRecursive);

        /// <inheritdoc/>
        public void ConfigureValidator(IConfiguresValidator<ValidationManifest> config)
        {
            config.ForMember(x => x.ValidatedType, m => m.AddRule<NotNull>());

            config.ForMember(x => x.RootValue, m =>
            {
                m.AddRules<ManifestValueValidatorBuilder>();
                m.AddRule<ParentMustDeriveFromValidationManifest>();
                m.AddRule<DoesNotDeriveFrom<RecursiveManifestValue>>(r => r.Name = RootValueOfManifestMustNotBeRecursive);
                m.AddRuleWithParent<RootValueMustBeForSameTypeAsManifest>();
            });

        }
    }
}