using CSF.Validation.Manifest;
using CSF.Validation.Rules;
using CSF.Validation.ValidatorBuilding;

namespace CSF.Validation.ValidatorValidation
{
    /// <summary>
    /// A builder which configures rules for a validator which validates a type that derives from <see cref="ManifestItem"/>.
    /// </summary>
    public class ManifestItemValidatorBuilder : IBuildsValidator<ManifestItem>
    {
        /// <inheritdoc/>
        public void ConfigureValidator(IConfiguresValidator<ManifestItem> config)
        {
            config.ForMember(x => x.ValidatedType, m =>
            {
                m.AddRule<NotNull>();
                // TODO: Add a rule to verify that if the current manifest is a polymorphic type then its validated type should derive from the parent validated type
            });

            config.ForMember(x => x.Parent, m => m.AddRule<NotNull>());

            config.ForMember(x => x.OwnCollectionItemValue, m => m.AddRules<RecursiveItemValidatorBuilder>());
            config.AddRule<CollectionItemValueMustBeCollectionItem>();
            config.AddRule<CollectionItemValueMustBeNullIfValidatedTypeIsNotEnumerable>();
            config.AddRule<CollectionItemValueMustValidateCompatibleTypeForValidatedType>(r =>
            {
                r.AddDependency(d => d.RuleType<CollectionItemValueMustBeNullIfValidatedTypeIsNotEnumerable>());
            });

            config.ForMember(x => x.Children, m => m.AddRule<ContainsNoNullItems<ManifestItem>>());
            config.ForMemberItems(x => x.OwnChildren, m => m.AddRules<RecursiveItemValidatorBuilder>());
            config.AddRule<AllChildrenMustBeValues>();

            config.ForMember(x => x.Rules, m => m.AddRule<ContainsNoNullItems<ManifestRule>>());
            config.ForMemberItems(x => x.Rules, m => m.AddRules<ManifestRuleValidatorBuilder>());

            config.ForMember(x => x.PolymorphicTypes, m => m.AddRule<ContainsNoNullItems<ManifestItem>>());
            config.ForMemberItems(x => x.OwnPolymorphicTypes, m => m.AddRules<RecursiveItemValidatorBuilder>());
            config.AddRule<AllPolymorphicTypesMustBeMarkedAsSo>();

            config.ForMember(x => x.RecursiveAncestor, m =>
            {
                m.AddRuleWithParent<NullIfNotRecursive>();
                m.AddRuleWithParent<NotNullIfRecursive>();
            });

            config.ForMember(x => x.AccessorFromParent, m =>
            {
                m.AddRuleWithParent<NullIfTheParentIsAManifest>();
                m.AddRuleWithParent<NotNullIfTheParentIsNotAManifest>();
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

        /// <summary>
        /// Validator builder which adds recursive validaiton for items which are attached to the manifest item object graph.
        /// </summary>
        public class RecursiveItemValidatorBuilder : IBuildsValidator<ManifestItem>
        {
            /// <inheritdoc/>
            public void ConfigureValidator(IConfiguresValidator<ManifestItem> config)
            {
                config.ValidateAsAncestor(1);
            }
        }
   }
}