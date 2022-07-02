using System;
using CSF.Validation.Manifest;

namespace CSF.Validation.ManifestModel
{
    /// <summary>
    /// An implementation of <see cref="IGetsManifestItemFromModelToManifestConversionContext"/> which throws an exception.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is intended to be used as the innermost impl within a Cahin of Responsibility for converting contexts to manifest items.
    /// If the logic within this impl is entered then it means that no other impl was able to process the context.
    /// </para>
    /// </remarks>
    public class ThrowingContextToManifestItemConverter : IGetsManifestItemFromModelToManifestConversionContext
    {
        /// <inheritdoc/>
        public IManifestItem GetManifestItem(ModelToManifestConversionContext context)
        {
            var message = String.Format(Resources.ExceptionMessages.GetExceptionMessage("UnexpectedModelToManifestConversionType"),
                                        nameof(ModelToManifestConversionType));
            throw new ArgumentException(message, nameof(context));
        }
    }
}