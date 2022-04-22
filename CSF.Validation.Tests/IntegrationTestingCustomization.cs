using System;
using System.Reflection;
using AutoFixture;
using Microsoft.Extensions.DependencyInjection;

namespace CSF.Validation
{
    public class IntegrationTestingCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            var provider = GetServiceProvider();
            fixture.Inject(provider);
            fixture.Customize<IGetsValidator>(c => c.FromFactory((IServiceProvider resolver) => resolver.GetRequiredService<IGetsValidator>()));
        }

        static IServiceProvider GetServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection
                .UseValidationFramework()
                .UseStandardValidationRules()
                .UseValidationRulesInAssembly(Assembly.GetExecutingAssembly());
            return serviceCollection.BuildServiceProvider();
        }
    }
}