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
                .UseValidationRulesInAssembly(Assembly.GetExecutingAssembly())
                .UseMessageProviders(c => {
                    c.AddMessageProvider(typeof(IntegrationTests.DateTimeInRangeMessageProvider));
                    c.AddMessageProvider(typeof(IntegrationTests.CantBeOwnedByUnderageChildrenMessageProvider));
                })
                ;
            return serviceCollection.BuildServiceProvider();
        }
    }
}