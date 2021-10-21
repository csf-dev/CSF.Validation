using System;
using System.Collections.Generic;
using System.Linq;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// A service which detects circular dependencies between executable validation rules.
    /// </summary>
    public class CircularDependencyDetector : IDetectsCircularDependencies
    {
        /// <summary>
        /// Gets a collection of any circular dependencies which are detected.
        /// </summary>
        /// <param name="rulesAndDependencies">A collection of the all of the rules and dependencies</param>
        /// <returns>A collection of circular dependency models, indicating the circular dependencies detected.</returns>
        public IEnumerable<CircularDependency> GetCircularDependencies(IEnumerable<ExecutableRuleAndDependencies> rulesAndDependencies)
        {
            var candidates = new CandidateRulesCollection(rulesAndDependencies);

            while(candidates.TryGetNext(out var current))
            {
                var circularDependency = TryGetCircularDependency(current, candidates);
                if(circularDependency == null) continue;

                candidates.CloseFoundCircularDependency(circularDependency);
                yield return circularDependency;
            }
        }

        static CircularDependency TryGetCircularDependency(ExecutableRuleAndDependencies ruleAndDependencies,
                                                           CandidateRulesCollection remainingCandidates)
        {
            // This optimisation probably isn't required but it makes it
            // incredibly clear that for rules with no dependencies, there's no work to do.
            if(!ruleAndDependencies.Dependencies.Any())
                return null;

            var firstDependencies = ruleAndDependencies.Dependencies
                .Select(x => (x, new List<ExecutableRule> { ruleAndDependencies.ExecutableRule, x }));
            var openList = new Queue<(ExecutableRule,List<ExecutableRule>)>(firstDependencies);

            while(openList.Any())
            {
                var (current, stack) = openList.Dequeue();

                if (current == ruleAndDependencies.ExecutableRule)
                    return new CircularDependency { DependencyChain = stack.ToList() };

                var dependencies = remainingCandidates.GetDependencies(current);
                foreach(var dependency in dependencies)
                {
                    var dependencyStack = new List<ExecutableRule>(stack);
                    dependencyStack.Add(dependency);
                    openList.Enqueue((dependency, dependencyStack));
                }
            }

            return null;
        }

        class CandidateRulesCollection
        {
            readonly Dictionary<ExecutableRule, ExecutableRuleAndDependencies> allRulesAndDependencies;
            readonly HashSet<ExecutableRuleAndDependencies> openList;

            internal bool TryGetNext(out ExecutableRuleAndDependencies next)
            {
                if(openList.Count == 0)
                {
                    next = null;
                    return false;
                }

                next = openList.First();
                openList.Remove(next);
                return true;
            }

            internal void CloseFoundCircularDependency(CircularDependency circularDependency)
            {
                foreach(var rule in circularDependency.DependencyChain)
                    openList.Remove(allRulesAndDependencies[rule]);
            }

            internal IEnumerable<ExecutableRule> GetDependencies(ExecutableRule rule)
                => allRulesAndDependencies[rule].Dependencies;

            internal CandidateRulesCollection(IEnumerable<ExecutableRuleAndDependencies> allRulesAndDependencies)
            {
                this.allRulesAndDependencies = allRulesAndDependencies.ToDictionary(k => k.ExecutableRule, v => v);
                this.openList = new HashSet<ExecutableRuleAndDependencies>(allRulesAndDependencies);
            }
        }
    }
}