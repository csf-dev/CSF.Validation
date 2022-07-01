using System;
using CSF.Validation.Manifest;

namespace CSF.Validation.ManifestModel
{
    /// <summary>
    /// A Chain of Responsibility impl of <see cref="IGetsManifestItemFromModelToManifestConversionContext"/> which deals with
    /// contexts for <see cref="ModelToManifestConversionType.PolymorphicType"/>.
    /// </summary>
    public class ContextToManifestPolymorphicTypeConverter : IGetsManifestItemFromModelToManifestConversionContext
    {
        readonly IGetsManifestItemFromModelToManifestConversionContext next;

        /// <inheritdoc/>
        public IManifestItem GetManifestItem(ModelToManifestConversionContext context)
        {
            if(context.ConversionType != ModelToManifestConversionType.PolymorphicType)
                return next.GetManifestItem(context);

            var validatedType = Type.GetType(context.PolymorphicTypeName, true);

            var polymorphicType = new ManifestPolymorphicType
            {
                Parent = context.ParentManifestValue,
                ValidatedType = validatedType,
            };
            
            if (context.ParentManifestValue is IHasPolymorphicTypes polyParent)
                polyParent.PolymorphicTypes.Add(polymorphicType);

            return polymorphicType;
        }

        /// <summary>
        /// Initialises a new instance of <see cref="ContextToManifestPolymorphicTypeConverter"/>.
        /// </summary>
        /// <param name="next">The wrapped/next implementation.</param>
        /// <exception cref="System.ArgumentNullException">If <paramref name="next"/> is <see langword="null" />.</exception>
        public ContextToManifestPolymorphicTypeConverter(IGetsManifestItemFromModelToManifestConversionContext next)
        {
            this.next = next ?? throw new System.ArgumentNullException(nameof(next));
        }
    }
}