using System;
using AutoFixture;
using Microsoft.Extensions.DependencyInjection;

namespace CSF.Validation
{
    public class IntegrationTestingCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<IServiceProvider>(c => c.FromFactory(GetServiceProvider));
        }

        static IServiceProvider GetServiceProvider()
        {
            var services = new ServiceCollection();
            services.UseValidationFramework();
            return services.BuildServiceProvider();
        }
    }
}