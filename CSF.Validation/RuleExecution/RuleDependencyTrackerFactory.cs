using System.Collections.Generic;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// A factory service to get instances of <see cref="ITracksRuleDependencies"/>.
    /// </summary>
    public class RuleDependencyTrackerFactory : IGetsRuleDependencyTracker
    {
        /// <summary>
        /// Gets the dependency tracker instance for the specified collection of rules and validation options.
        /// </summary>
        /// <param name="allRules">The complete collection of rules.</param>
        /// <param name="options">The validation of options.</param>
        /// <returns>A dependency-tracking service.</returns>
        public ITracksRuleDependencies GetDependencyTracker(IEnumerable<ExecutableRuleAndDependencies> allRules, ValidationOptions options)
            => new RuleDependencyTracker(allRules, options);
    }
}