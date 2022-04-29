using Microsoft.Extensions.DependencyInjection;
using System;
using CSF.Validation.Bootstrap;
using System.Reflection;
using System.Collections.Generic;
using CSF.Validation.Messages;

namespace CSF.Validation
{
    /// <summary>
    /// Extension methods to add validation to a dependency injection container.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Use the methods of this class when <xref href="ConfiguringTheFramework?text=configuring+the+validation+framework"/>
    /// with your dependency injection container.
    /// </para>
    /// </remarks>
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
                .AddMessagesServices()
                ;
        }

        /// <summary>
        /// Scans the specified <paramref name="assembly"/> for validation rules and adds every one of them to
        /// the service collection, so that they may be dependency-injected.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Use this method, <see cref="UseValidationRulesInAssemblies(IServiceCollection, Assembly[])"/> or
        /// <see cref="UseValidationRulesInAssemblies(IServiceCollection, IEnumerable{Assembly})"/> as a convenient
        /// way to add many validation rules to your dependency injection container.
        /// </para>
        /// </remarks>
        /// <param name="serviceCollection">The service collection to which the validation rules should be added.</param>
        /// <param name="assembly">An assembly to scan for validation rule classes.</param>
        /// <returns>The service collection, so that calls may be chained.</returns>
        public static IServiceCollection UseValidationRulesInAssembly(this IServiceCollection serviceCollection, Assembly assembly)
            => serviceCollection.UseValidationRulesInAssemblies(assembly);

        /// <summary>
        /// Scans the specified <paramref name="assemblies"/> for validation rules and adds every one of them to
        /// the service collection, so that they may be dependency-injected.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Use this method, <see cref="UseValidationRulesInAssembly(IServiceCollection, Assembly)"/> or
        /// <see cref="UseValidationRulesInAssemblies(IServiceCollection, IEnumerable{Assembly})"/> as a convenient
        /// way to add many validation rules to your dependency injection container.
        /// </para>
        /// </remarks>
        /// <param name="serviceCollection">The service collection to which the validation rules should be added.</param>
        /// <param name="assemblies">A collection of assemblies to scan for validation rule classes.</param>
        /// <returns>The service collection, so that calls may be chained.</returns>
        public static IServiceCollection UseValidationRulesInAssemblies(this IServiceCollection serviceCollection, params Assembly[] assemblies)
            => serviceCollection.UseValidationRulesInAssemblies((IEnumerable<Assembly>)assemblies);

        /// <summary>
        /// Scans the specified <paramref name="assemblies"/> for validation rules and adds every one of them to
        /// the service collection, so that they may be dependency-injected.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Use this method, <see cref="UseValidationRulesInAssembly(IServiceCollection, Assembly)"/> or
        /// <see cref="UseValidationRulesInAssemblies(IServiceCollection, Assembly[])"/> as a convenient
        /// way to add many validation rules to your dependency injection container.
        /// </para>
        /// </remarks>
        /// <param name="serviceCollection">The service collection to which the validation rules should be added.</param>
        /// <param name="assemblies">A collection of assemblies to scan for validation rule classes.</param>
        /// <returns>The service collection, so that calls may be chained.</returns>
        public static IServiceCollection UseValidationRulesInAssemblies(this IServiceCollection serviceCollection, IEnumerable<Assembly> assemblies)
        {
            if (assemblies is null)
                throw new ArgumentNullException(nameof(assemblies));

            foreach (var ruleType in RuleAssemblyScanner.GetRuleTypesFromAssemblies(assemblies))
                serviceCollection.UseValidationRule(ruleType);

            return serviceCollection;
        }

        /// <summary>
        /// Adds a single validation rule type to the the service collection, so that it may be dependency-injected.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Use this method only when you wish to add individual rules to dependency injection.  It is usually more convenient
        /// to use one of the following methods to add rules in bulk, using assembly-scanning techniques.
        /// </para>
        /// <list type="bullet">
        /// <item><description><see cref="UseValidationRulesInAssembly(IServiceCollection, Assembly)"/></description></item>
        /// <item><description><see cref="UseValidationRulesInAssemblies(IServiceCollection, Assembly[])"/></description></item>
        /// <item><description><see cref="UseValidationRulesInAssemblies(IServiceCollection, IEnumerable{Assembly})"/></description></item>
        /// </list>
        /// </remarks>
        /// <param name="serviceCollection">The service collection to which the validation rule should be added.</param>
        /// <param name="ruleType">The type of validation rule to add to DI.</param>
        /// <returns>The service collection, so that calls may be chained.</returns>
        public static IServiceCollection UseValidationRule(this IServiceCollection serviceCollection, Type ruleType)
        {
            if (ruleType is null)
                throw new ArgumentNullException(nameof(ruleType));

            serviceCollection.AddTransient(ruleType);
            return serviceCollection;
        }

        /// <summary>
        /// Scans the specified <paramref name="assembly"/> for validation message providers and adds every one of them to
        /// the service collection, so that they may be dependency-injected.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Use this method, <see cref="UseMessageProvidersInAssemblies(IServiceCollection, Assembly[])"/> or
        /// <see cref="UseMessageProvidersInAssemblies(IServiceCollection, IEnumerable{Assembly})"/> as a convenient
        /// way to add many validation message providers to your dependency injection container.
        /// </para>
        /// <para>
        /// This method also registers the message provider types which are found with the default <see cref="IRegistryOfMessageTypes"/>:
        /// <see cref="MessageProviderRegistry.Default"/>.
        /// </para>
        /// </remarks>
        /// <param name="serviceCollection">The service collection to which the validation message providers should be added.</param>
        /// <param name="assembly">An assembly to scan for validation message provider classes.</param>
        /// <returns>The service collection, so that calls may be chained.</returns>
        public static IServiceCollection UseMessageProvidersInAssembly(this IServiceCollection serviceCollection, Assembly assembly)
            => serviceCollection.UseMessageProvidersInAssemblies(assembly);

        /// <summary>
        /// Scans the specified <paramref name="assemblies"/> for validation message providers and adds every one of them to
        /// the service collection, so that they may be dependency-injected.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Use this method, <see cref="UseMessageProvidersInAssembly(IServiceCollection, Assembly)"/> or
        /// <see cref="UseMessageProvidersInAssemblies(IServiceCollection, IEnumerable{Assembly})"/> as a convenient
        /// way to add many validation message providers to your dependency injection container.
        /// </para>
        /// <para>
        /// This method also registers the message provider types which are found with the default <see cref="IRegistryOfMessageTypes"/>:
        /// <see cref="MessageProviderRegistry.Default"/>.
        /// </para>
        /// </remarks>
        /// <param name="serviceCollection">The service collection to which the validation message providers should be added.</param>
        /// <param name="assemblies">A collection of assemblies to scan for validation message provider classes.</param>
        /// <returns>The service collection, so that calls may be chained.</returns>
        public static IServiceCollection UseMessageProvidersInAssemblies(this IServiceCollection serviceCollection, params Assembly[] assemblies)
            => serviceCollection.UseMessageProvidersInAssemblies((IEnumerable<Assembly>)assemblies);

        /// <summary>
        /// Scans the specified <paramref name="assemblies"/> for validation message providers and adds every one of them to
        /// the service collection, so that they may be dependency-injected.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Use this method, <see cref="UseMessageProvidersInAssembly(IServiceCollection, Assembly)"/> or
        /// <see cref="UseMessageProvidersInAssemblies(IServiceCollection, Assembly[])"/> as a convenient
        /// way to add many validation message providers to your dependency injection container.
        /// </para>
        /// <para>
        /// This method also registers the message provider types which are found with the default <see cref="IRegistryOfMessageTypes"/>:
        /// <see cref="MessageProviderRegistry.Default"/>.
        /// </para>
        /// </remarks>
        /// <param name="serviceCollection">The service collection to which the validation message providers should be added.</param>
        /// <param name="assemblies">A collection of assemblies to scan for validation message provider classes.</param>
        /// <returns>The service collection, so that calls may be chained.</returns>
        public static IServiceCollection UseMessageProvidersInAssemblies(this IServiceCollection serviceCollection, IEnumerable<Assembly> assemblies)
        {
            if (assemblies is null)
                throw new ArgumentNullException(nameof(assemblies));

            foreach (var providerType in MessageAssemblyScanner.GetMessageProviderTypesFromAssemblies(assemblies))
                serviceCollection.UseMessageProvider(providerType);

            return serviceCollection;
        }

        /// <summary>
        /// Adds a single validation message provider type to the the service collection, so that it may be dependency-injected.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Use this method only when you wish to add individual message providers to dependency injection.  It is usually more convenient
        /// to use one of the following methods to add message providers in bulk, using assembly-scanning techniques.
        /// </para>
        /// <list type="bullet">
        /// <item><description><see cref="UseMessageProvidersInAssembly(IServiceCollection, Assembly)"/></description></item>
        /// <item><description><see cref="UseMessageProvidersInAssemblies(IServiceCollection, Assembly[])"/></description></item>
        /// <item><description><see cref="UseMessageProvidersInAssemblies(IServiceCollection, IEnumerable{Assembly})"/></description></item>
        /// </list>
        /// <para>
        /// This method also registers the message provider type with the default <see cref="IRegistryOfMessageTypes"/>:
        /// <see cref="MessageProviderRegistry.Default"/>.
        /// </para>
        /// </remarks>
        /// <param name="serviceCollection">The service collection to which the validation rule should be added.</param>
        /// <param name="providerType">The type of validation message provider to add to DI.</param>
        /// <returns>The service collection, so that calls may be chained.</returns>
        public static IServiceCollection UseMessageProvider(this IServiceCollection serviceCollection, Type providerType)
        {
            if (providerType is null)
                throw new ArgumentNullException(nameof(providerType));

            MessageProviderRegistry.Default.RegisterMessageProviderTypes(providerType);
            serviceCollection.AddTransient(providerType);
            return serviceCollection;
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