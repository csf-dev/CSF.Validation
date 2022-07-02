using Microsoft.Extensions.DependencyInjection;
using CSF.Validation.ValidatorBuilding;

namespace CSF.Validation.Bootstrap
{
    static class ValidatorBuildingServiceCollectionExtensions
    {
        internal static IServiceCollection AddValidatorBuildingServices(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddTransient<IGetsValidatorBuilderContext,ValidatorBuilderContextFactory>()
                .AddTransient<IGetsRuleBuilder,RuleBuilderFactory>()
                .AddTransient<IGetsValueAccessorBuilder,ValueAccessorBuilderFactory>()
                .AddTransient<IGetsValidatorManifest,ImportedValidatorBuilderManifestFactory>()
                .AddTransient<IGetsManifestRuleIdentifier,ManifestIdentifierFactory>()
                .AddTransient<IGetsValidatorBuilder,ValidatorBuilderFactory>()
                .AddTransient<IGetsManifestRuleIdentifierFromRelativeIdentifier,RelativeToManifestRuleIdentifierConverter>()
                .AddTransient<IGetsValidatedTypeForBuilderType,BuilderValidatedTypeProvider>()
                .AddTransientFactory<IGetsManifestRuleIdentifierFromRelativeIdentifier>()
                .AddTransientFactory<IGetsRuleBuilder>()
                .AddTransientFactory<IGetsValidatorBuilderContext>()
                .AddTransientFactory<IGetsValueAccessorBuilder>()
                .AddTransientFactory<IGetsValidatorManifest>()
                .AddTransientFactory<IGetsManifestRuleIdentifier>()
                ;
        }
    }
}