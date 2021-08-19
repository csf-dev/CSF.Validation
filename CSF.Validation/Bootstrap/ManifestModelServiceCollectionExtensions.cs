using CSF.Validation.ManifestModel;
using Microsoft.Extensions.DependencyInjection;

namespace CSF.Validation.Bootstrap
{
    static class ManifestModelServiceCollectionExtensions
    {
        internal static IServiceCollection AddManifestModelServices(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddTransient<IGetsValidationManifestFromModel,ValidationManifestFromModelConverter>()
                .AddTransient<IConvertsModelValuesToManifestValues,ModelValueToManifestValueConverter>()
                .AddTransient<IConvertsModelRulesToManifestRules,ModelRuleToManifestRuleConverter>()
                .AddTransient<IGetsRuleConfiguration,RuleConfigurationFactory>()
                .AddTransient<IResolvesRuleType,RuleTypeResolver>()
                .AddTransient<IGetsAccessorFunction,ReflectionAccessorFunctionFactory>()
                ;
        }
    }
}