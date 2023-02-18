using System;
using System.Linq;
using System.Threading.Tasks;
using CSF.Validation.ValidatorBuilding;
using NUnit.Framework;

namespace CSF.Validation.IntegrationTests
{
    [TestFixture, Parallelizable]
    public class RuleWithMessageIntegrationTests
    {
        static readonly Guid guid = Guid.NewGuid();

        [Test,AutoMoqData]
        public async Task ARuleWithMessageClassShouldHaveItsInstanceReusedWhenProvidingTheMessage([IntegrationTesting] IGetsValidator validatorFactory, object validated)
        {
            var options = new ValidationOptions { EnableMessageGeneration = true };
            var validator = validatorFactory.GetValidator<object>(new ObjectValidatorBuilder());
            var result = await validator.ValidateAsync(validated, options);
            Assert.That(result?.RuleResults.Single()?.Message, Is.EqualTo(guid.ToString()));
        }

        [Test,AutoMoqData]
        public async Task WhenProducingAMessageWithInstrumentationEnabledTheTimeTakenToGenerateTheMessageShouldBeRecorded([IntegrationTesting] IGetsValidator validatorFactory, object validated)
        {
            var options = new ValidationOptions { EnableMessageGeneration = true, InstrumentRuleExecution = true };
            var validator = validatorFactory.GetValidator<object>(new ObjectValidatorBuilder());
            var result = await validator.ValidateAsync(validated, options);
            Assert.That(result?.RuleResults.Single()?.InstrumentationData.MessageGenerationTime, Is.Not.Null);
        }

        public class ObjectValidatorBuilder : IBuildsValidator<object>
        {
            public void ConfigureValidator(IConfiguresValidator<object> config)
            {
                config.AddRule<SampleRuleWithMessage<object>>(r => r.ConfigureRule(x => x.GuidProperty = guid));
            }
        }
    }
}