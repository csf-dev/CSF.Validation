using CSF.Validation.Manifest;
using CSF.Validation.Rules;
using CSF.Validation.ValidatorBuilding;

namespace CSF.Validation.ValidatorValidation
{
    /// <summary>
    /// A validator-builder for instances of <see cref="ManifestValue"/>.
    /// </summary>
    public class ManifestValueValidatorBuilder : IBuildsValidator<ManifestValue>
    {
        /// <inheritdoc/>
        public void ConfigureValidator(IConfiguresValidator<ManifestValue> config)
        {
            config.AddBaseRules<ManifestItem, ManifestItemValidatorBuilder>();

            config.ForMember(x => x.AccessorFromParent, m =>
            {
                m.AddRuleWithParent<NullIfTheParentIsAManifest>();
            });

            config.ForMember(x => x.MemberName, m =>
            {
                m.AddRuleWithParent<NullIfTheParentIsAManifest>();
                m.AddRuleWithParent<MemberMustExist>();
            });

            config.ForMember(x => x.AccessorExceptionBehaviour, m =>
            {
                m.AddRule<MustBeDefinedEnumConstant<ValueAccessExceptionBehaviour>>();
            });
        }
    }
}