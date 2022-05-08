using CSF.Validation.RuleExecution;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// An objet that gets a rule context for an executable rule.
    /// </summary>
    public interface IGetsRuleContext
    {
        /// <summary>
        /// Gets a rule context for the specified executable rule.
        /// </summary>
        /// <param name="rule">The executable rule.</param>
        /// <returns>A rule context.</returns>
        RuleContext GetRuleContext(ExecutableRule rule);
    }
}