using System;
using System.Linq;
using System.Threading.Tasks;
using CSF.Validation.Rules;
using NUnit.Framework;

namespace CSF.Validation.IntegrationTests
{
    [TestFixture,Parallelizable]
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

        [Test,AutoMoqData]
        public async Task ValidateAsyncShouldReturnPassResultForARulewithADependency([IntegrationTesting] IGetsValidator validatorFactory)
        {
            var validator = validatorFactory.GetValidator<Customer>(typeof(CustomerValidatorBuilder));

            var customer = new Customer
            {
                Person = new Person
                {
                    Name = "John Smith",
                    Birthday = new DateTime(2000, 1, 1),
                    Pets = new[] {
                        new Pet {
                            Name = "Miffles",
                            NumberOfLegs = 4,
                            Type = "Cat"
                        },
                    },
                }
            };

            var result = await validator.ValidateAsync(customer);

            Assert.That(result.Passed, Is.True);
        }

        [Test,AutoMoqData]
        public async Task ValidateAsyncShouldReturnFailureResultForARulewithAFailedDependency([IntegrationTesting] IGetsValidator validatorFactory)
        {
            var validator = validatorFactory.GetValidator<Customer>(typeof(CustomerValidatorBuilder));

            var customer = new Customer
            {
                Person = new Person
                {
                    Name = "John Smith",
                    Birthday = new DateTime(2000, 1, 1),
                    Pets = null,
                }
            };

            var result = await validator.ValidateAsync(customer);

            Assert.Multiple(() =>
            {
                Assert.That(result.Passed, Is.False, "Result is false");

                var dependencyFailures = result.RuleResults
                    .Where(x => !x.IsPass && x.Outcome == RuleOutcome.DependencyFailed)
                    .Select(x => x.Identifier.RuleType)
                    .ToList();
                var failures = result.RuleResults
                    .Where(x => !x.IsPass && x.Outcome == RuleOutcome.Failed)
                    .Select(x => x.Identifier.RuleType)
                    .ToList();
                Assert.That(failures, Is.EqualTo(new[] { typeof(NotNull) }), "One normal failure and it's the right type");

            });
        }
    }
}