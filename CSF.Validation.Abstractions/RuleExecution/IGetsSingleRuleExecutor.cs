namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// An object which can get a service instance suitable for executing a single validation rule.
    /// </summary>
    public interface IGetsSingleRuleExecutor
    {
        /// <summary>
        /// Gets the service which may be used for executing validation rules.
        /// </summary>
        /// <param name="options">The validation options.</param>
        /// <returns>A single-rule execution service instance.</returns>
        IExeucutesSingleRule GetRuleExecutor(ValidationOptions options);
    }
}