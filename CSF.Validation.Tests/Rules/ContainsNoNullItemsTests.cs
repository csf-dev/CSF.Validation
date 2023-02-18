using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSF.Validation.Rules
{
    [TestFixture,NUnit.Framework.Parallelizable]
    public class ContainsNoNullItemsTests
    {
        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfTheValidatedValueIsNull(ContainsNoNullItems sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync(null, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfTheCollectionContainsNoNulls(ContainsNoNullItems sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync(new[] { "foo", "bar" }, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnFailureIfTheCollectionContainsANullItem(ContainsNoNullItems sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync(new[] { "foo", null }, context), Is.FailingRuleResult);
        }

        [Test,AutoMoqData]
        public async Task GetResultAsyncShouldIncludeDataIndicatingTheNumberOfNulls(ContainsNoNullItems sut, [RuleContext] RuleContext context)
        {
            var result = await sut.GetResultAsync(new[] { "foo", null, "bar", null, null }, context);
            Assert.Multiple(() =>
            {
                Assert.That(result.Data, Contains.Key("Count of nulls"), "Result data contains a 'Count of nulls' item");
                Assert.That(() => result.Data.TryGetValue("Count of nulls", out var count) ? count : 0, Is.EqualTo(3), "Correct count of null items");
            });
        }

        [Test,AutoMoqData]
        public void GetFailureMessageAsyncShouldReturnCorrectMessageIfThereAreUnknownNulls(ContainsNoNullItems sut,
                                                                                           [RuleContext] RuleContext context,
                                                                                           IValidationLogic logic,
                                                                                           IEnumerable value)
        {
            var result = new ValidationRuleResult(new RuleResult(RuleOutcome.Failed), context, logic);
            Assert.That(async () => await sut.GetFailureMessageAsync(value, result),
                        Is.EqualTo("No items within this value may be null; at least one null item was found."));
        }

        [Test,AutoMoqData]
        public void GetFailureMessageAsyncShouldReturnCorrectMessageIfThereIsOneNull(ContainsNoNullItems sut,
                                                                                     [RuleContext] RuleContext context,
                                                                                     IValidationLogic logic,
                                                                                     IEnumerable value)
        {
            var result = new ValidationRuleResult(new RuleResult(RuleOutcome.Failed, new Dictionary<string, object> { { "Count of nulls", 1 } }), context, logic);
            Assert.That(async () => await sut.GetFailureMessageAsync(value, result),
                        Is.EqualTo("No items within this collection may be null; one null item was found."));
        }

        [Test,AutoMoqData]
        public void GetFailureMessageAsyncShouldReturnCorrectMessageIfThereAreThreeNulls(ContainsNoNullItems sut,
                                                                                         [RuleContext] RuleContext context,
                                                                                         IValidationLogic logic,
                                                                                         IEnumerable value)
        {
            var result = new ValidationRuleResult(new RuleResult(RuleOutcome.Failed, new Dictionary<string, object> { { "Count of nulls", 3 } }), context, logic);
            Assert.That(async () => await sut.GetFailureMessageAsync(value, result),
                        Is.EqualTo("No items within this collection may be null; 3 null items were found."));
        }

        [Test,AutoMoqData]
        public void GetFailureMessageAsyncShouldReturnAMessageForGenericEnumerable(ContainsNoNullItems<int> sut,
                                                                                   [RuleContext] RuleContext context,
                                                                                   IValidationLogic logic,
                                                                                   IEnumerable<int> value)
        {
            var result = new ValidationRuleResult(new RuleResult(RuleOutcome.Failed), context, logic);
            Assert.That(async () => await sut.GetFailureMessageAsync(value, result), Is.Not.Null);
        }

        [Test,AutoMoqData]
        public void GetFailureMessageAsyncShouldReturnAMessageForGenericQueryable(ContainsNoNullItems<int> sut,
                                                                                  [RuleContext] RuleContext context,
                                                                                  IValidationLogic logic,
                                                                                  IQueryable<int> value)
        {
            var result = new ValidationRuleResult(new RuleResult(RuleOutcome.Failed), context, logic);
            Assert.That(async () => await sut.GetFailureMessageAsync(value, result), Is.Not.Null);
        }
    }
}