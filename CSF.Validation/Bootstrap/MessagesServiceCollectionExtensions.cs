using Microsoft.Extensions.DependencyInjection;
using CSF.Validation.Messages;

namespace CSF.Validation.Bootstrap
{
    static class MessagesServiceCollectionExtensions
    {
        internal static IServiceCollection AddMessagesServices(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IGetsCandidateMessageTypes,MessageProviderRegistry>()
                .AddSingleton<IGetsRuleMatchingInfoForMessageProviderType, MessageProviderTypeMatchingInfoProvider>()
                .AddTransient<IGetsMessageProviderInfoFactory,DecoratorBasedMessageProviderInfoFactory>()
                .AddTransient<IGetsFailureMessageProvider,FailureMessageProviderSelector>()
                .AddTransient<IGetsNonGenericMessageCriteria,FailureMessageUsageCriteriaFactory>()
                .AddTransient<IAddsFailureMessagesToResult,FailureMessageValidationResultPopulator>()
                .AddTransient<IGetsMessageProviderFactoryStrategy,MessageProviderFactoryStrategyProvider>()
                .AddTransient<MessageProviderInfoFactory>()
                .AddTransient<DoubleGenericMessageProviderStrategy>()
                .AddTransient<NonGenericMessageProviderStrategy>()
                .AddTransient<SingleGenericMessageProviderStrategy>()
                ;
        }
    }
}