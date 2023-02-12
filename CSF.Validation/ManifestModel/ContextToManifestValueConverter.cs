using CSF.Validation.Manifest;

namespace CSF.Validation.ManifestModel
{
    /// <summary>
    /// A Chain of Responsibility impl of <see cref="IGetsManifestItemFromModelToManifestConversionContext"/> which deals with
    /// contexts for <see cref="ModelToManifestConversionType.Manifest"/>.
    /// </summary>
    public class ContextToManifestValueConverter : IGetsManifestItemFromModelToManifestConversionContext
    {
        readonly IGetsManifestItemFromModelToManifestConversionContext next;

        /// <inheritdoc/>
        public ManifestItem GetManifestItem(ModelToManifestConversionContext context)
        {
            if(context.ConversionType != ModelToManifestConversionType.Manifest)
                return next.GetManifestItem(context);

            var manifestValue = new ManifestItem
            {
                Parent = context.ParentManifestValue,
                MemberName = context.MemberName,
                AccessorFromParent = context.AccessorFromParent,
                ValidatedType = context.ValidatedType,
            };

            if(context.CurrentValue is Value val)
                manifestValue.AccessorExceptionBehaviour = val.AccessorExceptionBehaviour;
            
            if (context.ParentManifestValue != null)
                context.ParentManifestValue.Children.Add(manifestValue);
            
            return manifestValue;
        }

        /// <summary>
        /// Initialises a new instance of <see cref="ContextToManifestValueConverter"/>.
        /// </summary>
        /// <param name="next">The wrapped/next implementation.</param>
        /// <exception cref="System.ArgumentNullException">If <paramref name="next"/> is <see langword="null" />.</exception>
        public ContextToManifestValueConverter(IGetsManifestItemFromModelToManifestConversionContext next)
        {
            this.next = next ?? throw new System.ArgumentNullException(nameof(next));
        }
    }
}