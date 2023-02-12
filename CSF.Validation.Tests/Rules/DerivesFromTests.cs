using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSF.Validation.Rules
{
    [TestFixture,NUnit.Framework.Parallelizable]
    public class DerivesFromTests
    {
        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfTheValueIsTheSameType(DerivesFrom<Pet> sut, [RuleContext] RuleContext context, Pet value)
        {
            Assert.That(() => sut.GetResultAsync(value, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfTheValueIsADerivedType(DerivesFrom<Pet> sut, [RuleContext] RuleContext context, Cat value)
        {
            Assert.That(() => sut.GetResultAsync(value, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfTheValueIsNull(DerivesFrom<Pet> sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync(null, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnFailIfTheValueIsNotADerivedType(DerivesFrom<Pet> sut, [RuleContext] RuleContext context, int value)
        {
            Assert.That(() => sut.GetResultAsync(value, context), Is.FailingRuleResult);
        }

        [Test,AutoMoqData]
        public async Task GetResultAsyncShouldRecordTheActualTypeAsData(DerivesFrom<Pet> sut, [RuleContext] RuleContext context, int value)
        {
            var result = await sut.GetResultAsync(value, context);
            Assert.That(() => result.Data.TryGetValue("Actual type", out var type) ? type : null, Is.EqualTo(typeof(int)));
        }

        [Test,AutoMoqData]
        public void GetFailureMessageAsyncShouldReturnMessageWithTypes(DerivesFrom<Pet> sut,
                                                                       [RuleContext] RuleContext context,
                                                                       IValidationLogic logic,
                                                                       object value)
        {
            var result = new ValidationRuleResult(new RuleResult(RuleOutcome.Failed, new Dictionary<string, object> { { "Actual type", typeof(int) } }), context, logic);
            Assert.That(async () => await sut.GetFailureMessageAsync(value, result),
                        Is.EqualTo("The value must derive from CSF.Validation.Rules.DerivesFromTests+Pet. The actual value is an instance of System.Int32."));
        }

        [Test,AutoMoqData]
        public void GetFailureMessageAsyncShouldReturnMessageWithMissingTypeIfDataNotPresent(DerivesFrom<Pet> sut,
                                                                                             [RuleResult] ValidationRuleResult result,
                                                                                             object value)
        {
            Assert.That(async () => await sut.GetFailureMessageAsync(value, result),
                        Is.EqualTo("The value must derive from CSF.Validation.Rules.DerivesFromTests+Pet. The actual value is an instance of <unknown>."));
        }

        [Test,AutoMoqData]
        public void GetResultAsyncNonGenericShouldReturnPassIfTheValueIsTheSameType(DerivesFrom sut, [RuleContext] RuleContext context, Pet value)
        {
            sut.Type = typeof(Pet);
            Assert.That(() => sut.GetResultAsync(value, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncNonGenericShouldReturnPassIfTheValueIsADerivedType(DerivesFrom sut, [RuleContext] RuleContext context, Cat value)
        {
            sut.Type = typeof(Pet);
            Assert.That(() => sut.GetResultAsync(value, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncNonGenericShouldReturnPassIfTheValueIsNull(DerivesFrom sut, [RuleContext] RuleContext context)
        {
            sut.Type = typeof(Pet);
            Assert.That(() => sut.GetResultAsync(null, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncNonGenericShouldReturnFailIfTheValueIsNotADerivedType(DerivesFrom sut, [RuleContext] RuleContext context, int value)
        {
            sut.Type = typeof(Pet);
            Assert.That(() => sut.GetResultAsync(value, context), Is.FailingRuleResult);
        }

        [Test,AutoMoqData]
        public async Task GetResultAsyncNonGenericShouldRecordTheActualTypeAsData(DerivesFrom sut, [RuleContext] RuleContext context, int value)
        {
            sut.Type = typeof(Pet);
            var result = await sut.GetResultAsync(value, context);
            Assert.That(() => result.Data.TryGetValue("Actual type", out var type) ? type : null, Is.EqualTo(typeof(int)));
        }

        [Test,AutoMoqData]
        public void GetFailureMessageAsyncNonGenericShouldReturnMessageWithTypes(DerivesFrom sut,
                                                                       [RuleContext] RuleContext context,
                                                                       IValidationLogic logic,
                                                                       object value)
        {
            sut.Type = typeof(Pet);
            var result = new ValidationRuleResult(new RuleResult(RuleOutcome.Failed, new Dictionary<string, object> { { "Actual type", typeof(int) } }), context, logic);
            Assert.That(async () => await sut.GetFailureMessageAsync(value, result),
                        Is.EqualTo("The value must derive from CSF.Validation.Rules.DerivesFromTests+Pet. The actual value is an instance of System.Int32."));
        }

        [Test,AutoMoqData]
        public void GetFailureMessageAsyncNonGenericShouldReturnMessageWithMissingTypeIfDataNotPresent(DerivesFrom sut,
                                                                                             [RuleResult] ValidationRuleResult result,
                                                                                             object value)
        {
            sut.Type = typeof(Pet);
            Assert.That(async () => await sut.GetFailureMessageAsync(value, result),
                        Is.EqualTo("The value must derive from CSF.Validation.Rules.DerivesFromTests+Pet. The actual value is an instance of <unknown>."));
        }

        public class Pet {}

        public class Cat : Pet {}
    }
}