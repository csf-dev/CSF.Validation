using Microsoft.Extensions.DependencyInjection;
using CSF.Validation.ValidatorBuilding;
using System;

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
        public static void UseValidationFramework(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddTransient<CSF.Reflection.IStaticallyReflects,CSF.Reflection.Reflector>()
                .AddTransient<IGetsRuleBuilderContext,RuleBuilderContextFactory>()
                .AddTransient<IGetsRuleBuilder,RuleBuilderFactory>()
                .AddTransient<IGetsValueAccessorBuilder,ValueAccessorBuilderFactory>()
                .AddTransientFactory<IGetsManifestRuleIdentifierFromRelativeIdentifier>()
                .AddTransientFactory<IGetsRuleBuilder>()
                ;
        }

        static IServiceCollection AddTransientFactory<T>(this IServiceCollection serviceCollection)
            => serviceCollection.AddTransient<Func<T>>(s => () => s.GetService<T>());
    }
}