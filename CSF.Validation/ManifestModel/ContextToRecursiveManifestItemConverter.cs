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
            try
            {
                return context.ParentManifestValue.GetAncestor(ancestorLevels - 1);
            }
            catch(ArgumentOutOfRangeException ex)
            {
                var message = String.Format(Resources.ExceptionMessages.GetExceptionMessage("AncestorLevelsMustNotBeLessThanOne"),
                                            nameof(ValueBase.ValidateRecursivelyAsAncestor));
                throw new ValidationException(message, ex);
            }
            catch(InvalidOperationException ex)
            {
                var message = String.Format(Resources.ExceptionMessages.GetExceptionMessage("NotEnoughAncestorsForAncestorLevels"),
                                            nameof(ValueBase.ValidateRecursivelyAsAncestor),
                                            nameof(ValueBase));
                throw new ValidationException(message, ex);
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