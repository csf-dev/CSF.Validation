using System;
using CSF.Validation.Rules;
using Microsoft.Extensions.DependencyInjection;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// A factory service for implementations of <see cref="IExeucutesSingleRule"/>.
    /// </summary>
    public class SingleRuleExecutorFactory : IGetsSingleRuleExecutor
    {
        readonly IServiceProvider services;

        /// <summary>
        /// Gets the service which may be used for executing validation rules.
        /// </summary>
        /// <param name="options">The validation options.</param>
        /// <returns>A single-rule execution service instance.</returns>
        public IExeucutesSingleRule GetRuleExecutor(ResolvedValidationOptions options)
        {
            var executor = ActivatorUtilities.CreateInstance<SingleRuleExecutor>(services);
            if(!options.InstrumentRuleExecution) return executor;
            return ActivatorUtilities.CreateInstance<InstrumentingSingleRuleExecutorDecorator>(services, executor);
        }

        /// <summary>
        /// Initialises a new instance of <see cref="SingleRuleExecutorFactory"/>.
        /// </summary>
        /// <param name="services">A service provider</param>
        /// <exception cref="System.ArgumentNullException">If <paramref name="services"/> is <see langword="null" />.</exception>
        public SingleRuleExecutorFactory(IServiceProvider services)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
        }
    }
}