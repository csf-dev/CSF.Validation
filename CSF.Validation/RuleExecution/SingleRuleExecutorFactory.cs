using CSF.Validation.Rules;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// A factory service for implementations of <see cref="IExeucutesSingleRule"/>.
    /// </summary>
    public class SingleRuleExecutorFactory : IGetsSingleRuleExecutor
    {
        readonly IGetsRuleContext contextFactory;

        /// <summary>
        /// Gets the service which may be used for executing validation rules.
        /// </summary>
        /// <param name="options">The validation options.</param>
        /// <returns>A single-rule execution service instance.</returns>
        public IExeucutesSingleRule GetRuleExecutor(ResolvedValidationOptions options)
            => new SingleRuleExecutor(contextFactory);

        /// <summary>
        /// Initialises a new instance of <see cref="SingleRuleExecutorFactory"/>.
        /// </summary>
        /// <param name="contextFactory">A rule context factory.</param>
        /// <exception cref="System.ArgumentNullException">If <paramref name="contextFactory"/> is <see langword="null" />.</exception>
        public SingleRuleExecutorFactory(IGetsRuleContext contextFactory)
        {
            this.contextFactory = contextFactory ?? throw new System.ArgumentNullException(nameof(contextFactory));
        }
    }
}