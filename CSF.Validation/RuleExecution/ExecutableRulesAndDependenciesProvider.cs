using System;
using System.Collections.Generic;
using System.Linq;
using CSF.Validation.Manifest;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// Service which gets a collection of executable rules matched with the dependencies for those rules.
    /// </summary>
    public class ExecutableRulesAndDependenciesProvider : IGetsAllExecutableRulesWithDependencies
    {
        readonly IGetsAllExecutableRules executableRulesProvider;

        /// <summary>
        /// Gets a collection of the executable rules and their dependencies from the specified
        /// manifest value and an object to be validated.
        /// </summary>
        /// <param name="manifestValue">The manifest value.</param>
        /// <param name="objectToBeValidated">The object to be validated.</param>
        /// <returns>A collection of all of the executable rules and the dependencies for each rule.</returns>
        public IReadOnlyList<ExecutableRuleAndDependencies> GetRulesWithDependencies(ManifestValue manifestValue, object objectToBeValidated)
        {
            var executableRules = executableRulesProvider.GetExecutableRules(manifestValue, objectToBeValidated);
            return GetRulesWithDependencies(executableRules).ToList();
        }

        static IEnumerable<ExecutableRuleAndDependencies> GetRulesWithDependencies(IEnumerable<ExecutableRule> allRules)
        {
            return from rule in allRules
                   let dependencies = GetDependencies(rule)
                   select new ExecutableRuleAndDependencies(rule, dependencies);
        }

        static IEnumerable<ExecutableRule> GetDependencies(ExecutableRule rule)
        {
            return (from dependencyId in rule.ManifestRule.DependencyRules
                    select GetDependency(dependencyId, rule))
                .ToList();
        }

        static ExecutableRule GetDependency(ManifestRuleIdentifier dependencyIdentifier, ExecutableRule rule)
        {
            var matchingValidatedValue = GetCandidateValidatedValueMatches(dependencyIdentifier, rule)
                .FirstOrDefault(x => !(x is null));

            if(matchingValidatedValue is null)
            {
                var message = String.Format(Resources.ExceptionMessages.GetExceptionMessage("CannotGetMatchingValueForDependency"),
                                            rule.ManifestRule.Identifier,
                                            dependencyIdentifier);
                throw new ValidationException(message);
            }

            var dependency = matchingValidatedValue.Rules
                .FirstOrDefault(valueRule => Equals(valueRule.ManifestRule.Identifier, dependencyIdentifier));

            if(dependency == null)
            {
                var message = String.Format(Resources.ExceptionMessages.GetExceptionMessage("DependencyRuleNotOnMatchingValue"),
                                            rule.ManifestRule.Identifier,
                                            dependencyIdentifier);
                throw new ValidationException(message);
            }

            return dependency;
        }

        static IEnumerable<ValidatedValue> GetCandidateValidatedValueMatches(ManifestRuleIdentifier dependencyIdentifier, ExecutableRule rule)
        {
            for (var currentValue = rule.ValidatedValue; currentValue != null; currentValue = currentValue.ParentValue)
                yield return GetMatchingValidatedValue(dependencyIdentifier, currentValue);
        }

        static ValidatedValue GetMatchingValidatedValue(ManifestRuleIdentifier dependencyIdentifier, ValidatedValue validatedValue)
        {
            if(Equals(validatedValue.ManifestValue, dependencyIdentifier.ManifestValue))
                return validatedValue;

            return validatedValue.ChildValues
                .FirstOrDefault(child => Equals(child.ManifestValue, dependencyIdentifier.ManifestValue));
        }

        /// <summary>
        /// Initialises a new instance of <see cref="ExecutableRulesAndDependenciesProvider"/>
        /// </summary>
        /// <param name="executableRulesProvider">The executable rules provider.</param>
        /// <exception cref="System.ArgumentNullException">If <paramref name="executableRulesProvider"/> is <see langword="null"/>.</exception>
        public ExecutableRulesAndDependenciesProvider(IGetsAllExecutableRules executableRulesProvider)
        {
            this.executableRulesProvider = executableRulesProvider ?? throw new System.ArgumentNullException(nameof(executableRulesProvider));
        }
    }
}