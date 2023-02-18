using System.Collections.Generic;
using CSF.Validation.Manifest;
using NUnit.Framework;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.Rules
{
    [TestFixture,NUnit.Framework.Parallelizable]
    public class MayNotHaveOwnItemsIfRecursiveTests
    {
        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfItemIsNotRecursive([ManifestModel] ManifestItem item,
                                                                       [ManifestModel] ManifestItem subordinate,
                                                                       [RuleContext] RuleContext context,
                                                                       MayNotHaveOwnItemsIfRecursive sut)
        {
            item.CollectionItemValue = subordinate;
            item.ItemType = ManifestItemTypes.Value;
            Assert.That(() => sut.GetResultAsync(item, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfItemIsRecursiveAndHasNoSubordinates([ManifestModel] ManifestItem item,
                                                                                        [RuleContext] RuleContext context,
                                                                                        MayNotHaveOwnItemsIfRecursive sut)
        {
            item.ItemType = ManifestItemTypes.RecursiveValue;
            Assert.That(() => sut.GetResultAsync(item, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnFailIfItemIsRecursiveAndHasSubordinates([ManifestModel] ManifestItem item,
                                                                                      [ManifestModel] ManifestItem subordinate,
                                                                                      [RuleContext] RuleContext context,
                                                                                      MayNotHaveOwnItemsIfRecursive sut)
        {
            item.CollectionItemValue = subordinate;
            item.ItemType = ManifestItemTypes.RecursiveValue;
            Assert.That(() => sut.GetResultAsync(item, context), Is.FailingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetFailureMessageAsyncShouldReturnCorrectMessage([ManifestModel] ManifestItem item,
                                                                     [RuleContext] RuleContext context,
                                                                     IValidationLogic logic,
                                                                     MayNotHaveOwnItemsIfRecursive sut)
        {
            var data = new Dictionary<string, object>
            {
                { "OwnCollectionItemValue", true },
                { "OwnChildren", 2 },
                { "OwnPolymorphicTypes", 3 },
                { "OwnRules", 4 },
            };
            var result = new ValidationRuleResult(new RuleResult(RuleOutcome.Failed, data), context, logic);
            Assert.That(async () => await sut.GetFailureMessageAsync(item, result),
                        Is.EqualTo(@"Manifest items which include the type Recursive must not have any of their own descendent items; they must all be derived from the recursive ancestor.
OwnCollectionItemValue is not null? True (must be null for recursive items)
OwnChildren has count 2 (must be empty for recursive items)
OwnPolymorphicTypes has count 3 (must be empty for recursive items)
OwnRules has count 4 (must be empty for recursive items)"));
        }
    }
}