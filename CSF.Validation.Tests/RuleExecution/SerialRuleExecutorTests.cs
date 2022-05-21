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
    [TestFixture,Parallelizable]
    public class SerialRuleExecutorTests
    {
        [Test,AutoMoqData]
        public void ExecuteAllRulesAsyncShouldReturnResultsForAllRules([Frozen] IGetsRuleDependencyTracker dependencyTrackerFactory,
                                                                       [Frozen] IGetsSingleRuleExecutor ruleExecutorFactory,
                                                                       [Frozen] ValidationOptions options,
                                                                       SerialRuleExecutor sut,
                                                                       [ExecutableModel] ExecutableRuleAndDependencies rule1,
                                                                       [ExecutableModel] ExecutableRuleAndDependencies rule2,
                                                                       [ExecutableModel] ExecutableRuleAndDependencies rule3,
                                                                       ITracksRuleDependencies dependencyTracker,
                                                                       IExeucutesSingleRule ruleExecutor,
                                                                       [RuleResult] ValidationRuleResult result1,
                                                                       [RuleResult] ValidationRuleResult result2,
                                                                       [RuleResult] ValidationRuleResult result3)
        {
            var allRules = new[] { rule1, rule2, rule3 };
            Mock.Get(dependencyTrackerFactory).Setup(x => x.GetDependencyTracker(allRules, options)).Returns(dependencyTracker);
            var sequence = new MockSequence();
            Mock.Get(dependencyTracker).InSequence(sequence).Setup(x => x.GetRulesWhichMayBeExecuted()).Returns(() => allRules.Select(r => r.ExecutableRule));
            Mock.Get(dependencyTracker).InSequence(sequence).Setup(x => x.GetRulesWhichMayBeExecuted()).Returns(() => Enumerable.Empty<ExecutableRule>());
            Mock.Get(ruleExecutorFactory).Setup(x => x.GetRuleExecutor(options)).Returns(ruleExecutor);
            Mock.Get(ruleExecutor)
                .Setup(x => x.ExecuteRuleAsync(rule1.ExecutableRule, It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(result1));
            Mock.Get(ruleExecutor)
                .Setup(x => x.ExecuteRuleAsync(rule2.ExecutableRule, It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(result2));
            Mock.Get(ruleExecutor)
                .Setup(x => x.ExecuteRuleAsync(rule3.ExecutableRule, It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(result3));

            Assert.That(async () => await sut.ExecuteAllRulesAsync(allRules, default),
                        Is.EquivalentTo(new[] { result1, result2, result3 }));
        }

        [Test,AutoMoqData]
        public async Task ExecuteAllRulesAsyncShouldUpdateTheResultUponTheRule([Frozen] IGetsRuleDependencyTracker dependencyTrackerFactory,
                                                                               [Frozen] IGetsSingleRuleExecutor ruleExecutorFactory,
                                                                               [Frozen] ValidationOptions options,
                                                                               SerialRuleExecutor sut,
                                                                               [ExecutableModel] ExecutableRuleAndDependencies rule,
                                                                               ITracksRuleDependencies dependencyTracker,
                                                                               IExeucutesSingleRule ruleExecutor,
                                                                               [RuleResult] ValidationRuleResult result)
        {
            var allRules = new[] { rule };
            Mock.Get(dependencyTrackerFactory).Setup(x => x.GetDependencyTracker(allRules, options)).Returns(dependencyTracker);
            var sequence = new MockSequence();
            Mock.Get(dependencyTracker).InSequence(sequence).Setup(x => x.GetRulesWhichMayBeExecuted()).Returns(() => allRules.Select(r => r.ExecutableRule));
            Mock.Get(dependencyTracker).InSequence(sequence).Setup(x => x.GetRulesWhichMayBeExecuted()).Returns(() => Enumerable.Empty<ExecutableRule>());
            Mock.Get(ruleExecutorFactory).Setup(x => x.GetRuleExecutor(options)).Returns(ruleExecutor);
            Mock.Get(ruleExecutor)
                .Setup(x => x.ExecuteRuleAsync(rule.ExecutableRule, It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(result));

            await sut.ExecuteAllRulesAsync(allRules);

            Assert.That(rule.ExecutableRule.Result, Is.SameAs(result));
        }

        // This rule has a timeout of 1/10th a second because there is a risk of an infinite loop if something is wrong.
        // This way it fails quickly if it's broken and doesn't delay the test run.
        [Test,AutoMoqData,Timeout(100)]
        public void ExecuteAllRulesAsyncShouldNotExecuteDependantRulesBeforeTheirDependenciesHaveResults([Frozen] IGetsRuleDependencyTracker dependencyTrackerFactory,
                                                                                                         [Frozen] IGetsSingleRuleExecutor ruleExecutorFactory,
                                                                                                         [Frozen] ValidationOptions options,
                                                                                                         SerialRuleExecutor sut,
                                                                                                         [ExecutableModel] ExecutableRuleAndDependencies rule1,
                                                                                                         [ExecutableModel] ExecutableRuleAndDependencies rule2,
                                                                                                         [ExecutableModel] ExecutableRuleAndDependencies rule3,
                                                                                                         ITracksRuleDependencies dependencyTracker,
                                                                                                         IExeucutesSingleRule ruleExecutor,
                                                                                                         [RuleResult] ValidationRuleResult result1,
                                                                                                         [RuleResult] ValidationRuleResult result2,
                                                                                                         [RuleResult] ValidationRuleResult result3)
        {
            var allRules = new[] { rule3, rule2, rule1 };
            Mock.Get(dependencyTrackerFactory).Setup(x => x.GetDependencyTracker(allRules, options)).Returns(dependencyTracker);
            Mock.Get(dependencyTracker)
                .Setup(x => x.GetRulesWhichMayBeExecuted())
                .Returns(() =>
                {
                    if(rule1.ExecutableRule.Result is null) return new[] { rule1.ExecutableRule };
                    if(rule2.ExecutableRule.Result is null) return new[] { rule2.ExecutableRule };
                    if(rule3.ExecutableRule.Result is null) return new[] { rule3.ExecutableRule };
                    return Enumerable.Empty<ExecutableRule>();
                });
            Mock.Get(ruleExecutorFactory).Setup(x => x.GetRuleExecutor(options)).Returns(ruleExecutor);
            Mock.Get(ruleExecutor)
                .Setup(x => x.ExecuteRuleAsync(rule1.ExecutableRule, It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(result1));
            Mock.Get(ruleExecutor)
                .Setup(x => x.ExecuteRuleAsync(rule2.ExecutableRule, It.IsAny<CancellationToken>()))
                .Callback(() =>
                {
                    if(rule1.ExecutableRule.Result is null)
                        Assert.Fail("Rule 2 should not be executed before rule 1");
                })
                .Returns(() => Task.FromResult(result2));
            Mock.Get(ruleExecutor)
                .Setup(x => x.ExecuteRuleAsync(rule3.ExecutableRule, It.IsAny<CancellationToken>()))
                .Callback(() =>
                {
                    if(rule1.ExecutableRule.Result is null || rule2.ExecutableRule.Result is null)
                        Assert.Fail("Rule 3 should not be executed before rules 1 or 2");
                })
                .Returns(() => Task.FromResult(result3));

            Assert.That(async () => await sut.ExecuteAllRulesAsync(allRules, default),
                        Is.EquivalentTo(new[] { result1, result2, result3 }));
        }

        [Test,AutoMoqData]
        public async Task ExecuteAllRulesAsyncShouldIncludeErrorResultsForEachErroredValue([Frozen] IGetsRuleDependencyTracker dependencyTrackerFactory,
                                                                                           ITracksRuleDependencies dependencyTracker,
                                                                                           SerialRuleExecutor sut,
                                                                                           ErrorGetValueToBeValidatedResponse response1,
                                                                                           ErrorGetValueToBeValidatedResponse response2,
                                                                                           [ExecutableModel] ExecutableRule rule1,
                                                                                           [ExecutableModel] ExecutableRule rule2,
                                                                                           [ExecutableModel] ExecutableRule rule3)
        {
            Mock.Get(dependencyTrackerFactory)
                .Setup(x => x.GetDependencyTracker(It.IsAny<IEnumerable<ExecutableRuleAndDependencies>>(), It.IsAny<ValidationOptions>()))
                .Returns(dependencyTracker);
            Mock.Get(dependencyTracker)
                .Setup(x => x.GetRulesWhichMayBeExecuted())
                .Returns(Enumerable.Empty<ExecutableRule>());
            Mock.Get(dependencyTracker)
                .Setup(x => x.GetRulesWhoseDependenciesHaveFailed())
                .Returns(Enumerable.Empty<ExecutableRule>());
            var ruleAndDependencies1 = new ExecutableRuleAndDependencies(rule1);
            var ruleAndDependencies2 = new ExecutableRuleAndDependencies(rule2);
            var ruleAndDependencies3 = new ExecutableRuleAndDependencies(rule3);
            rule1.ValidatedValue.ValueResponse = response1;
            rule2.ValidatedValue.ValueResponse = response1;
            rule3.ValidatedValue.ValueResponse = response2;

            var result = await sut.ExecuteAllRulesAsync(new[] { ruleAndDependencies1, ruleAndDependencies2, ruleAndDependencies3 });

            Assert.Multiple(() =>
            {
                Assert.That(result,
                            Has.One.Matches<ValidationRuleResult>(v => v.Outcome == RuleOutcome.Errored && v.Exception == response1.Exception),
                            "First response present");
                Assert.That(result,
                            Has.One.Matches<ValidationRuleResult>(v => v.Outcome == RuleOutcome.Errored && v.Exception == response2.Exception),
                            "second response present");
            });
        }

        [Test,AutoMoqData]
        public async Task ExecuteAllRulesAsyncShouldNotIncludeErrorResultsForIgnoredValues([Frozen] IGetsRuleDependencyTracker dependencyTrackerFactory,
                                                                                           ITracksRuleDependencies dependencyTracker,
                                                                                           SerialRuleExecutor sut,
                                                                                           ErrorGetValueToBeValidatedResponse response1,
                                                                                           IgnoredGetValueToBeValidatedResponse response2,
                                                                                           [ExecutableModel] ExecutableRule rule1,
                                                                                           [ExecutableModel] ExecutableRule rule2,
                                                                                           [ExecutableModel] ExecutableRule rule3)
        {
            Mock.Get(dependencyTrackerFactory)
                .Setup(x => x.GetDependencyTracker(It.IsAny<IEnumerable<ExecutableRuleAndDependencies>>(), It.IsAny<ValidationOptions>()))
                .Returns(dependencyTracker);
            Mock.Get(dependencyTracker)
                .Setup(x => x.GetRulesWhichMayBeExecuted())
                .Returns(Enumerable.Empty<ExecutableRule>());
            Mock.Get(dependencyTracker)
                .Setup(x => x.GetRulesWhoseDependenciesHaveFailed())
                .Returns(Enumerable.Empty<ExecutableRule>());
            var ruleAndDependencies1 = new ExecutableRuleAndDependencies(rule1);
            var ruleAndDependencies2 = new ExecutableRuleAndDependencies(rule2);
            var ruleAndDependencies3 = new ExecutableRuleAndDependencies(rule3);
            rule1.ValidatedValue.ValueResponse = response1;
            rule2.ValidatedValue.ValueResponse = response1;
            rule3.ValidatedValue.ValueResponse = response2;

            var result = await sut.ExecuteAllRulesAsync(new[] { ruleAndDependencies1, ruleAndDependencies2, ruleAndDependencies3 });

            Assert.Multiple(() =>
            {
                Assert.That(result,
                            Has.One.Matches<ValidationRuleResult>(v => v.Outcome == RuleOutcome.Errored && v.Exception == response1.Exception),
                            "First response present");
                Assert.That(result, Has.Count.EqualTo(1), "Correct count");
            });
        }

        [Test, AutoMoqData]
        public async Task ExecuteAllRulesAsyncShouldNotExecuteRulesAssociatedWithUnsuccessfulValues([Frozen] IGetsRuleDependencyTracker dependencyTrackerFactory,
                                                                                                    [Frozen] IGetsSingleRuleExecutor ruleExecutorFactory,
                                                                                                    ITracksRuleDependencies dependencyTracker,
                                                                                                    IExeucutesSingleRule singleRuleExecutor,
                                                                                                    SerialRuleExecutor sut,
                                                                                                    IgnoredGetValueToBeValidatedResponse response1,
                                                                                                    IgnoredGetValueToBeValidatedResponse response2,
                                                                                                    [ExecutableModel] ExecutableRule rule1,
                                                                                                    [ExecutableModel] ExecutableRule rule2,
                                                                                                    [ExecutableModel] ExecutableRule rule3)
        {
            Mock.Get(dependencyTrackerFactory)
                .Setup(x => x.GetDependencyTracker(It.IsAny<IEnumerable<ExecutableRuleAndDependencies>>(), It.IsAny<ValidationOptions>()))
                .Returns(dependencyTracker);
            Mock.Get(ruleExecutorFactory)
                .Setup(x => x.GetRuleExecutor(It.IsAny<ValidationOptions>()))
                .Returns(singleRuleExecutor);
            var sequence = new MockSequence();
            Mock.Get(dependencyTracker)
                .InSequence(sequence)
                .Setup(x => x.GetRulesWhichMayBeExecuted())
                .Returns(new [] {rule1});
            Mock.Get(dependencyTracker)
                .InSequence(sequence)
                .Setup(x => x.GetRulesWhichMayBeExecuted())
                .Returns(Enumerable.Empty<ExecutableRule>());
            Mock.Get(dependencyTracker)
                .Setup(x => x.GetRulesWhoseDependenciesHaveFailed())
                .Returns(Enumerable.Empty<ExecutableRule>());
            var ruleAndDependencies1 = new ExecutableRuleAndDependencies(rule1);
            var ruleAndDependencies2 = new ExecutableRuleAndDependencies(rule2);
            var ruleAndDependencies3 = new ExecutableRuleAndDependencies(rule3);
            rule1.ValidatedValue.ValueResponse = response1;
            rule2.ValidatedValue.ValueResponse = response1;
            rule3.ValidatedValue.ValueResponse = response2;

            var result = await sut.ExecuteAllRulesAsync(new[] { ruleAndDependencies1, ruleAndDependencies2, ruleAndDependencies3 });

            Mock.Get(singleRuleExecutor)
                .Verify(x => x.ExecuteRuleAsync(It.IsAny<ExecutableRule>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}