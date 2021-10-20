using System.Collections.Generic;
using System.Linq;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// A model for an <see cref="ExecutableRule"/> along with all of its dependencies.
    /// </summary>
    public class ExecutableRuleAndDependencies
    {
        /// <summary>
        /// Gets the executable rule.
        /// </summary>
        public ExecutableRule ExecutableRule { get; }

        /// <summary>
        /// Gets a collection of the rule's dependencies.
        /// </summary>
        public IReadOnlyCollection<ExecutableRule> Dependencies { get; }

        /// <summary>
        /// Initialises an instance of <see cref="ExecutableRuleAndDependencies"/>.
        /// </summary>
        /// <param name="executableRule">The rule.</param>
        /// <param name="dependencies">The rule's dependencies.</param>
        /// <exception cref="System.ArgumentNullException">If <paramref name="executableRule"/> is <see langword="null"/>.</exception>
        public ExecutableRuleAndDependencies(ExecutableRule executableRule, IEnumerable<ExecutableRule> dependencies = null)
        {
            ExecutableRule = executableRule ?? throw new System.ArgumentNullException(nameof(executableRule));
            Dependencies = new List<ExecutableRule>(dependencies ?? Enumerable.Empty<ExecutableRule>());
        }
    }
}