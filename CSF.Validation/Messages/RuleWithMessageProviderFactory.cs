using System;
using Microsoft.Extensions.DependencyInjection;

namespace CSF.Validation.Messages
{
    /// <summary>
    /// Default implementation of <see cref="IGetsRuleWithMessageProvider"/>.
    /// </summary>
    public class RuleWithMessageProviderFactory : IGetsRuleWithMessageProvider
    {
        private readonly IServiceProvider services;

        /// <inheritdoc/>
        public IGetsValidationRuleResultWithMessage GetRuleWithMessageProvider(ResolvedValidationOptions options)
        {
            if (options is null)
                throw new ArgumentNullException(nameof(options));

            var provider = ActivatorUtilities.CreateInstance<SingleRuleValidationResultMessageEnricher>(services);
            if(!options.InstrumentRuleExecution) return provider;

            return ActivatorUtilities.CreateInstance<InstrumentingValidaionRuleResultWithMessageDecorator>(services, provider);
        }

        /// <summary>
        /// Initialises a new instance of <see cref="RuleWithMessageProviderFactory"/>.
        /// </summary>
        /// <param name="services">A service provider</param>
        /// <exception cref="ArgumentNullException">If <paramref name="services"/> is <see langword="null" />.</exception>
        public RuleWithMessageProviderFactory(IServiceProvider services)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
        }
    }
}