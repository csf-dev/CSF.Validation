using Microsoft.Extensions.DependencyInjection;
using CSF.Validation.Messages;

namespace CSF.Validation.Bootstrap
{
    static class MessagesServiceCollectionExtensions
    {
        internal static IServiceCollection AddMessagesServices(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IRegistryOfMessageTypes, MessageProviderRegistry>()
                .AddSingleton<IGetsRuleMatchingInfoForMessageProviderType, MessageProviderTypeMatchingInfoProvider>()
            ;
        }
    }
}