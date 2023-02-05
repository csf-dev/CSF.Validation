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
            config.ForMember(x => x.ValidatedType, m => m.AddRule<NotNull>());

            config.ForMember(x => x.Parent, m => m.AddRule<NotNull>());

            config.ForMember(x => x.CollectionItemValue, m => m.AddRules<ManifestCollectionItemValidatorBuilder>());
            config.AddRule<CollectionItemValueMustBeNullIfValidatedTypeIsNotEnumerable>();
            config.AddRule<CollectionItemValueMustValidateCompatibleTypeForValidatedType>(r =>
            {
                r.AddDependency(d => d.RuleType<CollectionItemValueMustBeNullIfValidatedTypeIsNotEnumerable>());
            });

            config.ForMember(x => x.Children, m => m.AddRule<ContainsNoNullItems<ManifestValue>>());
            config.ForMemberItems(x => x.Children, m => m.AddRules<ChildValueValidatorBuilder>());

            config.ForMember(x => x.Rules, m => m.AddRule<ContainsNoNullItems<ManifestRule>>());
            config.ForMemberItems(x => x.Rules, m => m.AddRules<ManifestRuleValidatorBuilder>());

            config.ForMember(x => x.PolymorphicTypes, m => m.AddRule<ContainsNoNullItems<ManifestPolymorphicType>>());
            config.ForMemberItems(x => x.PolymorphicTypes, m => m.AddRules<PolymorphicTypeValidatorBuilder>());

            config.WhenValueIs<ManifestPolymorphicType>(t =>
            {
                t.ForMember(x => x.PolymorphicTypes, m => m.AddRule<Empty>());
            });
        }

        /// <summary>
        /// Validator builder which adds recursive validaiton for child values within a validation manifest object graph.
        /// </summary>
        public class ChildValueValidatorBuilder : IBuildsValidator<ManifestValue>
        {
            /// <inheritdoc/>
            public void ConfigureValidator(IConfiguresValidator<ManifestValue> config)
            {
                config.ValidateAsAncestor(1);
            }
        }

        /// <summary>
        /// Validator builder which adds recursive validaiton for collection items within a validation manifest object graph.
        /// </summary>
        public class ManifestCollectionItemValidatorBuilder : IBuildsValidator<ManifestCollectionItem>
        {
            /// <inheritdoc/>
            public void ConfigureValidator(IConfiguresValidator<ManifestCollectionItem> config)
            {
                config.ValidateAsAncestor(1);
            }
        }

        /// <summary>
        /// Validator builder which adds recursive validaiton for polymorphic types within a validation manifest object graph.
        /// </summary>
        public class PolymorphicTypeValidatorBuilder : IBuildsValidator<ManifestPolymorphicType>
        {
            /// <inheritdoc/>
            public void ConfigureValidator(IConfiguresValidator<ManifestPolymorphicType> config)
            {
                config.ValidateAsAncestor(1);
            }
        }
    }
}