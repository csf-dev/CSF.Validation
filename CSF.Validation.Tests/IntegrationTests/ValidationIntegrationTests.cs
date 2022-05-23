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

            var result = await validator.ValidateAsync(person).ConfigureAwait(false);
            
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

            var result = await validator.ValidateAsync(person).ConfigureAwait(false);

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

            var result = await validator.ValidateAsync(customer).ConfigureAwait(false);

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

            var result = await validator.ValidateAsync(customer).ConfigureAwait(false);

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

        [Test,AutoMoqData]
        public async Task ValidateAsyncWithMessageSupportShouldReturnMessagesForAFailingRule([IntegrationTesting] IGetsValidator validatorFactory)
        {
            var validator = validatorFactory.GetValidatorWithMessageSupport<Person>(typeof(PersonValidatorBuilder));
            var person = new Person
            {
                Name = "John Smith",
                Birthday = new DateTime(1750, 1, 1),
                Pets = Array.Empty<Pet>(),
            };

            var result = await validator.ValidateAsync(person).ConfigureAwait(false);

            Assert.That(result.RuleResults.Single(x => !x.IsPass).Message,
                        Is.EqualTo("The date 1750-01-01 is invalid. It must equal-to or later than 1900-01-01."));
        }

        [Test,AutoMoqData]
        public async Task ValidateAsyncWithMessageSupportShouldReturnAMessageFromARuleThatHasAMessage([IntegrationTesting] IGetsValidator validatorFactory)
        {
            var validator = validatorFactory.GetValidatorWithMessageSupport<Customer>(typeof(CustomerValidatorBuilder));
            var customer = new Customer
            {
                Person = new Person
                {
                    Name = "John Smith",
                    Birthday = new DateTime(2000, 1, 1),
                    Pets = new[] {
                        new Pet {Name = "Pet1"},
                        new Pet {Name = "Pet2"},
                        new Pet {Name = "Pet3"},
                        new Pet {Name = "Pet4"},
                        new Pet {Name = "Pet5"},
                        new Pet {Name = "Pet6"},
                    }
                }
            };

            var result = await validator.ValidateAsync(customer).ConfigureAwait(false);

            Assert.That(result.RuleResults.Single(x => !x.IsPass).Message,
                        Is.EqualTo("Nobody may have more than 5 pets."));
        }
    }
}