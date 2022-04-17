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
                var manifestValue = ConvertToManifestValue(current);

                foreach (var child in current.CurrentValue.Children)
                {
                    var childContext = GetChildContext(child.Key, child.Value, current, manifestValue);
                    openList.Enqueue(childContext);
                }

                if (result.RootValue is null)
                    result.RootValue = manifestValue;

                result.ConvertedValues.Add(new ModelAndManifestValuePair
                {
                    ModelValue = current.CurrentValue,
                    ManifestValue = manifestValue,
                });
            }

            return result;
        }

        ManifestValue ConvertToManifestValue(ModelToManifestConversionContext context)
        {
            var validatedType = validatedTypeProvider.GetValidatedType(context.ValidatedType, context.CurrentValue.EnumerateItems);

            var manifestValue = new ManifestValue
            {
                Parent = context.ParentManifestValue,
                MemberName = context.MemberName,
                AccessorFromParent = context.AccessorFromParent,
                // EnumerateItems = context.CurrentValue.EnumerateItems,
                ValidatedType = validatedType,
                IgnoreAccessorExceptions = context.CurrentValue.IgnoreAccessorExceptions,
            };

            if (!String.IsNullOrWhiteSpace(context.CurrentValue.IdentityMemberName))
                manifestValue.IdentityAccessor = accessorFactory.GetAccessorFunction(validatedType, context.CurrentValue.IdentityMemberName).AccessorFunction;

            if (context.ParentManifestValue != null)
                context.ParentManifestValue.Children.Add(manifestValue);

            return manifestValue;
        }

        ModelToManifestConversionContext GetChildContext(string memberName,
                                                         Value value,
                                                         ModelToManifestConversionContext parentContext,
                                                         ManifestValue parentValue)
        {
            var parentValidatedType = validatedTypeProvider.GetValidatedType(parentContext.ValidatedType, parentContext.CurrentValue.EnumerateItems);
            var accessor = accessorFactory.GetAccessorFunction(parentValidatedType, memberName);

            return new ModelToManifestConversionContext
            {
                CurrentValue = value,
                AccessorFromParent = accessor.AccessorFunction,
                MemberName = memberName,
                ParentManifestValue = parentValue,
                ValidatedType = accessor.ExpectedType,
            };
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