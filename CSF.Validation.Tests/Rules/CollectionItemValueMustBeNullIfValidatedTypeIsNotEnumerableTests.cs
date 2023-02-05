using System.Collections.Generic;
using CSF.Validation.Manifest;
using NUnit.Framework;

namespace CSF.Validation.Rules
{
    [TestFixture,Parallelizable]
    public class CollectionItemValueMustBeNullIfValidatedTypeIsNotEnumerableTests
    {
        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfCollectionItemValueIsNull(CollectionItemValueMustBeNullIfValidatedTypeIsNotEnumerable sut,
                                                                              [ManifestModel] ManifestValue value,
                                                                              [RuleContext] RuleContext context)
        {
            value.CollectionItemValue = null;
            Assert.That(() => sut.GetResultAsync(value, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfValidatedTypeIsNull(CollectionItemValueMustBeNullIfValidatedTypeIsNotEnumerable sut,
                                                                        [ManifestModel] ManifestValue value,
                                                                        [RuleContext] RuleContext context)
        {
            value.ValidatedType = null;
            Assert.That(() => sut.GetResultAsync(value, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfValidatedTypeIsEnumerable(CollectionItemValueMustBeNullIfValidatedTypeIsNotEnumerable sut,
                                                                              [ManifestModel] ManifestValue value,
                                                                              [ManifestModel] ManifestCollectionItem item,
                                                                              [RuleContext] RuleContext context)
        {
            value.CollectionItemValue = item;
            value.ValidatedType = typeof(List<string>);
            Assert.That(() => sut.GetResultAsync(value, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnFailIfValidatedTypeIsNotEnumerable(CollectionItemValueMustBeNullIfValidatedTypeIsNotEnumerable sut,
                                                                                 [ManifestModel] ManifestValue value,
                                                                                 [ManifestModel] ManifestCollectionItem item,
                                                                                 [RuleContext] RuleContext context)
        {
            value.CollectionItemValue = item;
            value.ValidatedType = typeof(int);
            Assert.That(() => sut.GetResultAsync(value, context), Is.FailingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetFailureMessageAsyncShouldReturnCorrectMessage(CollectionItemValueMustBeNullIfValidatedTypeIsNotEnumerable sut,
                                                                     [ManifestModel] ManifestValue value,
                                                                     [RuleResult] ValidationRuleResult result)
        {
            Assert.That(async () => await sut.GetFailureMessageAsync(value, result),
                        Is.EqualTo("If the ValidatedType of the ManifestItem does not implement IEnumerable<Object> then CollectionItemValue must be null."));
        }
    }
}