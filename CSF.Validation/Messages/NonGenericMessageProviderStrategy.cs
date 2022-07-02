using System;

namespace CSF.Validation.Messages
{
    /// <summary>
    /// A strategy for getting <see cref="IGetsFailureMessage"/> from a type that already implements that same non-generic interface.
    /// </summary>
    public class NonGenericMessageProviderStrategy : IGetsNonGenericMessageProvider
    {
        readonly IServiceProvider serviceProvider;

        /// <inheritdoc/>
        public IGetsFailureMessage GetNonGenericFailureMessageProvider(MessageProviderTypeInfo providerType, Type ruleInterface)
            => (IGetsFailureMessage)serviceProvider.GetService(providerType.ProviderType);

        /// <summary>
        /// Initialises a new instance of <see cref="NonGenericMessageProviderStrategy"/>.
        /// </summary>
        /// <param name="serviceProvider">A service provider.</param>
        /// <exception cref="ArgumentNullException">If the <paramref name="serviceProvider"/> is <see langword="null" />.</exception>
        public NonGenericMessageProviderStrategy(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }
    }
}