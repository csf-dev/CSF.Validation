using System;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Messages;
using CSF.Validation.Rules;
using static CSF.Validation.Rules.CommonResults;
using NUnit.Framework;
using CSF.Validation.ValidatorBuilding;
using System.Linq;

namespace CSF.Validation.IntegrationTests
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class MessageThrowingAnExceptionIntegrationTests
    {
        [Test,AutoMoqData]
        public async Task WhenAMessageThrowsTheExceptionShouldBeStoredInTheRuleResult([IntegrationTesting] IGetsValidator validatorFactory, string value)
        {
            var validator = validatorFactory.GetValidator<string>(new ThrowingMessageValidatorBuilder());
            var options = new ValidationOptions
            {
                EnableMessageGeneration = true,
                TreatMessageGenerationErrorsAsRuleErrors = true,
                RuleThrowingBehaviour = RuleThrowingBehaviour.Never,
            };
            var result = await validator.ValidateAsync(value, options);

            Assert.Multiple(() =>
            {
                var ruleResult = result?.RuleResults.FirstOrDefault();
                Assert.That(ruleResult?.Exception, Is.Not.Null, "Exception not null");
                Assert.That(ruleResult?.Exception?.Message, Is.EqualTo("An exception occurred whilst getting the message for this rule; see the inner exception for more information."), "Exception message");
                Assert.That(ruleResult?.Exception?.InnerException?.Message, Is.EqualTo("This method always throws"), "Inner exception message");
                Assert.That(ruleResult?.Outcome, Is.EqualTo(RuleOutcome.Errored), "Outcome");
            });
        }

        [Test,AutoMoqData]
        public async Task WhenAMessageThrowsButErrorOptionIsFalseThenMessageShouldBeNullAndNoErrorRecorded([IntegrationTesting] IGetsValidator validatorFactory, string value)
        {
            var validator = validatorFactory.GetValidator<string>(new ThrowingMessageValidatorBuilder());
            var options = new ValidationOptions
            {
                EnableMessageGeneration = true,
                TreatMessageGenerationErrorsAsRuleErrors = false,
            };
            var result = await validator.ValidateAsync(value, options);

            Assert.Multiple(() =>
            {
                var ruleResult = result?.RuleResults.FirstOrDefault();
                Assert.That(ruleResult.Exception, Is.Null, "Exception not null");
                Assert.That(ruleResult.Message, Is.Null, "Message is null");
                Assert.That(ruleResult?.Outcome, Is.EqualTo(RuleOutcome.Failed), "Outcome");
            });
        }

        [Test,AutoMoqData]
        public void ValidateAsyncShouldThrowIfAMessageThrowsAndbothRuleErrorsAndRuleThrowingAreEnabled([IntegrationTesting] IGetsValidator validatorFactory, string value)
        {
            var validator = validatorFactory.GetValidator<string>(new ThrowingMessageValidatorBuilder());
            var options = new ValidationOptions
            {
                EnableMessageGeneration = true,
                TreatMessageGenerationErrorsAsRuleErrors = true,
                RuleThrowingBehaviour = RuleThrowingBehaviour.OnError,
            };

            Assert.That(async () => await validator.ValidateAsync(value, options), Throws.InstanceOf<ValidationException>());
        }

        public class SampleRule : IRule<string>
        {
            public ValueTask<RuleResult> GetResultAsync(string validated, RuleContext context, CancellationToken token = default)
                => FailAsync();
        }

        [FailureMessageStrategy(RuleType = typeof(SampleRule))]
        public class MessageThatThrowsAnException : IGetsFailureMessage<string>
        {
            public ValueTask<string> GetFailureMessageAsync(string value, ValidationRuleResult result, CancellationToken token = default)
                => throw new Exception("This method always throws");
        }

        public class ThrowingMessageValidatorBuilder : IBuildsValidator<string>
        {
            public void ConfigureValidator(IConfiguresValidator<string> config)
            {
                config.AddRule<SampleRule>();
            }
        }
    }
}