using System;
using System.Linq;
using System.Threading.Tasks;
using CSF.Validation.Manifest;
using CSF.Validation.Rules;
using NUnit.Framework;

namespace CSF.Validation.IntegrationTests
{
    [TestFixture, NUnit.Framework.Parallelizable]
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
        public async Task ValidateAsyncShouldReturnFailureResultForInvalidCollectionObject([IntegrationTesting] IGetsValidator validatorFactory)
        {
            var validator = validatorFactory.GetValidator<Person>(typeof(PersonValidatorBuilder));

            var person = new Person
            {
                Name = "Bobby",
                Birthday = new DateTime(2000, 1, 1),
                Pets = new[] {
                    new Pet {
                        Name = null,
                        NumberOfLegs = 4,
                        Type = "Cat"
                    },
                },
            };

            var result = await validator.ValidateAsync(person).ConfigureAwait(false);
            
            Assert.That(result.Passed, Is.False);
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
            var validator = validatorFactory.GetValidator<Person>(typeof(PersonValidatorBuilder));
            var person = new Person
            {
                Name = "John Smith",
                Birthday = new DateTime(1750, 1, 1),
                Pets = Array.Empty<Pet>(),
            };

            var result = await validator.ValidateAsync(person, new ValidationOptions { EnableMessageGeneration = true }).ConfigureAwait(false);

            Assert.That(result.RuleResults.Single(x => !x.IsPass).Message,
                        Is.EqualTo("The date 1750-01-01 is invalid. It must equal-to or later than 1900-01-01."));
        }

        [Test,AutoMoqData]
        public async Task ValidateAsyncWithMessageSupportShouldReturnAMessageFromARuleThatHasAMessage([IntegrationTesting] IGetsValidator validatorFactory)
        {
            var validator = validatorFactory.GetValidator<Customer>(typeof(CustomerValidatorBuilder));
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

            var result = await validator.ValidateAsync(customer, new ValidationOptions { EnableMessageGeneration = true }).ConfigureAwait(false);

            Assert.That(result.RuleResults.Single(x => !x.IsPass).Message,
                        Is.EqualTo("Nobody may have more than 5 pets."));
        }

        [Test,AutoMoqData]
        public async Task ForMemberAndWithoutSuccessesShouldBeUsableToTraverseToASingleResult([IntegrationTesting] IGetsValidator validatorFactory)
        {
            var validator = validatorFactory.GetValidator<Customer>(typeof(CustomerValidatorBuilder));
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

            var result = await validator.ValidateAsync(customer, new ValidationOptions { EnableMessageGeneration = true }).ConfigureAwait(false);

            Assert.That(result.ForMember(x => x.Person).WithoutSuccesses().Single().Message,
                        Is.EqualTo("Nobody may have more than 5 pets."));
        }

        [Test,AutoMoqData]
        public async Task ForMatchingMemberItemCanFindRulesForTheCorrectItem([IntegrationTesting] IGetsValidator validatorFactory)
        {
            var validator = validatorFactory.GetValidator<Customer>(typeof(CustomerValidatorBuilder));
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

            var result = await validator.ValidateAsync(customer, new ValidationOptions { EnableMessageGeneration = true }).ConfigureAwait(false);

            var expectedPet = customer.Person.Pets.Skip(1).First();
            Assert.That(result.ForMember(x => x.Person).ForMatchingMemberItem(x => x.Pets, expectedPet).ForOnlyThisValue().First().ValidatedValue,
                        Is.SameAs(expectedPet));
        }

        [Test,AutoMoqData]
        public async Task ToSerializableResultShouldReturnSameNumberOfRuleResultsAsOriginalResult([IntegrationTesting] IGetsValidator validatorFactory)
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
            
