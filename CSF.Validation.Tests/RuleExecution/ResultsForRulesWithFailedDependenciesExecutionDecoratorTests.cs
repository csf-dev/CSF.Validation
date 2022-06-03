using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using CSF.Validation.Rules;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.RuleExecution
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class ResultsForRulesWithFailedDependenciesExecutionDecoratorTests
    {
        [Test,AutoMoqData]
        public async Task ExecuteAllRulesAsyncShouldReturnAResultForEachRuleWithFailedDependencies([Frozen] IExecutesAllRules wrapped,
                                                                                                   [Frozen] IGetsRuleContext contextFactory,
                                                                                                   ResultsForRulesWithFailedDependenciesExecutionDecorator sut,
                                                                                                   IRuleExecutionContext executionContext,
                                                                                                   [ExecutableModel] ExecutableRule rule1,
                                                                                                   [ExecutableModel] ExecutableRule rule2,
                                                                                                   [RuleContext] RuleContext context)
        {
            Mock.Get(wrapped)
                .Setup(x => x.ExecuteAllRulesAsync(executionContext, default))
                .Returns(Task.FromResult<IReadOnlyCollection<ValidationRuleResult>>(Array.Empty<ValidationRuleResult>()));
            Mock.Get(executionContext)
                .Setup(x => x.GetRulesWhoseDependenciesHaveFailed())
                .Returns(new[] { rule1, rule2 });
            Mock.Get(contextFactory)
                .Setup(x => x.GetRuleContext(It.IsAny<ExecutableRule>()))
                .Returns(context);
            rule1.Result = new RuleResult(RuleOutcome.DependencyFailed);
            rule2.Result = new RuleResult(RuleOutcome.DependencyFailed);

            var results = await sut.ExecuteAllRulesAsync(executionContext);

            Assert.Multiple(() =>
            {
                Assert.That(results, Has.Count.EqualTo(2), "Count of results");
                Assert.That(results.Select(x => x.ValidationLogic).ToList(),
                            Is.EqualTo(new[] { rule1.RuleLogic, rule2.RuleLogic }),
                            "Correct rules used, identified by their logic instances.");
            });
        }
    }
}