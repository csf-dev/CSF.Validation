using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Rules;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.RuleExecution
{
    [TestFixture,Parallelizable]
    public class SingleRuleExecutorTests
    {
        [Test,AutoMoqData]
        public void ExecuteRuleAsyncShouldThrowIfCancellationIsRequestedWithoutExecutingRuleLogic([ExecutableModel] ExecutableRule rule,
                                                                                                  SingleRuleExecutor sut,
                                                                                                  [AlreadyCancelled] CancellationToken token)
        {
            Assert.That(async () => await sut.ExecuteRuleAsync(rule, token), Throws.InstanceOf<OperationCanceledException>());
            Mock.Get(rule.RuleLogic)
                .Verify(x => x.GetResultAsync(It.IsAny<object>(), It.IsAny<object>(), It.IsAny<RuleContext>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test,AutoMoqData]
        public async Task ExecuteRuleAsyncShouldExecuteRuleLogicWithCorrectValidatedInstances([ExecutableModel] ExecutableRule rule,
                                                                                              SingleRuleExecutor sut,
                                                                                              ValidatedValue validatedValue,
                                                                                              ValidatedValue parentValue,
                                                                                              [RuleResult] RuleResult expectedResult)
        {
            rule.ValidatedValue = validatedValue;
            validatedValue.ParentValue = parentValue;

            Mock.Get(rule.RuleLogic)
                .Setup(x => x.GetResultAsync(It.IsAny<object>(), It.IsAny<object>(), It.IsAny<RuleContext>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(expectedResult));

            await sut.ExecuteRuleAsync(rule);

            Mock.Get(rule.RuleLogic)
                .Verify(x => x.GetResultAsync(validatedValue.ActualValue, parentValue.ActualValue, It.IsAny<RuleContext>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test,AutoMoqData]
        public async Task ExecuteRuleAsyncShouldReturnResultCreatedFromRuleLogic([ExecutableModel] ExecutableRule rule,
                                                                                 SingleRuleExecutor sut,
                                                                                 [RuleResult] RuleResult expectedResult)
        {
            Mock.Get(rule.RuleLogic)
                .Setup(x => x.GetResultAsync(It.IsAny<object>(), It.IsAny<object>(), It.IsAny<RuleContext>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(expectedResult));

            var result = await sut.ExecuteRuleAsync(rule);

            Assert.Multiple(() =>
            {
                Assert.That(result.Data, Is.SameAs(expectedResult.Data), nameof(ValidationRuleResult.Data));
                Assert.That(result.Outcome, Is.EqualTo(expectedResult.Outcome), nameof(ValidationRuleResult.Outcome));
            });
        }

        [Test,AutoMoqData]
        public async Task ExecuteRuleAsyncShouldPassCorrectContextsToRuleLogic([ExecutableModel] ExecutableRule rule,
                                                                               SingleRuleExecutor sut,
                                                                               [RuleResult] RuleResult expectedResult,
                                                                               ValidatedValue value,
                                                                               ValidatedValue parentValue,
                                                                               ValidatedValue grandparentValue,
                                                                               ValidatedValue greatGrandparentValue)
        {
            rule.ValidatedValue = value;
            value.ParentValue = parentValue;
            parentValue.ParentValue = grandparentValue;
            grandparentValue.ParentValue = greatGrandparentValue;
            greatGrandparentValue.ParentValue = null;

            RuleContext capturedContext = null;

            Mock.Get(rule.RuleLogic)
                .Setup(x => x.GetResultAsync(It.IsAny<object>(), It.IsAny<object>(), It.IsAny<RuleContext>(), It.IsAny<CancellationToken>()))
                .Callback((object v1, object v2, RuleContext context, CancellationToken t) => capturedContext = context)
                .Returns(() => Task.FromResult(expectedResult));

            await sut.ExecuteRuleAsync(rule);

            Assert.Multiple(() =>
            {
                Assert.That(capturedContext?.Identifier,
                            Is.SameAs(rule.RuleIdentifier),
                            nameof(RuleContext.Identifier));
                Assert.That(capturedContext?.AncestorContexts.Select(x => x.Object).ToList(),
                            Is.EqualTo(new [] { parentValue.ActualValue, grandparentValue.ActualValue, greatGrandparentValue.ActualValue }),
                            nameof(RuleContext.AncestorContexts));
            });
        }
    }
}