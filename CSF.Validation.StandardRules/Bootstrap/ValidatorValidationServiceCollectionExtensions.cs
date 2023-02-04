using CSF.Validation.ValidatorValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CSF.Validation.Bootstrap
{
    internal static class ValidatorValidationServiceCollectionExtensions
    {
        internal static IServiceCollection AddValidatorValidationServices(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddTransient<ManifestItemValidatorBuilder>()
                .AddTransient<ManifestValueValidatorBuilder>()
                .AddTransient<ValidationManifestValidatorBuilder>()
                .AddTransient<ManifestRuleValidatorBuilder>()
                .AddTransient<IValidatesValidationManifest, ValidationManifestValidator>()
                ;
        }
    }
}