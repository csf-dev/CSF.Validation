using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using CSF.Validation.Rules;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.RuleExecution
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class ResultsForErroredValuesExecutionDecoratorTests
    {

        [Test,AutoMoqData]
        public async Task ExecuteAllRulesAsyncShouldIncludeErrorResultsForEachErroredValue(IRuleExecutionContext executionContext,
                                                                                           [Frozen] IExecutesAllRules wrapped,
                                                                                           ResultsForErroredValuesExecutionDecorator sut,
                                                                                           ErrorGetValueToBeValidatedResponse response1,
                                                                                           ErrorGetValueToBeValidatedResponse response2,
                                                                                           [ExecutableModel] ExecutableRule rule1,
                                                                                           [ExecutableModel] ExecutableRule rule2,
                                                                                           [ExecutableModel] ExecutableRule rule3)
        {
            Mock.Get(wrapped)
                .Setup(x => x.ExecuteAllRulesAsync(executionContext, default))
                .Returns(Task.FromResult<IReadOnlyCollection<ValidationRuleResult>>(Array.Empty<ValidationRuleResult>()));
            var ruleAndDependencies1 = new ExecutableRuleAndDependencies(rule1);
            var ruleAndDependencies2 = new ExecutableRuleAndDependencies(rule2);
            var ruleAndDependencies3 = new ExecutableRuleAndDependencies(rule3);
            rule1.ValidatedValue.ValueResponse = response1;
            rule2.ValidatedValue.ValueResponse = response1;
            rule3.ValidatedValue.ValueResponse = response2;
            Mock.Get(executionContext)
                .SetupGet(x => x.AllRules)
                .Returns(new[] { ruleAndDependencies1, ruleAndDependencies2, ruleAndDependencies3 });

            var result = await sut.ExecuteAllRulesAsync(executionContext);

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
        public async Task ExecuteAllRulesAsyncShouldNotIncludeErrorResultsForIgnoredValues(IRuleExecutionContext executionContext,
                                                                                           [Frozen] IExecutesAllRules wrapped,
                                                                                           ResultsForErroredValuesExecutionDecorator sut,
                                                                                           ErrorGetValueToBeValidatedResponse response1,
                                                                                           IgnoredGetValueToBeValidatedResponse response2,
                                                                                           [ExecutableModel] ExecutableRule rule1,
                                                                                           [ExecutableModel] ExecutableRule rule2,
                                                                                           [ExecutableModel] ExecutableRule rule3)
        {
            Mock.Get(wrapped)
                .Setup(x => x.ExecuteAllRulesAsync(executionContext, default))
                .Returns(Task.FromResult<IReadOnlyCollection<ValidationRuleResult>>(Array.Empty<ValidationRuleResult>()));
            var ruleAndDependencies1 = new ExecutableRuleAndDependencies(rule1);
            var ruleAndDependencies2 = new ExecutableRuleAndDependencies(rule2);
            var ruleAndDependencies3 = new ExecutableRuleAndDependencies(rule3);
            rule1.ValidatedValue.ValueResponse = response1;
            rule2.ValidatedValue.ValueResponse = response1;
            rule3.ValidatedValue.ValueResponse = response2;
            Mock.Get(executionContext)
                .SetupGet(x => x.AllRules)
                .Returns(new[] { ruleAndDependencies1, ruleAndDependencies2, ruleAndDependencies3 });

            var result = await sut.ExecuteAllRulesAsync(executionContext);

            Assert.Multiple(() =>
            {
                Assert.That(result,
                            Has.One.Matches<ValidationRuleResult>(v => v.Outcome == RuleOutcome.Errored && v.Exception == response1.Exception),
                            "First response present");
                Assert.That(result, Has.Count.EqualTo(1), "Correct count");
            });
        }
    }
}