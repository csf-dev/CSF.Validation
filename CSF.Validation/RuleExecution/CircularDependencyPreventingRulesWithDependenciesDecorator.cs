using System;
using System.Collections.Generic;
using System.Linq;
using CSF.Validation.Manifest;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// A decorator for the service <see cref="IGetsAllExecutableRulesWithDependencies"/> which returns the
    /// result from the wrapped implementation but additionally throws <see cref="ValidationException"/> if
    /// any circular dependencies are found.
    /// </summary>
    public class CircularDependencyPreventingRulesWithDependenciesDecorator : IGetsAllExecutableRulesWithDependencies
    {
        const int maxCircularDependenciesInException = 10;

        static readonly string newLine = Environment.NewLine;

        readonly IGetsAllExecutableRulesWithDependencies wrapped;
        readonly IDetectsCircularDependencies circularDependencyDetector;

        /// <summary>
        /// Gets a collection of the executable rules and their dependencies from the specified
        /// manifest value and an object to be validated.
        /// </summary>
        /// <param name="manifestValue">The manifest value.</param>
        /// <param name="objectToBeValidated">The object to be validated.</param>
        /// <returns>A collection of all of the executable rules and the dependencies for each rule.</returns>
        public IReadOnlyList<ExecutableRuleAndDependencies> GetRulesWithDependencies(ManifestValue manifestValue, object objectToBeValidated)
        {
            var result = wrapped.GetRulesWithDependencies(manifestValue, objectToBeValidated);
            AssertNoCircularDependencies(result);
            return result;
        }

        void AssertNoCircularDependencies(IEnumerable<ExecutableRuleAndDependencies> rules)
        {
            var circularDependencies = circularDependencyDetector.GetCircularDependencies(rules);
            if(!circularDependencies.Any()) return;

            var message = GetExceptionMessage(circularDependencies);
            throw new ValidationException(message);
        }

        static string GetExceptionMessage(IEnumerable<CircularDependency> circularDependencies)
        {
            var messageTemplate = Resources.ExceptionMessages.GetExceptionMessage("CircularDependenciesNotAllowedInRules");
            var messageFirstLine = String.Format(messageTemplate, maxCircularDependenciesInException);

            var firstFewCircularDependencies = circularDependencies;
            var formattedCircularDependencies = String.Join(newLine, firstFewCircularDependencies);

            return String.Concat(messageFirstLine, newLine, newLine, formattedCircularDependencies);
        }

        /// <summary>
        /// Initialises a new instance of <see cref="CircularDependencyPreventingRulesWithDependenciesDecorator"/>
        /// </summary>
        /// <param name="wrapped">The wrapped implementation.</param>
        /// <param name="circularDependencyDetector">A circular dependency detector.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="circularDependencyDetector"/> or <paramref name="wrapped"/> are <see langword="null"/>.</exception>
        public CircularDependencyPreventingRulesWithDependenciesDecorator(IGetsAllExecutableRulesWithDependencies wrapped, IDetectsCircularDependencies circularDependencyDetector)
        {
            this.wrapped = wrapped ?? throw new ArgumentNullException(nameof(wrapped));
            this.circularDependencyDetector = circularDependencyDetector ?? throw new ArgumentNullException(nameof(circularDependencyDetector));
        }
    }
}