using System;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using CSF.Validation.Rules;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.RuleExecution
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class RuleExecutorFactoryTests
    {
        [Test,AutoMoqData]
        public void GetRuleExecutorAsyncShouldReturnANonNullRuleExecutor([Frozen] IServiceProvider resolver,
                                                                         RuleExecutorFactory sut,
                                                                         ResolvedValidationOptions options,
                                                                         IGetsRuleExecutionContext dependencyTrackerFactory,
                                                                         IGetsSingleRuleExecutor ruleExecutorFactory,
                                                                         IGetsRuleContext contextFactory)
        {
            Mock.Get(resolver)
                .Setup(x => x.GetService(typeof(IGetsRuleExecutionContext)))
                .Returns(dependencyTrackerFactory);
            Mock.Get(resolver)
                .Setup(x => x.GetService(typeof(IGetsSingleRuleExecutor)))
                .Returns(ruleExecutorFactory);
            Mock.Get(resolver)
                .Setup(x => x.GetService(typeof(IGetsRuleContext)))
                .Returns(contextFactory);

            Assert.That(async () => await sut.GetRuleExecutorAsync(options), Is.Not.Null);
        }
    }
}