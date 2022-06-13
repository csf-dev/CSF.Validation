using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using CSF.Validation.Rules;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.RuleExecution
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class SingleRuleExecutorTests
    {
        [Test,AutoMoqData]
        public void ExecuteRuleAsyncShouldThrowIfCancellationIsRequestedWithoutExecutingRuleLogic([Frozen] IGetsRuleContext contextFactory,
                                                                                                  [RuleContext] RuleContext context,
                                                                                                  [ExecutableModel] ExecutableRule rule,
                                                                                                  SingleRuleExecutor sut,
                                                                                                  [AlreadyCancelled] CancellationToken token)
        {
            Mock.Get(contextFactory).Setup(x => x.GetRuleContext(rule)).Returns(context);
            Assert.That(async () => await sut.ExecuteRuleAsync(rule, token), Throws.InstanceOf<OperationCanceledException>());
            Mock.Get(rule.RuleLogic)
                .Verify(x => x.GetResultAsync(It.IsAny<object>(), It.IsAny<object>(), It.IsAny<RuleContext>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test,AutoMoqData]
        public async Task ExecuteRuleAsyncShouldExecuteRuleLogicWithCorrectValidatedInstances([Frozen] IGetsRuleContext contextFactory,
                                                                                              [RuleContext] RuleContext context,
                                                                                              [ExecutableModel] ExecutableRule rule,
                                                                                              SingleRuleExecutor sut,
                                                                                              ValidatedValue validatedValue,
                                                                                              ValidatedValue parentValue,
                                                                                              [RuleResult] RuleResult expectedResult)
        {
            rule.ValidatedValue = validatedValue;
            validatedValue.ParentValue = parentValue;

            Mock.Get(contextFactory).Setup(x => x.GetRuleContext(rule)).Returns(context);
            Mock.Get(rule.RuleLogic)
                .Setup(x => x.GetResultAsync(It.IsAny<object>(), It.IsAny<object>(), It.IsAny<RuleContext>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(expectedResult));

            await sut.ExecuteRuleAsync(rule);

            Mock.Get(rule.RuleLogic)
                .Verify(x => x.GetResultAsync(validatedValue.GetActualValue(), parentValue.GetActualValue(), It.IsAny<RuleContext>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test,AutoMoqData]
        public async Task ExecuteRuleAsyncShouldReturnResultCreatedFromRuleLogic([Frozen] IGetsRuleContext contextFactory,
                                                                                 [RuleContext] RuleContext context,
                                                                                 [ExecutableModel] ExecutableRule rule,
                                                                                 SingleRuleExecutor sut,
                                                                                 [RuleResult] RuleResult expectedResult)
        {
            Mock.Get(rule.RuleLogic)
                .Setup(x => x.GetResultAsync(It.IsAny<object>(), It.IsAny<object>(), It.IsAny<RuleContext>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(expectedResult));
            Mock.Get(contextFactory).Setup(x => x.GetRuleContext(rule)).Returns(context);

            var result = await sut.ExecuteRuleAsync(rule);

            Assert.Multiple(() =>
            {
                Assert.That(result.Data, Is.SameAs(expectedResult.Data), nameof(ValidationRuleResult.Data));
                Assert.That(result.Outcome, Is.EqualTo(expectedResult.Outcome), nameof(ValidationRuleResult.Outcome));
            });
        }

        [Test,AutoMoqData]
        public async Task ExecuteRuleAsyncShouldGetContextFromFactory([Frozen] IGetsRuleContext contextFactory,
                                                                               [RuleContext] RuleContext context,
                                                                               [ExecutableModel] ExecutableRule rule,
                                                                               SingleRuleExecutor sut,
                                                                               [RuleResult] RuleResult expectedResult)
        {
            RuleContext capturedContext = null;

            Mock.Get(rule.RuleLogic)
                .Setup(x => x.GetResultAsync(It.IsAny<object>(), It.IsAny<object>(), It.IsAny<RuleContext>(), It.IsAny<CancellationToken>()))
                .Callback((object v1, object v2, RuleContext context, CancellationToken t) => capturedContext = context)
                .Returns(() => Task.FromResult(expectedResult));
            Mock.Get(contextFactory).Setup(x => x.GetRuleContext(rule)).Returns(context);

            await sut.ExecuteRuleAsync(rule);

            Assert.That(capturedContext, Is.SameAs(context));
        }

        [Test,AutoMoqData]
        public async Task ExecuteRuleAsyncShouldReturnErrorResultWithTimeoutIfRuleTakesTooLongToExecute([Frozen] IGetsRuleContext contextFactory,
                                                                                                        [RuleContext] RuleContext context,
                                                                                                        [ExecutableModel] ExecutableRule rule,
                                                                                                        SingleRuleExecutor sut)
        {
            var timeout = TimeSpan.FromMilliseconds(100);
            Mock.Get(rule.RuleLogic).Setup(x => x.GetTimeout()).Returns(timeout);
            Mock.Get(rule.RuleLogic)
                .Setup(x => x.GetResultAsync(It.IsAny<object>(), It.IsAny<object>(), It.IsAny<RuleContext>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.Run(async () => {
                    await Task.Delay(200);
                    return CommonResults.Pass();
                }));
            Mock.Get(contextFactory).Setup(x => x.GetRuleContext(rule)).Returns(context);

            var result = await sut.ExecuteRuleAsync(rule);
            Assert.Multiple(() =>
            {
                Assert.That(result.Outcome, Is.EqualTo(RuleOutcome.Errored), "Result has error outcome");
                Assert.That(result.Data, Does.ContainKey(RuleResult.RuleTimeoutDataKey), "Result has a rule-timeout data key");
                
                if(result.Data.TryGetValue(RuleResult.RuleTimeoutDataKey, out var timeoutData))
                    Assert.That(timeoutData, Is.EqualTo(timeout), "Timeout data is equal");
            });
        }

        [Test,AutoMoqData]
        public void ExecuteRuleAsyncShouldReturnPassResultWithTimeoutIfRuleCompletesInATimelyManner([Frozen] IGetsRuleContext contextFactory,
                                                                                                    [RuleContext] RuleContext context,
                                                                                                    [ExecutableModel] ExecutableRule rule,
                                                                                                    SingleRuleExecutor sut)
        {
            var timeout = TimeSpan.FromMilliseconds(300);
            Mock.Get(rule.RuleLogic).Setup(x => x.GetTimeout()).Returns(timeout);
            Mock.Get(rule.RuleLogic)
                .Setup(x => x.GetResultAsync(It.IsAny<object>(), It.IsAny<object>(), It.IsAny<RuleContext>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.Run(async () => {
                    await Task.Delay(50);
                    return CommonResults.Pass();
                }));
            Mock.Get(contextFactory).Setup(x => x.GetRuleContext(rule)).Returns(context);

            Assert.That(() => sut.ExecuteRuleAsync(rule), Is.PassingValidationResult);
        }
    }
}