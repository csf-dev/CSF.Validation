namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// A factory service for implementations of <see cref="IExeucutesSingleRule"/>.
    /// </summary>
    public class SingleRuleExecutorFactory : IGetsSingleRuleExecutor
    {
        /// <summary>
        /// Gets the service which may be used for executing validation rules.
        /// </summary>
        /// <param name="options">The validation options.</param>
        /// <returns>A single-rule execution service instance.</returns>
        public IExeucutesSingleRule GetRuleExecutor(ValidationOptions options)
            => new SingleRuleExecutor();
    }
}