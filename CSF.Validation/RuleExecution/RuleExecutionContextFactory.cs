using System.Collections.Generic;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// A factory service to get instances of <see cref="IRuleExecutionContext"/>.
    /// </summary>
    public class RuleExecutionContextFactory : IGetsRuleExecutionContext
    {
        /// <inheritdoc/>
        public IRuleExecutionContext GetExecutionContext(IEnumerable<ExecutableRuleAndDependencies> allRules, ResolvedValidationOptions options)
            => new RuleExecutionContext(allRules);
    }
}