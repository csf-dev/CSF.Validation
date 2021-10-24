using Microsoft.Extensions.DependencyInjection;
using System;
using CSF.Validation.Bootstrap;

namespace CSF.Validation
{
    /// <summary>
    /// Extension methods to add validation to a dependency injection container.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the validation framework to the service collection, such that it may be injected by its interfaces.
        /// </summary>
        /// <param name="serviceCollection">The service collection to which the validation framework should be added.</param>
        /// <returns>The service collection, so that calls may be chained.</returns>
        public static IServiceCollection UseValidationFramework(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddExternalDependencyServices()
                .AddManifestServices()
                .AddManifestModelServices()
                .AddRuleExecutionServices()
                .AddRulesServices()
                .AddValidatorBuildingServices()
                ;
        }

        /// <summary>
        /// Helper method that registers <see cref="Func{T}"/> in the container by registering a lambda
        /// that resolves an instance from the service provider.
        /// </summary>
        /// <typeparam name="T">The service type</typeparam>
        /// <param name="serviceCollection">A service collection.</param>
        /// <returns>The service collection, so that calls may be chained.</returns>
        internal static IServiceCollection AddTransientFactory<T>(this IServiceCollection serviceCollection)
            => serviceCollection.AddTransient<Func<T>>(s => () => s.GetRequiredService<T>());
    }
}