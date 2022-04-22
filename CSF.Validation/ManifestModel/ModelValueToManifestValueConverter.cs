using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using CSF.Validation.Manifest;

namespace CSF.Validation.ManifestModel
{
    /// <summary>
    /// A service which converts <see cref="ModelToManifestConversionContext"/> instances (containing a <see cref="Value"/>)
    /// into a hierarchy of <see cref="CSF.Validation.Manifest.ManifestValue"/>, contained within a <see cref="ModelToManifestValueConversionResult"/>.
    /// </summary>
    public class ModelValueToManifestValueConverter : IConvertsModelValuesToManifestValues
    {
        readonly IGetsAccessorFunction accessorFactory;
        readonly IGetsValidatedType validatedTypeProvider;

        /// <summary>
        /// Converts all of the hierarchy of <see cref="Value"/> instances within the specified context into
        /// an equivalent hierarchy of <see cref="CSF.Validation.Manifest.ManifestValue"/>, which are returned
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
                var value = ConvertToManifestValueBase(current);
                if (result.RootValue is null && value is ManifestValue manifestValue)
                    result.RootValue = manifestValue;

                FindAndAddChildrenToOpenList(openList, current, value);

                result.ConvertedValues.Add(new ModelAndManifestValuePair
                {
                    ModelValue = current.CurrentValue,
                    ManifestValue = value,
                });
            }

            return result;
        }

        void FindAndAddChildrenToOpenList(Queue<ModelToManifestConversionContext> openList, ModelToManifestConversionContext currentContext, ManifestValueBase parent)
        {
            if(!(currentContext.CurrentValue.CollectionItemValue is null))
            {
                var validatedType = validatedTypeProvider.GetValidatedType(parent.ValidatedType, true);
                var collectionItem = new ModelToManifestConversionContext
                {
                    CurrentValue = currentContext.CurrentValue.CollectionItemValue,
                    IsCollectionItem = true,
                    MemberName = currentContext.MemberName,
                    ParentManifestValue = parent,
                    ValidatedType = validatedType,
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
                };
                openList.Enqueue(collectionItem);
            }
        }

        ManifestValueBase ConvertToManifestValueBase(ModelToManifestConversionContext context)
        {
            var value = context.IsCollectionItem
                ? (ManifestValueBase) ConvertToManifestCollectionItem(context)
                : (ManifestValueBase) ConvertToManifestValue(context);

            if (!String.IsNullOrWhiteSpace(context.CurrentValue.IdentityMemberName))
                value.IdentityAccessor = accessorFactory.GetAccessorFunction(context.ValidatedType, context.CurrentValue.IdentityMemberName).AccessorFunction;

            return value;
        }

        static ManifestCollectionItem ConvertToManifestCollectionItem(ModelToManifestConversionContext context)
        {
            var manifestValue = new ManifestCollectionItem
            {
                Parent = context.ParentManifestValue.Parent,
                ValidatedType = context.ValidatedType,
            };
            if (context.ParentManifestValue != null && context.IsCollectionItem)
                context.ParentManifestValue.CollectionItemValue = manifestValue;
            return manifestValue;
        }

        static ManifestValue ConvertToManifestValue(ModelToManifestConversionContext context)
        {
            var manifestValue = new ManifestValue
            {
                Parent = context.ParentManifestValue,
                MemberName = context.MemberName,
                AccessorFromParent = context.AccessorFromParent,
                ValidatedType = context.ValidatedType,
            };
            if(context.CurrentValue is Value val)
                manifestValue.IgnoreAccessorExceptions = val.IgnoreAccessorExceptions;
            if (context.ParentManifestValue != null && !context.IsCollectionItem)
                context.ParentManifestValue.Children.Add(manifestValue);
            return manifestValue;
        }

        /// <summary>
        /// Initialises an instance of <see cref="ModelValueToManifestValueConverter"/>.
        /// </summary>
        /// <param name="accessorFactory">A factory for accessor functions.</param>
        /// <param name="validatedTypeProvider">A service that gets the validated type.</param>
        public ModelValueToManifestValueConverter(IGetsAccessorFunction accessorFactory, IGetsValidatedType validatedTypeProvider)
        {
            this.validatedTypeProvider = validatedTypeProvider ?? throw new ArgumentNullException(nameof(validatedTypeProvider));
            this.accessorFactory = accessorFactory ?? throw new ArgumentNullException(nameof(accessorFactory));
        }
    }
}