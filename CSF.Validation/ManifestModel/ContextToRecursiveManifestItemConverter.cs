using System;
using System.Collections.Generic;
using System.Linq;
using CSF.Validation.Manifest;

namespace CSF.Validation.ManifestModel
{
    /// <summary>
    /// A Chain of Responsibility impl of <see cref="IGetsManifestItemFromModelToManifestConversionContext"/> which deals with
    /// contexts for <see cref="ModelToManifestConversionType.RecursiveManifestValue"/>.
    /// </summary>
    public class ContextToRecursiveManifestItemConverter : IGetsManifestItemFromModelToManifestConversionContext
    {
        readonly IGetsManifestItemFromModelToManifestConversionContext next;

        /// <inheritdoc/>
        public IManifestItem GetManifestItem(ModelToManifestConversionContext context)
        {
            if(context.ConversionType != ModelToManifestConversionType.RecursiveManifestValue)
                return next.GetManifestItem(context);

            var ancestor = GetAncestor(context);
            var recursiveItem = new RecursiveManifestValue(ancestor)
            {
                AccessorFromParent = context.AccessorFromParent,
                MemberName = context.MemberName,
                Parent = context.ParentManifestValue,
            };
            
            if (context.ParentManifestValue != null)
                context.ParentManifestValue.Children.Add(recursiveItem);

            return recursiveItem;
        }

        static IManifestItem GetAncestor(ModelToManifestConversionContext context)
        {
            var ancestorLevels = context.CurrentValue.ValidateRecursivelyAsAncestor.Value;
            if(ancestorLevels < 1)
            {
                var message = String.Format(Resources.ExceptionMessages.GetExceptionMessage("AncestorLevelsMustNotBeLessThanOne"),
                                            nameof(ValueBase.ValidateRecursivelyAsAncestor));
                throw new ValidationException(message);
            }

            var ancestors = GetAncestorManifestItems(context);

            try
            {
                return ancestors
                    .Skip(ancestorLevels - 1)
                    .First();
            }
            catch(InvalidOperationException ex)
            {
                var message = String.Format(Resources.ExceptionMessages.GetExceptionMessage("NotEnougAncestorsForAncestorLevels"),
                                            nameof(ValueBase.ValidateRecursivelyAsAncestor),
                                            ancestorLevels,
                                            ancestors.Count(),
                                            nameof(ValueBase));
                throw new ValidationException(message, ex);
            }
        }

        static IEnumerable<IManifestItem> GetAncestorManifestItems(ModelToManifestConversionContext context)
        {
            var current = context.ParentManifestValue;
            while(!(current is null))
            {
                yield return current;
                current = current.Parent;
            }
        }

        /// <summary>
        /// Initialises a new instance of <see cref="ContextToRecursiveManifestItemConverter"/>.
        /// </summary>
        /// <param name="next">The wrapped/next implementation.</param>
        /// <exception cref="System.ArgumentNullException">If <paramref name="next"/> is <see langword="null" />.</exception>
        public ContextToRecursiveManifestItemConverter(IGetsManifestItemFromModelToManifestConversionContext next)
        {
            this.next = next ?? throw new System.ArgumentNullException(nameof(next));
        }
    }
}