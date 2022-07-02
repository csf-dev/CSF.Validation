using System.Collections.Generic;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// An object which can get an instance of <see cref="IRuleExecutionContext"/> for
    /// a specified collection of rules.
    /// </summary>
    public interface IGetsRuleExecutionContext
    {
        /// <summary>
        /// Gets the rule execution context instance for the specified collection of rules and validation options.
        /// </summary>
        /// <param name="allRules">The complete collection of rules.</param>
        /// <param name="options">The validation of options.</param>
        /// <returns>An execution context.</returns>
        IRuleExecutionContext GetExecutionContext(IEnumerable<ExecutableRuleAndDependencies> allRules, ResolvedValidationOptions options);
    }
}