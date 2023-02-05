using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSF.Validation.Rules
{
    [TestFixture,NUnit.Framework.Parallelizable]
    public class DoesNotDeriveFromTests
    {
        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnFailIfTheValueIsTheSameType(DoesNotDeriveFrom<Pet> sut, [RuleContext] RuleContext context, Pet value)
        {
            Assert.That(() => sut.GetResultAsync(value, context), Is.FailingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnFailIfTheValueIsADerivedType(DoesNotDeriveFrom<Pet> sut, [RuleContext] RuleContext context, Cat value)
        {
            Assert.That(() => sut.GetResultAsync(value, context), Is.FailingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfTheValueIsNull(DoesNotDeriveFrom<Pet> sut, [RuleContext] RuleContext context)
        {
            Assert.That(() => sut.GetResultAsync(null, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassIfTheValueIsNotADerivedType(DoesNotDeriveFrom<Pet> sut, [RuleContext] RuleContext context, int value)
        {
            Assert.That(() => sut.GetResultAsync(value, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public async Task GetResultAsyncShouldRecordTheActualTypeAsData(DoesNotDeriveFrom<Pet> sut, [RuleContext] RuleContext context, Pet value)
        {
            var result = await sut.GetResultAsync(value, context);
            Assert.That(() => result.Data.TryGetValue("Actual type", out var type) ? type : null, Is.EqualTo(typeof(Pet)));
        }

        [Test,AutoMoqData]
        public void GetFailureMessageAsyncShouldReturnMessageWithTypes(DoesNotDeriveFrom<Pet> sut,
                                                                       [RuleContext] RuleContext context,
                                                                       IValidationLogic logic,
                                                                       object value)
        {
            var result = new ValidationRuleResult(new RuleResult(RuleOutcome.Failed, new Dictionary<string, object> { { "Actual type", typeof(Cat) } }), context, logic);
            Assert.That(async () => await sut.GetFailureMessageAsync(value, result),
                        Is.EqualTo("The value must not derive from CSF.Validation.Rules.DoesNotDeriveFromTests+Pet. The actual value is an instance of CSF.Validation.Rules.DoesNotDeriveFromTests+Cat."));
        }

        [Test,AutoMoqData]
        public void GetFailureMessageAsyncShouldReturnMessageWithMissingTypeIfDataNotPresent(DoesNotDeriveFrom<Pet> sut,
                                                                                             [RuleResult] ValidationRuleResult result,
                                                                                             object value)
        {
            Assert.That(async () => await sut.GetFailureMessageAsync(value, result),
                        Is.EqualTo("The value must not derive from CSF.Validation.Rules.DoesNotDeriveFromTests+Pet. The actual value is an instance of <unknown>."));
        }

        [Test,AutoMoqData]
        public void GetResultAsyncNonGenericShouldReturnFailIfTheValueIsTheSameType(DoesNotDeriveFrom sut, [RuleContext] RuleContext context, Pet value)
        {
            sut.Type = typeof(Pet);
            Assert.That(() => sut.GetResultAsync(value, context), Is.FailingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncNonGenericShouldReturnFailIfTheValueIsADerivedType(DoesNotDeriveFrom sut, [RuleContext] RuleContext context, Cat value)
        {
            sut.Type = typeof(Pet);
            Assert.That(() => sut.GetResultAsync(value, context), Is.FailingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncNonGenericShouldReturnPassIfTheValueIsNull(DoesNotDeriveFrom sut, [RuleContext] RuleContext context)
        {
            sut.Type = typeof(Pet);
            Assert.That(() => sut.GetResultAsync(null, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncNonGenericShouldReturnPassIfTheValueIsNotADerivedType(DoesNotDeriveFrom sut, [RuleContext] RuleContext context, int value)
        {
            sut.Type = typeof(Pet);
            Assert.That(() => sut.GetResultAsync(value, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public async Task GetResultAsyncNonGenericShouldRecordTheActualTypeAsData(DoesNotDeriveFrom sut, [RuleContext] RuleContext context, Pet value)
        {
            sut.Type = typeof(Pet);
            var result = await sut.GetResultAsync(value, context);
            Assert.That(() => result.Data.TryGetValue("Actual type", out var type) ? type : null, Is.EqualTo(typeof(Pet)));
        }

        [Test,AutoMoqData]
        public void GetFailureMessageAsyncNonGenericShouldReturnMessageWithTypes(DoesNotDeriveFrom sut,
                                                                       [RuleContext] RuleContext context,
                                                                       IValidationLogic logic,
                                                                       object value)
        {
            sut.Type = typeof(Pet);
            var result = new ValidationRuleResult(new RuleResult(RuleOutcome.Failed, new Dictionary<string, object> { { "Actual type", typeof(int) } }), context, logic);
            Assert.That(async () => await sut.GetFailureMessageAsync(value, result),
                        Is.EqualTo("The value must not derive from CSF.Validation.Rules.DoesNotDeriveFromTests+Pet. The actual value is an instance of System.Int32."));
        }

        [Test,AutoMoqData]
        public void GetFailureMessageAsyncNonGenericShouldReturnMessageWithMissingTypeIfDataNotPresent(DoesNotDeriveFrom sut,
                                                                                             [RuleResult] ValidationRuleResult result,
                                                                                             object value)
        {
            sut.Type = typeof(Pet);
            Assert.That(async () => await sut.GetFailureMessageAsync(value, result),
                        Is.EqualTo("The value must not derive from CSF.Validation.Rules.DoesNotDeriveFromTests+Pet. The actual value is an instance of <unknown>."));
        }

        public class Pet {}

        public class Cat : Pet {}
    }
}