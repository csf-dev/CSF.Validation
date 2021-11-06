using CSF.Validation.Manifest;
using Microsoft.Extensions.DependencyInjection;

namespace CSF.Validation.Bootstrap
{
    static class ManifestServiceCollectionExtensions
    {
        internal static IServiceCollection AddManifestServices(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddTransient<IGetsValidatedType, ValidatedTypeProvider>()
                .AddTransient<IGetsValidatorFromManifest, ValidatorFromManifestFactory>()
                .AddTransient<IGetsManifestFromBuilder, ManifestFromBuilderProvider>();
            ;
        }
    }
}