using System;
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
                .AddTransient<IGetsAccessorFunction,ReflectionDelegateFactory>()
                .AddTransient<IGetsPropertySetterAction,ReflectionDelegateFactory>()
                .AddTransient<IGetsManifestItemFromModelToManifestConversionContext>(GetsModelConversionContextToManifestItemConverter)
                ;
        }

        /// <summary>
        /// Builds a Chain of Responsibility/Decorator stack to get a suitable impl of <see cref="IGetsManifestItemFromModelToManifestConversionContext"/>.
        /// </summary>
        /// <param name="serviceProvider">A service provider</param>
        /// <returns>The completed <see cref="IGetsManifestItemFromModelToManifestConversionContext"/> implementation.</returns>
        static IGetsManifestItemFromModelToManifestConversionContext GetsModelConversionContextToManifestItemConverter(IServiceProvider serviceProvider)
        {
            IGetsManifestItemFromModelToManifestConversionContext service;
            var accessorFactory = serviceProvider.GetRequiredService<IGetsAccessorFunction>();

            service = new ThrowingContextToManifestItemConverter();
            service = new ContextToRecursiveManifestItemConverter(service);
            service = new ContextToManifestPolymorphicTypeConverter(service);
            service = new ContextToManifestCollectionItemConverter(service);
            service = new ContextToManifestValueConverter(service);
            service = new IdentityAccessorPopulatingManifestFromModelConversionContextDecorator(service, accessorFactory);

            return service;
        }
    }
}