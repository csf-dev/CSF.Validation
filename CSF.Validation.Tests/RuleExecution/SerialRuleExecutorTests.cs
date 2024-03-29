using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using CSF.Validation.Rules;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.RuleExecution
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class SerialRuleExecutorTests
    {
        [Test,AutoMoqData]
        public void ExecuteAllRulesAsyncShouldReturnResultsForAllRules([Frozen] IExeucutesSingleRule ruleExecutor,
                                                                       SerialRuleExecutor sut,
                                                                       [ExecutableModel] ExecutableRuleAndDependencies rule1,
                                                                       [ExecutableModel] ExecutableRuleAndDependencies rule2,
                                                                       [ExecutableModel] ExecutableRuleAndDependencies rule3,
                                                                       IRuleExecutionContext executionContext,
                                                                       [RuleResult] ValidationRuleResult result1,
                                                                       [RuleResult] ValidationRuleResult result2,
                                                                       [RuleResult] ValidationRuleResult result3)
        {
            var allRules = new[] { rule1, rule2, rule3 };
            var sequence = new MockSequence();
            Mock.Get(executionContext).InSequence(sequence).Setup(x => x.GetRulesWhichMayBeExecuted()).Returns(() => allRules.Select(r => r.ExecutableRule));
            Mock.Get(executionContext).InSequence(sequence).Setup(x => x.GetRulesWhichMayBeExecuted()).Returns(() => Enumerable.Empty<ExecutableRule>());
            Mock.Get(ruleExecutor)
                .Setup(x => x.ExecuteRuleAsync(rule1.ExecutableRule, It.IsAny<CancellationToken>()))
                .Returns(() => new ValueTask<ValidationRuleResult>(result1));
            Mock.Get(ruleExecutor)
                .Setup(x => x.ExecuteRuleAsync(rule2.ExecutableRule, It.IsAny<CancellationToken>()))
                .Returns(() => new ValueTask<ValidationRuleResult>(result2));
            Mock.Get(ruleExecutor)
                .Setup(x => x.ExecuteRuleAsync(rule3.ExecutableRule, It.IsAny<CancellationToken>()))
                .Returns(() => new ValueTask<ValidationRuleResult>(result3));

            Assert.That(async () => await sut.ExecuteAllRulesAsync(executionContext, default),
                        Is.EquivalentTo(new[] { result1, result2, result3 }));
        }

        [Test,AutoMoqData]
        public async Task ExecuteAllRulesAsyncShouldUpdateTheResultUponTheRule([Frozen] IExeucutesSingleRule ruleExecutor,
                                                                               SerialRuleExecutor sut,
                                                                               [ExecutableModel] ExecutableRuleAndDependencies rule,
                                                                               IRuleExecutionContext executionContext,
                                                                               [RuleResult] ValidationRuleResult result)
        {
            var allRules = new[] { rule };
            var sequence = new MockSequence();
            Mock.Get(executionContext).InSequence(sequence).Setup(x => x.GetRulesWhichMayBeExecuted()).Returns(() => allRules.Select(r => r.ExecutableRule));
            Mock.Get(executionContext).InSequence(sequence).Setup(x => x.GetRulesWhichMayBeExecuted()).Returns(() => Enumerable.Empty<ExecutableRule>());
            Mock.Get(ruleExecutor)
                .Setup(x => x.ExecuteRuleAsync(rule.ExecutableRule, It.IsAny<CancellationToken>()))
                .Returns(() => new ValueTask<ValidationRuleResult>(result));

            await sut.ExecuteAllRulesAsync(executionContext);

            Assert.That(rule.ExecutableRule.Result, Is.SameAs(result));
        }

        // This rule has a timeout of 1/10th a second because there is a risk of an infinite loop if something is wrong.
        // This way it fails quickly if it's broken and doesn't delay the test run.
        [Test,AutoMoqData,Timeout(100)]
        public void ExecuteAllRulesAsyncShouldNotExecuteDependantRulesBeforeTheirDependenciesHaveResults([Frozen] IExeucutesSingleRule ruleExecutor,
                                                                                                         SerialRuleExecutor sut,
                                                                                                         [ExecutableModel] ExecutableRuleAndDependencies rule1,
                                                                                                         [ExecutableModel] ExecutableRuleAndDependencies rule2,
                                                                                                         [ExecutableModel] ExecutableRuleAndDependencies rule3,
                                                                                                         IRuleExecutionContext executionContext,
                                                                                                         [RuleResult] ValidationRuleResult result1,
                                                                                                         [RuleResult] ValidationRuleResult result2,
                                                                                                         [RuleResult] ValidationRuleResult result3)
        {
            var allRules = new[] { rule3, rule2, rule1 };
            Mock.Get(executionContext)
                .Setup(x => x.GetRulesWhichMayBeExecuted())
                .Returns(() =>
                {
                    if(rule1.ExecutableRule.Result is null) return new[] { rule1.ExecutableRule };
                    if(rule2.ExecutableRule.Result is null) return new[] { rule2.ExecutableRule };
                    if(rule3.ExecutableRule.Result is null) return new[] { rule3.ExecutableRule };
                    return Enumerable.Empty<ExecutableRule>();
                });
            Mock.Get(ruleExecutor)
                .Setup(x => x.ExecuteRuleAsync(rule1.ExecutableRule, It.IsAny<CancellationToken>()))
                .Returns(() => new ValueTask<ValidationRuleResult>(result1));
            Mock.Get(ruleExecutor)
                .Setup(x => x.ExecuteRuleAsync(rule2.ExecutableRule, It.IsAny<CancellationToken>()))
                .Callback(() =>
                {
                    if(rule1.ExecutableRule.Result is null)
                        Assert.Fail("Rule 2 should not be executed before rule 1");
                })
                .Returns(() => new ValueTask<ValidationRuleResult>(result2));
            Mock.Get(ruleExecutor)
                .Setup(x => x.ExecuteRuleAsync(rule3.ExecutableRule, It.IsAny<CancellationToken>()))
                .Callback(() =>
                {
                    if(rule1.ExecutableRule.Result is null || rule2.ExecutableRule.Result is null)
                        Assert.Fail("Rule 3 should not be executed before rules 1 or 2");
                })
                .Returns(() => new ValueTask<ValidationRuleResult>(result3));

            Assert.That(async () => await sut.ExecuteAllRulesAsync(executionContext, default),
                        Is.EquivalentTo(new[] { result1, result2, result3 }));
        }
    }
}