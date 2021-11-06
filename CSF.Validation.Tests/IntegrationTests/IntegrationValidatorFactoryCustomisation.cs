using System;
using AutoFixture;
using Microsoft.Extensions.DependencyInjection;

namespace CSF.Validation.IntegrationTests
{
    public class IntegrationValidatorFactoryCustomisation : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection
                .UseValidationFramework()
                .UseStandardValidationRules();
            var provider = serviceCollection.BuildServiceProvider();

            fixture.Inject(provider);
            fixture.Customize<IGetsValidator>(c => c.FromFactory((IServiceProvider resolver) => resolver.GetRequiredService<IGetsValidator>()));
        }
    }
}