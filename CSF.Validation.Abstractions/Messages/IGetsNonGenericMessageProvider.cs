using System;

namespace CSF.Validation.Messages
{
    /// <summary>
    /// An object which can get an instance of <see cref="IGetsFailureMessage"/> from a message provider type.
    /// </summary>
    public interface IGetsNonGenericMessageProvider
    {
        /// <summary>
        /// Gets the failure message provider for the specified type and original rule interface.
        /// </summary>
        /// <param name="providerType">The message provider type.</param>
        /// <param name="ruleInterface">The interface used for the original rule.</param>
        /// <returns>An implementation of <see cref="IGetsFailureMessage"/>.</returns>
        IGetsFailureMessage GetNonGenericFailureMessageProvider(MessageProviderTypeInfo providerType, Type ruleInterface);
    }
}