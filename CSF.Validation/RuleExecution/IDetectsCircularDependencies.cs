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
        /// <param name="rulesAndDependencies"></param>
        /// <returns></returns>
        IEnumerable<CircularDependency> GetCircularDependencies(IEnumerable<ExecutableRuleAndDependencies> rulesAndDependencies);
    }
}