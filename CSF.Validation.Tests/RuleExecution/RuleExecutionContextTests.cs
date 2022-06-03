using System;
using System.Collections.Generic;
using CSF.Validation.Rules;
using NUnit.Framework;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.RuleExecution
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class RuleExecutionContextTests
    {
        [Test,AutoMoqData]
        public void GetRulesWhichMayBeExecutedShouldNotReturnRulesWhichHaveUnresolvedDependencies([ExecutableModel] ExecutableRule rule1,
                                                                                                  [ExecutableModel] ExecutableRule rule2,
                                                                                                  [ExecutableModel] ExecutableRule rule3,
                                                                                                  [ExecutableModel] ExecutableRule rule4)
        {
            var rulesAndDeps = new[] {
                new ExecutableRuleAndDependencies(rule1, new[] { rule2 }),
                new ExecutableRuleAndDependencies(rule2, new[] { rule3 }, new[] { rule1 }),
                new ExecutableRuleAndDependencies(rule3, null, new[] { rule2 }),
                new ExecutableRuleAndDependencies(rule4)
            };
            var sut = GetSut(rulesAndDeps);

            Assert.That(() => sut.GetRulesWhichMayBeExecuted(), Is.EquivalentTo(new[] { rule3, rule4 }));
        }

        [Test,AutoMoqData]
        public void GetRulesWhichMayBeExecutedShouldReturnRulesWhichMayBeExecutedBecauseTheirDependenciesPassed([ExecutableModel] ExecutableRule rule1,
                                                                                                                [ExecutableModel] ExecutableRule rule2,
                                                                                                                [ExecutableModel] ExecutableRule rule3,
                                                                                                                [ExecutableModel] ExecutableRule rule4)
        {
            var rulesAndDeps = new[] {
                new ExecutableRuleAndDependencies(rule1, new[] { rule2 }),
                new ExecutableRuleAndDependencies(rule2, new[] { rule3 }, new[] { rule1 }),
                new ExecutableRuleAndDependencies(rule3, dependedUponBy: new[] { rule2 }),
                new ExecutableRuleAndDependencies(rule4)
            };
            var sut = GetSut(rulesAndDeps);
            rule3.Result = Pass();
            sut.HandleValidationRuleResult(rule3);

            Assert.That(() => sut.GetRulesWhichMayBeExecuted(), Is.EquivalentTo(new[] { rule2, rule4 }));
        }

        [Test,AutoMoqData]
        public void GetRulesWhichMayBeExecutedShouldNotReturnRulesWhichAlreadyHaveResults([ExecutableModel] ExecutableRule rule1,
                                                                                          [ExecutableModel] ExecutableRule rule2,
                                                                                          [ExecutableModel] ExecutableRule rule3,
                                                                                          [ExecutableModel] ExecutableRule rule4,
                                                                                          [ExecutableModel] ExecutableRule rule5)
        {
            rule1.Result = Pass();
            rule2.Result = Fail();
            rule3.Result = Error();
            rule4.Result = new RuleResult(RuleOutcome.DependencyFailed);
            var rulesAndDeps = new[] {
                new ExecutableRuleAndDependencies(rule1),
                new ExecutableRuleAndDependencies(rule2),
                new ExecutableRuleAndDependencies(rule3),
                new ExecutableRuleAndDependencies(rule4),
                new ExecutableRuleAndDependencies(rule5)
            };
            var sut = GetSut(rulesAndDeps);

            Assert.That(() => sut.GetRulesWhichMayBeExecuted(), Is.EquivalentTo(new[] { rule5 }));
        }

        [Test,AutoMoqData]
        public void GetRulesWhichMayBeExecutedShouldNotReturnRulesWhichHaveFailedValues([ExecutableModel] ExecutableRule rule1,
                                                                                          [ExecutableModel] ExecutableRule rule2,
                                                                                          [ExecutableModel] ExecutableRule rule3,
                                                                                          [ExecutableModel] ExecutableRule rule4,
                                                                                          [ExecutableModel] ExecutableRule rule5)
        {
            rule1.ValidatedValue.ValueResponse = new IgnoredGetValueToBeValidatedResponse();
            rule2.ValidatedValue.ValueResponse = new SuccessfulGetValueToBeValidatedResponse(new object());
            rule3.ValidatedValue.ValueResponse = new ErrorGetValueToBeValidatedResponse(new Exception());
            rule4.ValidatedValue.ValueResponse = new SuccessfulGetValueToBeValidatedResponse(new object());
            rule5.ValidatedValue.ValueResponse = new ErrorGetValueToBeValidatedResponse(new Exception());
            var rulesAndDeps = new[] {
                new ExecutableRuleAndDependencies(rule1),
                new ExecutableRuleAndDependencies(rule2),
                new ExecutableRuleAndDependencies(rule3),
                new ExecutableRuleAndDependencies(rule4),
                new ExecutableRuleAndDependencies(rule5)
            };
            var sut = GetSut(rulesAndDeps);

            Assert.That(() => sut.GetRulesWhichMayBeExecuted(), Is.EquivalentTo(new[] { rule2, rule4 }));
        }

        [Test,AutoMoqData]
        public void GetRulesWhoseDependenciesHaveFailedShouldReturnCorrectRulesAfterHandleValidationRuleResult([ExecutableModel] ExecutableRule rule1,
                                                                                                               [ExecutableModel] ExecutableRule rule2,
                                                                                                               [ExecutableModel] ExecutableRule rule3,
                                                                                                               [ExecutableModel] ExecutableRule rule4)
        {
            var rulesAndDeps = new[] {
                new ExecutableRuleAndDependencies(rule1, new[] { rule2 }),
                new ExecutableRuleAndDependencies(rule2, new[] { rule3 }, new[] { rule1 }),
                new ExecutableRuleAndDependencies(rule3, null, new[] { rule2 }),
                new ExecutableRuleAndDependencies(rule4)
            };
            var sut = GetSut(rulesAndDeps);
            rule3.Result = Fail();
            sut.HandleValidationRuleResult(rule3);

            Assert.That(() => sut.GetRulesWhoseDependenciesHaveFailed(), Is.EquivalentTo(new[] { rule2, rule1 }));
        }

        IRuleExecutionContext GetSut(IEnumerable<ExecutableRuleAndDependencies> rules)
            => new RuleExecutionContext(rules);
    }
}