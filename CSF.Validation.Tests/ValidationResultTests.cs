using System.Linq;
using CSF.Validation.IntegrationTests;
using CSF.Validation.Manifest;
using NUnit.Framework;

namespace CSF.Validation
{
    [TestFixture,Parallelizable]
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
                        Throws.ArgumentException.And.Message.Contains("does not contain any values for a member named"));
        }

        [Test,AutoMoqData]
        public void ForMatchingMemberItemShouldThrowIfTheRequestedMemberIsNotACollectionItem([ManifestModel] ValidationManifest manifest,
                                                                                             [ManifestModel] ManifestValue value,
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
                        Throws.ArgumentException.And.Message.Contains("but in order to use ForMatchingMemberItem, that value must represent a collection of items"));
        }
    }
}