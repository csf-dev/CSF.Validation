using System;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.RuleExecution
{
    [TestFixture,Parallelizable]
    public class RuleExecutorFactoryTests
    {
        [Test,AutoMoqData]
        public void GetRuleExecutorAsyncShouldReturnAnInstanceOfSerialRuleExecutor([Frozen] IServiceProvider resolver,
                                                                                   RuleExecutorFactory sut,
                                                                                   ValidationOptions options,
                                                                                   IGetsRuleDependencyTracker dependencyTrackerFactory,
                                                                                   IGetsSingleRuleExecutor ruleExecutorFactory,
                                                                                   IGetsResultsAndUpdatesRulesWhichHaveDependencyFailures dependencyFailureResultProvider)
        {
            Mock.Get(resolver)
                .Setup(x => x.GetService(typeof(IGetsRuleDependencyTracker)))
                .Returns(dependencyTrackerFactory);
            Mock.Get(resolver)
                .Setup(x => x.GetService(typeof(IGetsSingleRuleExecutor)))
                .Returns(ruleExecutorFactory);
            Mock.Get(resolver)
                .Setup(x => x.GetService(typeof(IGetsResultsAndUpdatesRulesWhichHaveDependencyFailures)))
                .Returns(dependencyFailureResultProvider);

            Assert.That(async () => await sut.GetRuleExecutorAsync(options), Is.InstanceOf<SerialRuleExecutor>());
        }
    }
}