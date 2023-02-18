using Microsoft.Extensions.DependencyInjection;
using CSF.Validation.Messages;
using System;

namespace CSF.Validation.Bootstrap
{
    static class MessagesServiceCollectionExtensions
    {
        internal static IServiceCollection AddMessagesServices(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton(GetCandidateMessageProvider)
                .AddSingleton<IGetsRuleMatchingInfoForMessageProviderType, MessageProviderTypeMatchingInfoProvider>()
                .AddTransient<IGetsMessageProviderInfoFactory,DecoratorBasedMessageProviderInfoFactory>()
                .AddTransient<IGetsFailureMessageProvider,FailureMessageProviderSelector>()
                .AddTransient<IGetsNonGenericMessageCriteria,FailureMessageUsageCriteriaFactory>()
                .AddTransient<IAddsFailureMessagesToResult,FailureMessageValidationResultPopulator>()
                .AddTransient<IGetsMessageProviderFactoryStrategy,MessageProviderFactoryStrategyProvider>()
                .AddTransient<IGetsRuleWithMessageProvider,RuleWithMessageProviderFactory>()
                .AddTransient<MessageProviderInfoFactory>()
                .AddTransient<DoubleGenericMessageProviderStrategy>()
                .AddTransient<NonGenericMessageProviderStrategy>()
                .AddTransient<SingleGenericMessageProviderStrategy>()
                .AddTransient<UseExistingInstanceMessageProviderStrategy>()
                .AddTransient<MessageProviderRegistry>()
                ;
        }

        static IGetsCandidateMessageTypes GetCandidateMessageProvider(IServiceProvider serviceProvider)
        {
            var baseProvider = serviceProvider.GetRequiredService<MessageProviderRegistry>();
            return new RuleWithMessageCandidateTypeDecorator(baseProvider);
        }
    }
}