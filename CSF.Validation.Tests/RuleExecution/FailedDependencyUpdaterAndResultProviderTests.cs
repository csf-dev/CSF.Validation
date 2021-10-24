using System.Linq;
using CSF.Validation.Rules;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.RuleExecution
{
    [TestFixture,Parallelizable]
    public class FailedDependencyUpdaterAndResultProviderTests
    {
        [Test,AutoMoqData]
        public void GetResultsAndUpdateRulesShouldReturnDependencyFailureResultsForAllAppropriateRules(FailedDependencyUpdaterAndResultProvider sut,
                                                                                                       ITracksRuleDependencies tracker,
                                                                                                       [ExecutableModel] ExecutableRule rule1,
                                                                                                       [ExecutableModel] ExecutableRule rule2)
        {
            Mock.Get(tracker).Setup(x => x.GetRulesWhoseDependenciesHaveFailed()).Returns(new[] { rule1, rule2 });

            var result = sut.GetResultsAndUpdateRules(tracker);

            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Exactly(2).Matches<ValidationRuleResult>(r => r.Outcome == Rules.RuleOutcome.DependencyFailed), nameof(ValidationRuleResult.Outcome));
                Assert.That(result.Select(x => x.Identifier), Is.EquivalentTo(new[] { rule1, rule2 }.Select(x => x.RuleIdentifier)), "Rule identifiers upon results are correct");
            });
        }

        [Test, AutoMoqData]
        public void GetResultsAndUpdateRulesShouldUpdateAllAppropriateRulesWithResults(FailedDependencyUpdaterAndResultProvider sut,
                                                                                       ITracksRuleDependencies tracker,
                                                                                       [ExecutableModel] ExecutableRule rule1,
                                                                                       [ExecutableModel] ExecutableRule rule2)
        {
            Mock.Get(tracker).Setup(x => x.GetRulesWhoseDependenciesHaveFailed()).Returns(new[] { rule1, rule2 });
            
            sut.GetResultsAndUpdateRules(tracker);

            Assert.Multiple(() =>
            {
                Assert.That(rule1.Result?.Outcome, Is.EqualTo(RuleOutcome.DependencyFailed), "Rule 1 result");
                Assert.That(rule2.Result?.Outcome, Is.EqualTo(RuleOutcome.DependencyFailed), "Rule 2 result");
            });
        }
    }
}