            Assert.That(result.ToSerializableResult().RuleResults.Length, Is.EqualTo(result.RuleResults.Count));
        }

        [Test,AutoMoqData]
        public async Task PolymorphicValidationShouldBeAbleToCreateAnErrorResultForADerivedClass([IntegrationTesting] IGetsValidator validatorFactory)
        {
            var validator = validatorFactory.GetValidator<Customer>(typeof(CustomerValidatorBuilder));
            var customer = new FrequentShopper
            {
                Person = new Employee
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
                    },
                    PayrollNumber = -5,
                },
                LoyaltyPoints = -100,
            };

            var result = await validator.ValidateAsync(customer).ConfigureAwait(false);

            Assert.That(result.ForMember(x => x.Person).PolymorphicAs<Employee>().ForMember(x => x.PayrollNumber),
                        Has.One.Matches<ValidationRuleResult>(r => r.Identifier.RuleType == typeof(IntegerInRange)
                                                                && r.IsPass == false
                                                                && Equals(r.ValidatedValue, -5)));
        }

        [Test,AutoMoqData]
        public async Task PolymorphicAsShouldThrowIfUsedTwiceInSuccessionFromAResult([IntegrationTesting] IGetsValidator validatorFactory)
        {
            var validator = validatorFactory.GetValidator<Customer>(typeof(CustomerValidatorBuilder));
            var customer = new FrequentShopper
            {
                Person = new Employee
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
                    },
                    PayrollNumber = -5,
                },
                LoyaltyPoints = -100,
            };

            var result = await validator.ValidateAsync(customer).ConfigureAwait(false);

            Assert.That(() => result.ForMember(x => x.Person).PolymorphicAs<Employee>().PolymorphicAs<Employee>(),
                        Throws.ArgumentException.And.Message.StartWith("The validation manifest value for the current context must implement IHasPolymorphicTypes"));
        }

        [Test,AutoMoqData,Timeout(500)]
        public async Task RecursiveValidationShouldReturnaResultFromADescendentObjectValidatedUsingTheSameManifestAsAnAncestor([IntegrationTesting] IGetsValidator validatorFactory)
        {
            var manifest = new ValidationManifest
            {
                ValidatedType = typeof(Node),
                RootValue = new ManifestValue
                {
                    ValidatedType = typeof(Node),
                    IdentityAccessor = obj => ((Node)obj).Identity,
                    Children = new[] {
                        new ManifestValue
                        {
                            ValidatedType = typeof(NodeChild),
                            IdentityAccessor = obj => ((NodeChild) obj).Identity,
                            AccessorFromParent = obj => ((Node) obj).Child,
                        },
                        new ManifestValue
                        {
                            ValidatedType = typeof(string),
                            AccessorFromParent = obj => ((Node) obj).Name,
                        }
                    }
                }
            };
            var nameValue = manifest.RootValue.Children.Single(x => x.ValidatedType == typeof(string));
            var nameRule = new ManifestRule(nameValue, new ManifestRuleIdentifier(nameValue, typeof(MatchesRegex)))
            {
                RuleConfiguration = obj => ((MatchesRegex) obj).Pattern = "^Foo",
            };
            nameValue.Rules.Add(nameRule);
            var childValue = manifest.RootValue.Children.Single(x => x.ValidatedType == typeof(NodeChild));
            var recursiveValue = new RecursiveManifestValue(manifest.RootValue)
            {
                AccessorFromParent = obj => ((NodeChild)obj).Node,
            };
            childValue.Children.Add(recursiveValue);

            var validatedObject = new Node
            {
                Child = new NodeChild
                {
                    Node = new Node
                    {
                        Child = new NodeChild { Node = new Node { Name = "Invalid" } }
                    }
                }
            };
            var sut = validatorFactory.GetValidator<Node>(manifest);

            var result = await sut.ValidateAsync(validatedObject);
            Assert.That(result, Has.One.Matches<ValidationRuleResult>(r => r.Outcome == RuleOutcome.Failed && Equals(r.ValidatedValue, "Invalid")));
        }
    }
}