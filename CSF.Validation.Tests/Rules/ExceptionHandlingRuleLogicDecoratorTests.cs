using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.Rules
{
    [TestFixture,Parallelizable]
    public class ExceptionHandlingRuleLogicDecoratorTests
    {
        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnResultFromWrappedRuleIfItDoesNotThrow([Frozen] IValidationLogic wrapped,
                                                                                    ExceptionHandlingRuleLogicDecorator sut,
                                                                                    object value,
                                                                                    object parentValue,
                                                                                    [RuleContext] RuleContext context,
                                                                                    [RuleResult] RuleResult result)
        {
            Mock.Get(wrapped)
                .Setup(x => x.GetResultAsync(value, parentValue, context, It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(result));
            Assert.That(async () => await sut.GetResultAsync(value, parentValue, context, default), Is.SameAs(result));
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldNotThrowIfWrappedRuleThrows([Frozen] IValidationLogic wrapped,
                                                                    ExceptionHandlingRuleLogicDecorator sut,
                                                                    object value,
                                                                    object parentValue,
                                                                    [RuleContext] RuleContext context,
                                                                    Exception exception)
        {
            Mock.Get(wrapped)
                .Setup(x => x.GetResultAsync(value, parentValue, context, It.IsAny<CancellationToken>()))
                .Throws(exception);

            Assert.That(async () => await sut.GetResultAsync(value, parentValue, context, default), Throws.Nothing);
        }

        [Test,AutoMoqData]
        public async Task GetResultAsyncShouldReturnErroredResultIfWrappedRuleThrows([Frozen] IValidationLogic wrapped,
                                                                                     ExceptionHandlingRuleLogicDecorator sut,
                                                                                     object value,
                                                                                     object parentValue,
                                                                                     [RuleContext] RuleContext context,
                                                                                     Exception exception)
        {
            Mock.Get(wrapped)
                .Setup(x => x.GetResultAsync(value, parentValue, context, It.IsAny<CancellationToken>()))
                .Throws(exception);

            var result = await sut.GetResultAsync(value, parentValue, context, default);

            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Property(nameof(RuleResult.Outcome)).EqualTo(RuleOutcome.Errored));
                Assert.That(result, Has.Property(nameof(RuleResult.Exception)).SameAs(exception));
            });
        }
    }
}