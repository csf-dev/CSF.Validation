using CSF.Validation.Rules;
using Microsoft.Extensions.DependencyInjection;

namespace CSF.Validation.Bootstrap
{
    internal static class RulesServiceCollectionExtensions
    {
        internal static IServiceCollection AddRulesServices(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddTransient<IGetsValidationLogic, ValidationLogicFactory>()
                .AddTransient<IResolvesRule, ServiceProviderOrActivatorResolver>()
                .AddTransient<IResolvesServices, ServiceProviderOrActivatorResolver>()
                ;
        }
    }
}