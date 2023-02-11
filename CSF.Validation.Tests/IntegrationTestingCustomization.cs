using System;
using System.Reflection;
using AutoFixture;
using CSF.Validation.ValidatorValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CSF.Validation
{
    public class IntegrationTestingCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            var provider = GetServiceProvider();
            fixture.Inject(provider);
            ResolveFromServiceProvider<IGetsValidator>(fixture);
            ResolveFromServiceProvider<IValidatesValidationManifest>(fixture);
        }

        static IServiceProvider GetServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection
                .UseValidationFramework()
                .UseStandardValidationRules()
                .UseValidationRulesInAssembly(Assembly.GetExecutingAssembly())
                .UseValidatorBuildersInAssembly(Assembly.GetExecutingAssembly())
                .UseMessageProviders(c => {
                    c.AddMessageProvider(typeof(IntegrationTests.DateTimeInRangeMessageProvider));
                    c.AddMessageProvider(typeof(IntegrationTests.CantBeOwnedByUnderageChildrenMessageProvider));
                    // Blocked on #76
                    // c.AddMessageProvidersInAssemblies(typeof(StandardRulesServiceCollectionExtensions).Assembly);
                })
                ;
            return serviceCollection.BuildServiceProvider();
        }

        static void ResolveFromServiceProvider<T>(IFixture fixture)
            => fixture.Customize<T>(c => c.FromFactory((IServiceProvider resolver) => resolver.GetRequiredService<T>()));
    }
}