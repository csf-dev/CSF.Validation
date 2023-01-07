using System;
using CSF.Validation.Manifest;

namespace CSF.Validation.ManifestModel
{
    /// <summary>
    /// A decorator for <see cref="IGetsManifestItemFromModelToManifestConversionContext"/> which populates the
    /// <see cref="ManifestItem.IdentityAccessor"/> if the result of the appropriate type.
    /// </summary>
    public class IdentityAccessorPopulatingManifestFromModelConversionContextDecorator : IGetsManifestItemFromModelToManifestConversionContext
    {
        readonly IGetsManifestItemFromModelToManifestConversionContext wrapped;
        readonly IGetsAccessorFunction accessorFactory;

        /// <inheritdoc/>
        public ManifestItem GetManifestItem(ModelToManifestConversionContext context)
        {
            var result = wrapped.GetManifestItem(context);

            var identityMember = context.CurrentValue.IdentityMemberName;
            if (result is ManifestItem valueBase && !String.IsNullOrWhiteSpace(identityMember))
            {
                var accessorFunction = accessorFactory.GetAccessorFunction(context.ValidatedType, identityMember).AccessorFunction;
                valueBase.IdentityAccessor = accessorFunction;
            }

            return result;
        }

        /// <summary>
        /// Initialises a new instance of <see cref="IdentityAccessorPopulatingManifestFromModelConversionContextDecorator"/>.
        /// </summary>
        /// <param name="wrapped">The wrapped service.</param>
        /// <param name="accessorFactory">An accessor function factory.</param>
        /// <exception cref="ArgumentNullException">If any parameter is <see langword="null" />.</exception>
        public IdentityAccessorPopulatingManifestFromModelConversionContextDecorator(IGetsManifestItemFromModelToManifestConversionContext wrapped,
                                                                                     IGetsAccessorFunction accessorFactory)
        {
            this.wrapped = wrapped ?? throw new ArgumentNullException(nameof(wrapped));
            this.accessorFactory = accessorFactory ?? throw new ArgumentNullException(nameof(accessorFactory));
        }
    }
}