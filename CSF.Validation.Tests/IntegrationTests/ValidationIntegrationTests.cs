using System;
using System.Threading.Tasks;
using CSF.Validation.Rules;
using NUnit.Framework;

namespace CSF.Validation.IntegrationTests
{
    [TestFixture,Parallelizable,Ignore("Temporarily broken, to be restored")]
    public class ValidationIntegrationTests
    {
        [Test,AutoMoqData]
        public async Task ValidateAsyncShouldReturnPassResultForValidObject([IntegrationTesting] IGetsValidator validatorFactory)
        {
            var validator = validatorFactory.GetValidator<Person>(typeof(PersonValidatorBuilder));

            var person = new Person
            {
                Name = "Bobby",
                Birthday = new DateTime(2000, 1, 1),
                Pets = new[] {
                    new Pet {
                        Name = "Miffles",
                        NumberOfLegs = 4,
                        Type = "Cat"
                    },
                },
            };

            var result = await validator.ValidateAsync(person);

            Assert.That(result.Passed, Is.True);
        }

        [Test,AutoMoqData]
        public async Task ValidateAsyncShouldReturnFailureResultForSingleFailedRule([IntegrationTesting] IGetsValidator validatorFactory)
        {
            var validator = validatorFactory.GetValidator<Person>(typeof(PersonValidatorBuilder));

            var person = new Person
            {
                Name = null,
                Birthday = new DateTime(2000, 1, 1),
                Pets = new[] {
                    new Pet {
                        Name = "Miffles",
                        NumberOfLegs = 4,
                        Type = "Cat"
                    },
                },
            };

            var result = await validator.ValidateAsync(person);

            Assert.Multiple(() =>
            {
                Assert.That(result.Passed, Is.False, "Overall result");
                Assert.That(result.RuleResults,
                            Has.One.Matches<ValidationRuleResult>(r => r.Identifier.MemberName == "Name" && r.Outcome == RuleOutcome.Failed),
                            "Correct failing rule");
            });
        }
    }
}