using System.Linq;
using CSF.Validation.IntegrationTests;
using CSF.Validation.Manifest;
using CSF.Validation.Rules;
using NUnit.Framework;

namespace CSF.Validation
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class ValidationResultTests
    {
        [Test,AutoMoqData]
        public void ConstructorShouldThrowIfInstantiatedForAnIncompatibleType([ManifestModel] ValidationManifest manifest)
        {
            manifest.ValidatedType = typeof(string);
            Assert.That(() => new ValidationResult<int>(Enumerable.Empty<ValidationRuleResult>(), manifest), Throws.ArgumentException);
        }

        [Test,AutoMoqData]
        public void ConstructorShouldNotThrowIfInstantiatedForTheSameType([ManifestModel] ValidationManifest manifest)
        {
            manifest.ValidatedType = typeof(string);
            Assert.That(() => new ValidationResult<string>(Enumerable.Empty<ValidationRuleResult>(), manifest), Throws.Nothing);
        }

        [Test,AutoMoqData]
        public void ConstructorShouldNotThrowIfInstantiatedForACompatible([ManifestModel] ValidationManifest manifest)
        {
            manifest.ValidatedType = typeof(string);
            Assert.That(() => new ValidationResult<object>(Enumerable.Empty<ValidationRuleResult>(), manifest), Throws.Nothing);
        }

        [Test,AutoMoqData]
        public void ForMemberShouldThrowIfTheResultManifestHasNoChildrenOfThatName([ManifestModel] ValidationManifest manifest)
        {
            manifest.ValidatedType = typeof(string);
            var sut = new ValidationResult<string>(Enumerable.Empty<ValidationRuleResult>(), manifest);
            Assert.That(() => sut.ForMember(x => x.Length), Throws.ArgumentException);
        }

        [Test,AutoMoqData]
        public void ForMatchingMemberItemShouldThrowIfTheResultManifestHasNoChildrenOfThatName([ManifestModel] ValidationManifest manifest, Person person, Pet pet)
        {
            person.Pets.Add(pet);
            manifest.ValidatedType = typeof(Person);
            var sut = new ValidationResult<Person>(Enumerable.Empty<ValidationRuleResult>(), manifest);
            Assert.That(() => sut.ForMatchingMemberItem(x => x.Pets, pet),
                        Throws.ArgumentException.And.Message.Contains("must contain a value for a member named 'Pets' and that value must represent a collection of items."));
        }

        [Test,AutoMoqData]
        public void ForMatchingMemberItemShouldThrowIfTheRequestedMemberIsNotACollectionItem([ManifestModel] ValidationManifest manifest,
                                                                                             [ManifestModel] ManifestItem value,
                                                                                             Person person,
                                                                                             Pet pet)
        {
            person.Pets.Add(pet);
            manifest.ValidatedType = typeof(Person);
            manifest.RootValue.Children.Add(value);
            value.MemberName = nameof(Person.Pets);
            value.CollectionItemValue = null;
            var sut = new ValidationResult<Person>(Enumerable.Empty<ValidationRuleResult>(), manifest);
            Assert.That(() => sut.ForMatchingMemberItem(x => x.Pets, pet),
                        Throws.ArgumentException.And.Message.Contains("must contain a value for a member named 'Pets' and that value must represent a collection of items."));
        }

        [Test,AutoMoqData]
        public void PolymorphicAsShouldThrowIfTheManifestDoesNotHaveAMatchingPolymorphicType([ManifestModel] ValidationManifest manifest,
                                                                                             [ManifestModel] ManifestItem value)
        {
            manifest.ValidatedType = typeof(Person);
            manifest.RootValue = value;
            value.ValidatedType = typeof(Person);
            value.PolymorphicTypes.Clear();
            var sut = new ValidationResult<Person>(Enumerable.Empty<ValidationRuleResult>(), manifest);
            Assert.That(() => sut.PolymorphicAs<Employee>(),
                        Throws.ArgumentException.And.Message.StartsWith("The validation manifest value (for CSF.Validation.IntegrationTests.Person) must contain a polymorphic"));
        }

        [Test,AutoMoqData]
        public void ToStringShouldReturnCorrectStringWithNoOmittedOutcomes([RuleResult(Outcome = RuleOutcome.Passed)] ValidationRuleResult result1,
                                                                           [RuleResult(Outcome = RuleOutcome.Failed)] ValidationRuleResult result2,
                                                                           [RuleResult(Outcome = RuleOutcome.Errored)] ValidationRuleResult result3,
                                                                           [RuleResult(Outcome = RuleOutcome.DependencyFailed)] ValidationRuleResult result4,
                                                                           [ManifestModel] ValidationManifest manifest)
        {
            var sut = new ValidationResult<object>(new[] { result1, result2, result3, result4 }, manifest);
            var result = sut.ToString();

            Assert.Multiple(() =>
            {
                Assert.That(result, Does.Match(@"    \[ValidationRuleResult: Outcome = Passed"), "Includes the Passed rule");
                Assert.That(result, Does.Match(@"    \[ValidationRuleResult: Outcome = Failed"), "Includes the Failed rule");
                Assert.That(result, Does.Match(@"    \[ValidationRuleResult: Outcome = Errored"), "Includes the Errored rule");
                Assert.That(result, Does.Match(@"    \[ValidationRuleResult: Outcome = DependencyFailed"), "Includes the DependencyFailed rule");
                Assert.That(result, Does.Match(@"Rule results:"), "Matches message that does not indicate omissions");
            });
        }

        [Test,AutoMoqData]
        public void ToStringShouldReturnCorrectStringWithTwoOmittedOutcomes([RuleResult(Outcome = RuleOutcome.Passed)] ValidationRuleResult result1,
                                                                            [RuleResult(Outcome = RuleOutcome.Failed)] ValidationRuleResult result2,
                                                                            [RuleResult(Outcome = RuleOutcome.Errored)] ValidationRuleResult result3,
                                                                            [RuleResult(Outcome = RuleOutcome.DependencyFailed)] ValidationRuleResult result4,
                                                                            [ManifestModel] ValidationManifest manifest)
        {
            var sut = new ValidationResult<object>(new[] { result1, result2, result3, result4 }, manifest);
            var result = sut.ToString(new[] { RuleOutcome.Passed, RuleOutcome.DependencyFailed });

            Assert.Multiple(() =>
            {
                Assert.That(result, Does.Not.Match(@"    \[ValidationRuleResult: Outcome = Passed"), "Includes the Passed rule");
                Assert.That(result, Does.Match(@"    \[ValidationRuleResult: Outcome = Failed"), "Includes the Failed rule");
                Assert.That(result, Does.Match(@"    \[ValidationRuleResult: Outcome = Errored"), "Includes the Errored rule");
                Assert.That(result, Does.Not.Match(@"    \[ValidationRuleResult: Outcome = DependencyFailed"), "Includes the DependencyFailed rule");
                Assert.That(result, Does.Match(@"Rule results, omitting those with outcomes \{ Passed, DependencyFailed \}:"), "Matches message that indicates 2 omissions");
            });
        }
    }
}