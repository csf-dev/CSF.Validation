using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CSF.Reflection;
using CSF.Validation.Manifest;

namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// A factory service for getting instances of <see cref="ValidatorBuilderContext"/>.
    /// </summary>
    public class ValidatorBuilderContextFactory : IGetsValidatorBuilderContext
    {
        readonly IStaticallyReflects reflect;

        /// <inheritdoc/>
        public ValidatorBuilderContext GetContextForMember<TValidated, TValue>(Expression<Func<TValidated, TValue>> memberAccessor, ValidatorBuilderContext validatorContext)
        {
            if (memberAccessor is null)
                throw new ArgumentNullException(nameof(memberAccessor));

            var member = reflect.Member(memberAccessor);
            var accessor = memberAccessor.Compile();
            return GetOrCreateContext(obj => accessor((TValidated)obj), validatorContext, typeof(TValue), member.Name);
        }

        /// <inheritdoc/>
        public ValidatorBuilderContext GetContextForValue<TValidated, TValue>(Func<TValidated, TValue> valueAccessor, ValidatorBuilderContext validatorContext)
        {
            return GetOrCreateContext(obj => valueAccessor((TValidated)obj), validatorContext, typeof(TValue));
        }

        /// <inheritdoc/>
        public ValidatorBuilderContext GetContextForCollection(ValidatorBuilderContext collectionContext, Type collectionItemType)
        {
            var collectionItemValue = collectionContext.ManifestValue.CollectionItemValue ?? new ManifestItem
            {
                Parent = collectionContext.ManifestValue.Parent,
                ValidatedType = collectionItemType,
                ItemType = ManifestItemType.CollectionItem,
            };

            return new ValidatorBuilderContext(collectionItemValue);
        }

        /// <inheritdoc/>
        public ValidatorBuilderContext GetPolymorphicContext(ValidatorBuilderContext validatorContext, Type derivedType)
        {
            if (validatorContext is null)
                throw new ArgumentNullException(nameof(validatorContext));
            if (derivedType is null)
                throw new ArgumentNullException(nameof(derivedType));
            if (validatorContext.ManifestValue.IsPolymorphicType)
            {
                var message = String.Format(Resources.ExceptionMessages.GetExceptionMessage("MustNotBeAPolymorphicType"),
                                            typeof(ManifestItem).Name);
                throw new ArgumentException(message, nameof(validatorContext));
            }

            var polymorphicValue = validatorContext.ManifestValue.PolymorphicTypes.FirstOrDefault(x => x.ValidatedType == derivedType) ?? new ManifestItem
            {
                Parent = validatorContext.ManifestValue.Parent,
                ValidatedType = derivedType,
                ItemType = ManifestItemType.PolymorphicType,
            };
            return new ValidatorBuilderContext(polymorphicValue);
        }

        static ValidatorBuilderContext GetOrCreateContext(Func<object,object> accessor, ValidatorBuilderContext parentContext, Type validatedType, string memberName = null)
        {
            return GetExistingContextForMember(parentContext, validatedType, memberName) ?? new ValidatorBuilderContext(new ManifestItem
            {
                Parent = parentContext.ManifestValue,
                AccessorFromParent = accessor,
                MemberName = memberName,
                ValidatedType = validatedType,
            });
        }

        static ValidatorBuilderContext GetExistingContextForMember(ValidatorBuilderContext parentContext, Type validatedType, string memberName)
        {
            if(memberName is null) return null;
            return parentContext.Contexts.FirstOrDefault(x => x.ManifestValue.MemberName == memberName);
        }

        /// <summary>
        /// Initialises a new instance of <see cref="ValidatorBuilderContextFactory"/>.
        /// </summary>
        /// <param name="reflect">A service which provides static reflection.</param>
        public ValidatorBuilderContextFactory(IStaticallyReflects reflect)
        {
            this.reflect = reflect ?? throw new ArgumentNullException(nameof(reflect));
        }
    }
}