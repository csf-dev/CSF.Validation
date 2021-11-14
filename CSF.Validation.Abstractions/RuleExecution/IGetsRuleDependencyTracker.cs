using System.Collections.Generic;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// An object which can get an instance of <see cref="ITracksRuleDependencies"/> for
    /// a specified collection of rules.
    /// </summary>
    public interface IGetsRuleDependencyTracker
    {
        /// <summary>
        /// Gets the dependency tracker instance for the specified collection of rules and validation options.
        /// </summary>
        /// <param name="allRules">The complete collection of rules.</param>
        /// <param name="options">The validation of options.</param>
        /// <returns>A dependency-tracking service.</returns>
        ITracksRuleDependencies GetDependencyTracker(IEnumerable<ExecutableRuleAndDependencies> allRules, ValidationOptions options);
    }
}