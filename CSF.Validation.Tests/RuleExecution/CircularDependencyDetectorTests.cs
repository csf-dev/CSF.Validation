using System.Linq;
using NUnit.Framework;

namespace CSF.Validation.RuleExecution
{
    [TestFixture,Parallelizable]
    public class CircularDependencyDetectorTests
    {
        [Test,AutoMoqData]
        public void GetCircularDependenciesShouldReturnAnEmptyCollectionIfThereAreNoCircularDependencies([ExecutableModel] ExecutableRule rule1,
                                                                                                         [ExecutableModel] ExecutableRule rule2,
                                                                                                         [ExecutableModel] ExecutableRule rule3,
                                                                                                         [ExecutableModel] ExecutableRule rule4,
                                                                                                         CircularDependencyDetector sut)
        {
            var rulesAndDependencies = new[]
            {
                new ExecutableRuleAndDependencies(rule1, new[] { rule2, rule3 }),
                new ExecutableRuleAndDependencies(rule2),
                new ExecutableRuleAndDependencies(rule3, new[] { rule2 }),
                new ExecutableRuleAndDependencies(rule4),
            };

            Assert.That(() => sut.GetCircularDependencies(rulesAndDependencies), Is.Empty);
        }

        [Test,AutoMoqData]
        public void GetCircularDependenciesShouldReturnTwoWhenThereAreTwoCircularDependencies([ExecutableModel] ExecutableRule rule1,
                                                                                              [ExecutableModel] ExecutableRule rule2,
                                                                                              [ExecutableModel] ExecutableRule rule3,
                                                                                              [ExecutableModel] ExecutableRule rule4,
                                                                                              CircularDependencyDetector sut)
        {
            var rulesAndDependencies = new[]
            {
                new ExecutableRuleAndDependencies(rule1, new[] { rule2, }),
                new ExecutableRuleAndDependencies(rule2, new[] { rule1 }),
                new ExecutableRuleAndDependencies(rule3, new[] { rule4 }),
                new ExecutableRuleAndDependencies(rule4, new[] { rule3 }),
            };

            Assert.That(() => sut.GetCircularDependencies(rulesAndDependencies).ToList(), Has.Count.EqualTo(2));
        }

        [Test,AutoMoqData]
        public void GetCircularDependenciesShouldReturnACircularDependencyWithTheCorrectChainOfRules([ExecutableModel] ExecutableRule rule1,
                                                                                                     [ExecutableModel] ExecutableRule rule2,
                                                                                                     [ExecutableModel] ExecutableRule rule3,
                                                                                                     [ExecutableModel] ExecutableRule rule4,
                                                                                                     CircularDependencyDetector sut)
        {
            var rulesAndDependencies = new[]
            {
                new ExecutableRuleAndDependencies(rule1, new[] { rule2, }),
                new ExecutableRuleAndDependencies(rule2, new[] { rule3 }),
                new ExecutableRuleAndDependencies(rule3, new[] { rule4 }),
                new ExecutableRuleAndDependencies(rule4, new[] { rule1 }),
            };

            Assert.That(() => sut.GetCircularDependencies(rulesAndDependencies).Single().DependencyChain, Is.EqualTo(new [] { rule1, rule2, rule3, rule4, rule1 }));
        }
    }
}