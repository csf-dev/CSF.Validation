using CSF.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace CSF.Validation.Bootstrap
{
    static class ExternalDependenciesServiceCollectionExtensions
    {
        internal static IServiceCollection AddExternalDependencyServices(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddTransient<IStaticallyReflects,Reflector>();
        }
    }
}