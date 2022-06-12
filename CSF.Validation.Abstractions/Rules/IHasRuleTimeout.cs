using System;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// An object (intended to be a validation rule) which can provide a timeout value, which will terminate
    /// the rule early (with an <see cref="RuleOutcome.Errored"/> outcome) if the timeout expires before the
    /// rule completes execution.
    /// </summary>
    public interface IHasRuleTimeout
    {
        /// <summary>
        /// Gets the timeout value for the current rule.
        /// </summary>
        /// <returns>A timespan indicating the timeout duration, or a <see langword="null" /> reference if no timeout is applicable.</returns>
        TimeSpan? GetTimeout();
    }
}