using CSF.Validation.Manifest;

namespace CSF.Validation.ManifestModel
{
    /// <summary>
    /// A Chain of Responsibility impl of <see cref="IGetsManifestItemFromModelToManifestConversionContext"/> which deals with
    /// contexts for <see cref="ModelToManifestConversionType.CollectionItem"/>.
    /// </summary>
    public class ContextToManifestCollectionItemConverter : IGetsManifestItemFromModelToManifestConversionContext
    {
        readonly IGetsManifestItemFromModelToManifestConversionContext next;

        /// <inheritdoc/>
        public IManifestItem GetManifestItem(ModelToManifestConversionContext context)
        {
            if(context.ConversionType != ModelToManifestConversionType.CollectionItem)
                return next.GetManifestItem(context);

            var collectionItem = new ManifestCollectionItem
            {
                Parent = context.ParentManifestValue.Parent,
                ValidatedType = context.ValidatedType,
            };

            if (context.ParentManifestValue is ManifestValueBase mvb)
                mvb.CollectionItemValue = collectionItem;
            
            return collectionItem;
        }

        /// <summary>
        /// Initialises a new instance of <see cref="ContextToManifestCollectionItemConverter"/>.
        /// </summary>
        /// <param name="next">The wrapped/next implementation.</param>
        /// <exception cref="System.ArgumentNullException">If <paramref name="next"/> is <see langword="null" />.</exception>
        public ContextToManifestCollectionItemConverter(IGetsManifestItemFromModelToManifestConversionContext next)
        {
            this.next = next ?? throw new System.ArgumentNullException(nameof(next));
        }
    }
}