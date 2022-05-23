using System;

namespace CSF.Validation.Messages
{
    /// <summary>
    /// A strategy for <see cref="IGetsNonGenericMessageProvider"/> which uses an existing instance
    /// from a <see cref="InstanceMessageProviderInfo"/>.
    /// </summary>
    public class UseExistingInstanceMessageProviderStrategy : IGetsNonGenericMessageProvider
    {
        /// <inheritdoc/>
        public IGetsFailureMessage GetNonGenericFailureMessageProvider(MessageProviderTypeInfo providerType, Type ruleInterface)
            => ((InstanceMessageProviderInfo)providerType).MessageProvider;
    }
}