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
        /// <remarks>
        /// <para>
        /// Use this method to configure the validation framework with your own application's dependency injection.
        /// This method makes all of the interfaces in the CSF.Validation.Abstractions assembly injectable via DI.
        /// Thus, validators may be defined and consumed by projects which have only a reference to the abstractions
        /// package, without requiring those projects to take a dependency upon the main CSF.Validation package.
        /// In this way, many applications will only require a reference to CSF.Validations from the project where
        /// DI is configured.
        /// </para>
        /// </remarks>
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
                .AddValidatorFactory()
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