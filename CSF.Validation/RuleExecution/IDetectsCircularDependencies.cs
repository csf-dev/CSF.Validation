using System.Collections.Generic;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// An object which detects the presence of circular dependencies between executable rules.
    /// </summary>
    public interface IDetectsCircularDependencies
    {
        /// <summary>
        /// Gets a collection of any circular dependencies which are detected.
        /// </summary>
        /// <param name="rulesAndDependencies">A collection of the all of the rules and dependencies</param>
        /// <returns>A collection of circular dependency models, indicating the circular dependencies detected.</returns>
        IEnumerable<CircularDependency> GetCircularDependencies(IEnumerable<ExecutableRuleAndDependencies> rulesAndDependencies);
    }
}