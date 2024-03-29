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
        /// Adds the validation framework to the service collection, such that it may be injected into classes
        /// which should consume validation.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Use this method to configure the validation framework with your own application's dependency injection.
        /// This method makes all of the interfaces in the <c>CSF.Validation.Abstractions</c> assembly (found in the
        /// corresponding NuGet package of the same name) injectable via DI.
        /// </para>
        /// <para>
        /// Thus, validators may be defined and consumed from .NET projects which have only a reference to the abstractions
        /// package.  These projects do not require a dependency upon the full CSF.Validation NuGet package.
        /// For larger applications with good project separation, this will mean that the only project(s) which need a reference
        /// to the full CSF.Validation NuGet package are startup projects where dependency injection is configured.
        /// </para>
        /// <para>
        /// If you provide an <paramref name="optionsAction"/> then you may configure the default options for all validators
        /// created using this dependency injection container.
        /// Validation options may still be provided at the point of performing validation, using either
        /// <see cref="IValidator.ValidateAsync(object, ValidationOptions, System.Threading.CancellationToken)"/> or
        /// <see cref="IValidator{TValidated}.ValidateAsync(TValidated, ValidationOptions, System.Threading.CancellationToken)"/>.
        /// When both default options are configured here, and options are specified upon the <c>ValidateAsync</c> method, the
        /// options specified at the point of validation take precedence.
        /// </para>
        /// </remarks>
        /// <seealso cref="IValidator.ValidateAsync(object, ValidationOptions, System.Threading.CancellationToken)"/>
        /// <seealso cref="IValidator{TValidated}.ValidateAsync(TValidated, ValidationOptions, System.Threading.CancellationToken)"/>
        /// <seealso cref="ValidationOptions"/>
        /// <param name="serviceCollection">The service collection to which the validation framework should be added.</param>
        /// <param name="optionsAction">An optional callback which allows setting up default validation options for the validator.</param>
        /// <returns>The service collection, so that calls may be chained.</returns>
        public static IServiceCollection UseValidationFramework(this IServiceCollection serviceCollection, Action<ValidationOptions> optionsAction = null)
        {
            serviceCollection
                .AddExternalDependencyServices()
                .AddManifestServices()
                .AddManifestModelServices()
                .AddRuleExecutionServices()
                .AddRulesServices()
                .AddValidatorBuildingServices()
                .AddValidatorFactory()
                .AddMessagesServices();

            var optionsRegistration = serviceCollection.AddOptions<ValidationOptions>();
            if(!(optionsAction is null))
                optionsRegistration.Configure(optionsAction);

            return serviceCollection;
        }

        #region Validation rules

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
            => serviceCollection.AddTransient(ruleType);
        
        #endregion
        
        #region Validator builders

        /// <summary>
        /// Scans the specified <paramref name="assembly"/> for validator builders and adds every one of them to
        /// the service collection, so that they may be dependency-injected.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Use this method, <see cref="UseValidatorBuildersInAssemblies(IServiceCollection, Assembly[])"/> or
        /// <see cref="UseValidatorBuildersInAssemblies(IServiceCollection, IEnumerable{Assembly})"/> as a convenient
        /// way to add many validator builders to your dependency injection container.
        /// </para>
        /// </remarks>
        /// <param name="serviceCollection">The service collection to which the validator builders should be added.</param>
        /// <param name="assembly">An assembly to scan for validator builder classes.</param>
        /// <returns>The service collection, so that calls may be chained.</returns>
        public static IServiceCollection UseValidatorBuildersInAssembly(this IServiceCollection serviceCollection, Assembly assembly)
            => serviceCollection.UseValidatorBuildersInAssemblies(assembly);

        /// <summary>
        /// Scans the specified <paramref name="assemblies"/> for validator builders and adds every one of them to
        /// the service collection, so that they may be dependency-injected.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Use this method, <see cref="UseValidatorBuildersInAssembly(IServiceCollection, Assembly)"/> or
        /// <see cref="UseValidatorBuildersInAssemblies(IServiceCollection, IEnumerable{Assembly})"/> as a convenient
        /// way to add many validator builders to your dependency injection container.
        /// </para>
        /// </remarks>
        /// <param name="serviceCollection">The service collection to which the validator builders should be added.</param>
        /// <param name="assemblies">A collection of assemblies to scan for validator builder classes.</param>
        /// <returns>The service collection, so that calls may be chained.</returns>
        public static IServiceCollection UseValidatorBuildersInAssemblies(this IServiceCollection serviceCollection, params Assembly[] assemblies)
            => serviceCollection.UseValidatorBuildersInAssemblies((IEnumerable<Assembly>)assemblies);

        /// <summary>
        /// Scans the specified <paramref name="assemblies"/> for validator builders and adds every one of them to
        /// the service collection, so that they may be dependency-injected.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Use this method, <see cref="UseValidatorBuildersInAssembly(IServiceCollection, Assembly)"/> or
        /// <see cref="UseValidatorBuildersInAssemblies(IServiceCollection, Assembly[])"/> as a convenient
        /// way to add many validator builders to your dependency injection container.
        /// </para>
        /// </remarks>
        /// <param name="serviceCollection">The service collection to which the validator builders should be added.</param>
        /// <param name="assemblies">A collection of assemblies to scan for validator builder classes.</param>
        /// <returns>The service collection, so that calls may be chained.</returns>
        public static IServiceCollection UseValidatorBuildersInAssemblies(this IServiceCollection serviceCollection, IEnumerable<Assembly> assemblies)
        {
            if (assemblies is null)
                throw new ArgumentNullException(nameof(assemblies));

            foreach (var ruleType in BuilderAssemblyScanner.GetValidatorBuilderTypesFromAssemblies(assemblies))
                serviceCollection.UseValidatorBuilder(ruleType);

            return serviceCollection;
        }

        /// <summary>
        /// Adds a single validator builder type to the the service collection, so that it may be dependency-injected.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Use this method only when you wish to add individual builders to dependency injection.  It is usually more convenient
        /// to use one of the following methods to add builders in bulk, using assembly-scanning techniques.
        /// </para>
        /// <list type="bullet">
        /// <item><description><see cref="UseValidatorBuildersInAssembly(IServiceCollection, Assembly)"/></description></item>
        /// <item><description><see cref="UseValidatorBuildersInAssemblies(IServiceCollection, Assembly[])"/></description></item>
        /// <item><description><see cref="UseValidatorBuildersInAssemblies(IServiceCollection, IEnumerable{Assembly})"/></description></item>
        /// </list>
        /// </remarks>
        /// <param name="serviceCollection">The service collection to which the validator builder should be added.</param>
        /// <param name="builderType">The type of validation builder to add to DI.</param>
        /// <returns>The service collection, so that calls may be chained.</returns>
        public static IServiceCollection UseValidatorBuilder(this IServiceCollection serviceCollection, Type builderType)
            => serviceCollection.AddTransient(builderType);

        #endregion

        #region Message providers

        /// <summary>
        /// Configures the validation framework with types to use as failure message providers, when
        /// <see cref="ResolvedValidationOptions.EnableMessageGeneration"/> is <see langword="true" />.
        /// </summary>
        /// <param name="serviceCollection">A service collection</param>
        /// <param name="configAction">An action which indicates which message provider types to use.</param>
        /// <returns>The service collection, so that calls may be chained.</returns>
        public static IServiceCollection UseMessageProviders(this IServiceCollection serviceCollection, Action<IRegistersMessageProviders> configAction)
        {
            var configurationHelper = new MessageProviderRegistrationBuilder();
            configAction(configurationHelper);

            serviceCollection
                .AddOptions<MessageProviderTypeOptions>()
                .Configure(opts => {
                    foreach(var type in configurationHelper.MessageProviderTypes)
                        opts.MessageProviderTypes.Add(type);
                });
            foreach(var type in configurationHelper.MessageProviderTypes)
                serviceCollection.AddTransient(type);
            
            return serviceCollection;
        }

        #endregion

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