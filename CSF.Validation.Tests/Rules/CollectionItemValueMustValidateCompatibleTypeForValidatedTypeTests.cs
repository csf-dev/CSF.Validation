using System.Collections.Generic;
using CSF.Validation.Manifest;
using NUnit.Framework;

namespace CSF.Validation.Rules
{
    [TestFixture,Parallelizable]
    public class CollectionItemValueMustValidateCompatibleTypeForValidatedTypeTests
    {
        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfCollectionItemValidatedTypeIsNull(CollectionItemValueMustValidateCompatibleTypeForValidatedType sut,
                                                                                      [ManifestModel] ManifestValue value,
                                                                                      [ManifestModel] ManifestCollectionItem item,
                                                                                      [RuleContext] RuleContext context)
        {
            value.CollectionItemValue = item;
            value.CollectionItemValue.ValidatedType = null;
            Assert.That(() => sut.GetResultAsync(value, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfValidatedTypeIsNull(CollectionItemValueMustValidateCompatibleTypeForValidatedType sut,
                                                                        [ManifestModel] ManifestValue value,
                                                                        [ManifestModel] ManifestCollectionItem item,
                                                                        [RuleContext] RuleContext context)
        {
            value.ValidatedType = null;
            value.CollectionItemValue = item;
            value.CollectionItemValue.ValidatedType = typeof(int);
            Assert.That(() => sut.GetResultAsync(value, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfValidatedTypeIsCompatibleEnumerable(CollectionItemValueMustValidateCompatibleTypeForValidatedType sut,
                                                                                        [ManifestModel] ManifestValue value,
                                                                                        [ManifestModel] ManifestCollectionItem item,
                                                                                        [RuleContext] RuleContext context)
        {
            value.ValidatedType = typeof(List<Cat>);
            value.CollectionItemValue = item;
            value.CollectionItemValue.ValidatedType = typeof(Pet);
            Assert.That(() => sut.GetResultAsync(value, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnFailIfValidatedTypeIsIncompaatibleEnumerable(CollectionItemValueMustValidateCompatibleTypeForValidatedType sut,
                                                                                           [ManifestModel] ManifestValue value,
                                                                                           [ManifestModel] ManifestCollectionItem item,
                                                                                           [RuleContext] RuleContext context)
        {
            value.ValidatedType = typeof(List<Pet>);
            value.CollectionItemValue = item;
            value.CollectionItemValue.ValidatedType = typeof(Cat);
            Assert.That(() => sut.GetResultAsync(value, context), Is.FailingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetFailureMessageAsyncShouldReturnCorrectMessage(CollectionItemValueMustValidateCompatibleTypeForValidatedType sut,
                                                                     [ManifestModel] ManifestValue value,
                                                                     [ManifestModel] ManifestCollectionItem item,
                                                                     [RuleResult] ValidationRuleResult result)
        {
            value.ValidatedType = typeof(List<Pet>);
            value.CollectionItemValue = item;
            value.CollectionItemValue.ValidatedType = typeof(Cat);
            Assert.That(async () => await sut.GetFailureMessageAsync(value, result),
                        Is.EqualTo(@"When validating collection items, the ManifestItem.ValidatedType must be assignable to IEnumerable<T> for a generic type that matches the ManifestItem.CollectionItemValue.ValidatedType.
ManifestItem.ValidatedType = System.Collections.Generic.List`1[CSF.Validation.Rules.CollectionItemValueMustValidateCompatibleTypeForValidatedTypeTests+Pet]
ManifestItem.CollectionItemValue.ValidatedType = CSF.Validation.Rules.CollectionItemValueMustValidateCompatibleTypeForValidatedTypeTests+Cat"));
        }

        public class Pet {}

        public class Cat : Pet {}
    }
}