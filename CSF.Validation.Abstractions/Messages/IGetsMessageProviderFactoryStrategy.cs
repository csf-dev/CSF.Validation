using System;

namespace CSF.Validation.Messages
{
    /// <summary>
    /// An object which selects and returns a strategy for getting a non-generic message provider.
    /// </summary>
    public interface IGetsMessageProviderFactoryStrategy
    {
        /// <summary>
        /// Gets an appropriate strategy implementation for getting a message provider.
        /// If no suitable strategy is available then this method will return a <see langword="null" />
        /// reference.
        /// </summary>
        /// <param name="providerType">The candidate message provider type.</param>
        /// <param name="ruleInterface">The interface used for the validation rule.</param>
        /// <returns>Either a <see cref="IGetsNonGenericMessageProvider"/> or a <see langword="null" /> reference.</returns>
        IGetsNonGenericMessageProvider GetMessageProviderFactory(Type providerType, Type ruleInterface);
    }
}