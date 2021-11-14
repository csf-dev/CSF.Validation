using System.Collections.Generic;
using CSF.Validation.Rules;
using NUnit.Framework;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.RuleExecution
{
    [TestFixture,Parallelizable]
    public class RuleDependencyTrackerTests
    {
        [Test,AutoMoqData]
        public void GetRulesWhichMayBeExecutedShouldNotReturnRulesWhichHaveUnresolvedDependencies([ExecutableModel] ExecutableRule rule1,
                                                                                                  [ExecutableModel] ExecutableRule rule2,
                                                                                                  [ExecutableModel] ExecutableRule rule3,
                                                                                                  [ExecutableModel] ExecutableRule rule4,
                                                                                                  ValidationOptions options)
        {
            var rulesAndDeps = new[] {
                new ExecutableRuleAndDependencies(rule1, new[] { rule2 }),
                new ExecutableRuleAndDependencies(rule2, new[] { rule3 }, new[] { rule1 }),
                new ExecutableRuleAndDependencies(rule3, null, new[] { rule2 }),
                new ExecutableRuleAndDependencies(rule4)
            };
            var sut = GetSut(rulesAndDeps, options);

            Assert.That(() => sut.GetRulesWhichMayBeExecuted(), Is.EquivalentTo(new[] { rule3, rule4 }));
        }

        [Test,AutoMoqData]
        public void GetRulesWhichMayBeExecutedShouldReturnRulesWhichMayBeExecutedBecauseTheirDependenciesPassed([ExecutableModel] ExecutableRule rule1,
                                                                                                                [ExecutableModel] ExecutableRule rule2,
                                                                                                                [ExecutableModel] ExecutableRule rule3,
                                                                                                                [ExecutableModel] ExecutableRule rule4,
                                                                                                                ValidationOptions options)
        {
            var rulesAndDeps = new[] {
                new ExecutableRuleAndDependencies(rule1, new[] { rule2 }),
                new ExecutableRuleAndDependencies(rule2, new[] { rule3 }, new[] { rule1 }),
                new ExecutableRuleAndDependencies(rule3, dependedUponBy: new[] { rule2 }),
                new ExecutableRuleAndDependencies(rule4)
            };
            var sut = GetSut(rulesAndDeps, options);
            rule3.Result = Pass();
            sut.HandleValidationRuleResult(rule3);

            Assert.That(() => sut.GetRulesWhichMayBeExecuted(), Is.EquivalentTo(new[] { rule2, rule4 }));
        }

        [Test,AutoMoqData]
        public void GetRulesWhichMayBeExecutedShouldNotReturnRulesWhichAlreadyHaveResults([ExecutableModel] ExecutableRule rule1,
                                                                                          [ExecutableModel] ExecutableRule rule2,
                                                                                          [ExecutableModel] ExecutableRule rule3,
                                                                                          [ExecutableModel] ExecutableRule rule4,
                                                                                          [ExecutableModel] ExecutableRule rule5,
                                                                                          ValidationOptions options)
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
            var sut = GetSut(rulesAndDeps, options);

            Assert.That(() => sut.GetRulesWhichMayBeExecuted(), Is.EquivalentTo(new[] { rule5 }));
        }

        [Test,AutoMoqData]
        public void GetRulesWhoseDependenciesHaveFailedShouldReturnCorrectRulesAfterHandleValidationRuleResult([ExecutableModel] ExecutableRule rule1,
                                                                                                               [ExecutableModel] ExecutableRule rule2,
                                                                                                               [ExecutableModel] ExecutableRule rule3,
                                                                                                               [ExecutableModel] ExecutableRule rule4,
                                                                                                               ValidationOptions options)
        {
            var rulesAndDeps = new[] {
                new ExecutableRuleAndDependencies(rule1, new[] { rule2 }),
                new ExecutableRuleAndDependencies(rule2, new[] { rule3 }, new[] { rule1 }),
                new ExecutableRuleAndDependencies(rule3, null, new[] { rule2 }),
                new ExecutableRuleAndDependencies(rule4)
            };
            var sut = GetSut(rulesAndDeps, options);
            rule3.Result = Fail();
            sut.HandleValidationRuleResult(rule3);

            Assert.That(() => sut.GetRulesWhoseDependenciesHaveFailed(), Is.EquivalentTo(new[] { rule2, rule1 }));
        }

        ITracksRuleDependencies GetSut(IEnumerable<ExecutableRuleAndDependencies> rules, ValidationOptions options)
            => new RuleDependencyTracker(rules, options);
    }
}