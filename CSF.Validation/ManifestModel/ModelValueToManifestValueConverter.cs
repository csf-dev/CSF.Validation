using System;
using System.Collections.Generic;
using CSF.Validation.Manifest;

namespace CSF.Validation.ManifestModel
{
    /// <summary>
    /// A service which converts <see cref="ModelToManifestConversionContext"/> instances (containing a <see cref="Value"/>)
    /// into a hierarchy of <see cref="CSF.Validation.Manifest.ManifestItem"/>, contained within a <see cref="ModelToManifestValueConversionResult"/>.
    /// </summary>
    public class ModelValueToManifestValueConverter : IConvertsModelValuesToManifestValues
    {
        readonly IGetsAccessorFunction accessorFactory;
        readonly IGetsValidatedType validatedTypeProvider;
        readonly IGetsManifestItemFromModelToManifestConversionContext contextToItemConverter;

        /// <summary>
        /// Converts all of the hierarchy of <see cref="Value"/> instances within the specified context into
        /// an equivalent hierarchy of <see cref="CSF.Validation.Manifest.ManifestItem"/>, which are returned
        /// as a result object.
        /// </summary>
        /// <param name="context">A conversion context.</param>
        /// <returns>A result object containing the converted manifest values.</returns>
        /// <exception cref="ValidatorBuildingException">If the input value(s) are not valid for creating a validation manifest.</exception>
        /// <exception cref="System.ArgumentNullException">If the <paramref name="context"/> is <see langword="null"/>.</exception>
        public ModelToManifestValueConversionResult ConvertAllValues(ModelToManifestConversionContext context)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            var openList = new Queue<ModelToManifestConversionContext>(new[] { context });
            var result = new ModelToManifestValueConversionResult();

            while (openList.Count != 0)
            {
                var current = openList.Dequeue();
                var value = contextToItemConverter.GetManifestItem(current);

                if (result.RootValue is null && value.IsValue)
                    result.RootValue = value;

                FindAndAddChildrenToOpenList(openList, current, value);

                result.ConvertedValues.Add(new ModelAndManifestValuePair
                {
                    ModelValue = current.CurrentValue,
                    ManifestValue = value,
                });
            }

            return result;
        }

        void FindAndAddChildrenToOpenList(Queue<ModelToManifestConversionContext> openList, ModelToManifestConversionContext currentContext, ManifestItem parent)
        {
            if(!(currentContext.CurrentValue.CollectionItemValue is null))
            {
                var validatedType = validatedTypeProvider.GetValidatedType(parent.ValidatedType, true);
                var collectionItem = new ModelToManifestConversionContext
                {
                    CurrentValue = currentContext.CurrentValue.CollectionItemValue,
                    MemberName = currentContext.MemberName,
                    ParentManifestValue = parent,
                    ValidatedType = validatedType,
                    ConversionType = currentContext.CurrentValue.CollectionItemValue.ValidateRecursivelyAsAncestor.HasValue
                        ? ModelToManifestConversionType.RecursiveManifestValue
                        : ModelToManifestConversionType.CollectionItem,
                };
                openList.Enqueue(collectionItem);
            }

            foreach(var child in currentContext.CurrentValue.Children)
            {
                var accessor = accessorFactory.GetAccessorFunction(parent.ValidatedType, child.Key);
                var collectionItem = new ModelToManifestConversionContext
                {
                    CurrentValue = child.Value,
                    AccessorFromParent = accessor.AccessorFunction,
                    MemberName = child.Key,
                    ParentManifestValue = parent,
                    ValidatedType = accessor.ExpectedType,
                    ConversionType = child.Value.ValidateRecursivelyAsAncestor.HasValue
                        ? ModelToManifestConversionType.RecursiveManifestValue
                        : ModelToManifestConversionType.Manifest,
                };
                openList.Enqueue(collectionItem);
            }

            if(currentContext.ConversionType != ModelToManifestConversionType.PolymorphicType)
            {
                foreach(var polyValue in currentContext.CurrentValue.PolymorphicValues)
                {
                    var polymorphicItem = new ModelToManifestConversionContext
                    {
                        CurrentValue = polyValue.Value,
                        ParentManifestValue = parent,
                        PolymorphicTypeName = polyValue.Key,
                        ConversionType = ModelToManifestConversionType.PolymorphicType,
                    };
                    openList.Enqueue(polymorphicItem);
                }
            }
        }

        /// <summary>
        /// Initialises an instance of <see cref="ModelValueToManifestValueConverter"/>.
        /// </summary>
        /// <param name="accessorFactory">A factory for accessor functions.</param>
        /// <param name="validatedTypeProvider">A service that gets the validated type.</param>
        /// <param name="contextToItemConverter">A converter service for model conversion contexts.</param>
        public ModelValueToManifestValueConverter(IGetsAccessorFunction accessorFactory,
                                                  IGetsValidatedType validatedTypeProvider,
                                                  IGetsManifestItemFromModelToManifestConversionContext contextToItemConverter)
        {
            this.validatedTypeProvider = validatedTypeProvider ?? throw new ArgumentNullException(nameof(validatedTypeProvider));
            this.contextToItemConverter = contextToItemConverter ?? throw new ArgumentNullException(nameof(contextToItemConverter));
            this.accessorFactory = accessorFactory ?? throw new ArgumentNullException(nameof(accessorFactory));
        }
    }
}