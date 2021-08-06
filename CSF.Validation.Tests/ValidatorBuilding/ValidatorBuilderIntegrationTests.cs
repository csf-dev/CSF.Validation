
using System;
using CSF.Validation.Autofixture;
using CSF.Validation.Stubs;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace CSF.Validation.ValidatorBuilding
{
    [TestFixture, Parallelizable]
    public partial class ValidatorBuilderIntegrationTests
    {
        [Test,AutoMoqData]
        public void GetManifestRulesShouldNotReturnNull([IntegrationTesting] IServiceProvider services)
        {
            var sut = GetValidatorBuilderForComplexObjectValidator(services);
            var rules = sut.GetManifestRules();
            Assert.That(rules, Is.Not.Null);
        }

        static IGetsManifestRules GetValidatorBuilderForComplexObjectValidator(IServiceProvider services)
        {
            var factory = services.GetRequiredService<IGetsValidatorBuilder>();
            var sut = factory.GetValidatorBuilder<ComplexObject>();
            new ComplexObjectValidator().ConfigureValidator(sut);
            return sut;
        }
    }
}