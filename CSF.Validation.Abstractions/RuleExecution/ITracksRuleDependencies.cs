using System.Collections.Generic;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// An object which assists an in-progress execution of validation rules by tracking
    /// which rules have already executed and which rules may not yet be executed because
    /// their dependencies have yet to be executed.
    /// </summary>
    public interface ITracksRuleDependencies
    {
        /// <summary>
        /// Gets a collection of rules which are ready to be executed.  Either they have no dependencies or
        /// their dependencies have executed and passed.
        /// </summary>
        /// <returns>A collection of rules that are ready to execute.</returns>
        IEnumerable<ExecutableRule> GetRulesWhichMayBeExecuted();

        /// <summary>
        /// Gets a collection of rules which should not be executed because their dependency rule(s)
        /// have failed.
        /// </summary>
        /// <returns>A collection of rules which have failed dependencies.</returns>
        IEnumerable<ExecutableRule> GetRulesWhoseDependenciesHaveFailed();
    }
}