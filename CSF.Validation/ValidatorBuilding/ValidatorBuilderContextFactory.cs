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
        public ValidatorBuilderContext GetContextForMember<TValidated, TValue>(Expression<Func<TValidated, TValue>> memberAccessor, ValidatorBuilderContext validatorContext, bool enumerateItems = false)
        {
            if (enumerateItems)
                return GetCollectionItemContext(validatorContext, typeof(TValue));

            if (memberAccessor is null)
                throw new ArgumentNullException(nameof(memberAccessor));

            var member = reflect.Member(memberAccessor);
            var accessor = memberAccessor.Compile();
            return GetContext(obj => accessor((TValidated)obj), validatorContext, typeof(TValue), member.Name);
        }

        /// <inheritdoc/>
        public ValidatorBuilderContext GetContextForValue<TValidated, TValue>(Func<TValidated, TValue> valueAccessor, ValidatorBuilderContext validatorContext, bool enumerateItems = false)
        {
            if(enumerateItems)
                return GetCollectionItemContext(validatorContext, typeof(TValue));

            return GetContext(obj => valueAccessor((TValidated)obj), validatorContext, typeof(TValue));
        }

        /// <inheritdoc/>
        public ValidatorBuilderContext GetPolymorphicContext(ValidatorBuilderContext validatorContext, Type derivedType)
        {
            if (validatorContext is null)
                throw new ArgumentNullException(nameof(validatorContext));
            if (derivedType is null)
                throw new ArgumentNullException(nameof(derivedType));
            if (validatorContext.ManifestValue is ManifestPolymorphicType)
            {
                var message = String.Format(Resources.ExceptionMessages.GetExceptionMessage("MustNotBeAPolymorphicType"),
                                            typeof(ManifestPolymorphicType).Name);
                throw new ArgumentException(message, nameof(validatorContext));
            }

            ManifestPolymorphicType existingPoly;
            if ((existingPoly = validatorContext.ManifestValue.PolymorphicTypes.FirstOrDefault(x => x.ValidatedType == derivedType)) != null)
                return new ValidatorBuilderContext(existingPoly);

            var polymorphicValue = new ManifestPolymorphicType
            {
                Parent = validatorContext.ManifestValue.Parent,
                ValidatedType = derivedType,
            };
            validatorContext.ManifestValue.PolymorphicTypes.Add(polymorphicValue);
            return new ValidatorBuilderContext(polymorphicValue);
        }

        static ValidatorBuilderContext GetContext(Func<object,object> accessor, ValidatorBuilderContext parentContext, Type validatedType, string memberName = null)
        {
            ManifestValue existingManifest;
            if(!(memberName is null) && (existingManifest = parentContext.ManifestValue.Children.OfType<ManifestValue>().FirstOrDefault(x => x.MemberName == memberName)) != null)
                return new ValidatorBuilderContext(existingManifest);

            var manifestValue = new ManifestValue
            {
                Parent = parentContext.ManifestValue,
                AccessorFromParent = accessor,
                MemberName = memberName,
                ValidatedType = validatedType,
            };
            parentContext.ManifestValue.Children.Add(manifestValue);
            return new ValidatorBuilderContext(manifestValue);
        }

        ValidatorBuilderContext GetCollectionItemContext(ValidatorBuilderContext parentContext, Type validatedType)
        {
            if(parentContext.ManifestValue.CollectionItemValue != null)
                return new ValidatorBuilderContext(parentContext.ManifestValue.CollectionItemValue);

            var manifestValue = new ManifestCollectionItem
            {
                Parent = parentContext.ManifestValue,
                ValidatedType = validatedType,
            };
            parentContext.ManifestValue.CollectionItemValue = manifestValue;
            return new ValidatorBuilderContext(manifestValue);
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