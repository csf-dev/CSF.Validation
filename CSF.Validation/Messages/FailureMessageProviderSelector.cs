using System.Linq;

namespace CSF.Validation.Messages
{
    /// <summary>
    /// A service which can get the appropriate failure message provider for a specified <see cref="ValidationRuleResult"/>.
    /// </summary>
    public class FailureMessageProviderSelector : IGetsFailureMessageProvider
    {
        readonly IRegistryOfMessageTypes registry;
        readonly IGetsNonGenericMessageProvider providerFactory;
        readonly IGetsNonGenericMessageCriteria criteriaFactory;

        /// <summary>
        /// Gets the appropriate message provider for getting a feedback message for the
        /// specified <see cref="ValidationRuleResult"/>.
        /// </summary>
        /// <param name="ruleResult">The validation rule result for which to get a message provider.</param>
        /// <returns>Either an implementation of <see cref="IGetsFailureMessage"/>, or a <see langword="null" />
        /// reference, if no message provider is suitable for the result.</returns>
        public IGetsFailureMessage GetProvider(ValidationRuleResult ruleResult)
        {
            return (from candidate in registry.GetCandidateMessageProviderTypes(ruleResult)
                    let provider = providerFactory.GetNonGenericFailureMessageProvider(candidate.ProviderType, ruleResult.RuleInterface)
                    let criteria = criteriaFactory.GetNonGenericMessageCriteria(provider, ruleResult.RuleInterface)
                    where criteria.CanGetFailureMessage(ruleResult)
                    orderby candidate.Priority descending)
                .FirstOrDefault();
        }

        /// <summary>
        /// Initialises a new instance of <see cref="FailureMessageProviderSelector"/>.
        /// </summary>
        /// <param name="registry">The provider-type registry.</param>
        /// <param name="providerFactory">A factory for instances of <see cref="IGetsFailureMessage"/>.</param>
        /// <param name="criteriaFactory">A factory for instances of <see cref="IHasFailureMessageUsageCriteria"/>.</param>
        /// <exception cref="System.ArgumentNullException">If any constructor parameter is <see langword="null" />.</exception>
        public FailureMessageProviderSelector(IRegistryOfMessageTypes registry,
                                                        IGetsNonGenericMessageProvider providerFactory,
                                                        IGetsNonGenericMessageCriteria criteriaFactory)
        {
            this.registry = registry ?? throw new System.ArgumentNullException(nameof(registry));
            this.providerFactory = providerFactory ?? throw new System.ArgumentNullException(nameof(providerFactory));
            this.criteriaFactory = criteriaFactory ?? throw new System.ArgumentNullException(nameof(criteriaFactory));
        }
    }
}