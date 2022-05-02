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
        /// <remarks>
        /// <para>
        /// This method uses the <see cref="MessageProviderTypeAndPriority.Priority"/> as a tie-break if more than one provider from
        /// the <see cref="IRegistryOfMessageTypes"/> is suitable for the specified rule result.
        /// However, an additional priority of 10 is awarded to message providers which also implement either
        /// <see cref="IHasFailureMessageUsageCriteria"/> or one of its two generic variants for the appropriate generic type parameters:
        /// <see cref="IHasFailureMessageUsageCriteria{TValidated}"/> or <see cref="IHasFailureMessageUsageCriteria{TValidated, TParent}"/>.
        /// </para>
        /// <para>
        /// Thus, if the message provider implements a has-usage-criteria interface, for appropriate generic type(s) where applicable, then
        /// it will always be considered higher-priority than one that is not.
        /// </para>
        /// </remarks>
        /// <param name="ruleResult">The validation rule result for which to get a message provider.</param>
        /// <returns>Either an implementation of <see cref="IGetsFailureMessage"/>, or a <see langword="null" />
        /// reference, if no message provider is suitable for the result.</returns>
        public IGetsFailureMessage GetProvider(ValidationRuleResult ruleResult)
        {
            return (from candidate in registry.GetCandidateMessageProviderTypes(ruleResult)
                    let provider = providerFactory.GetNonGenericFailureMessageProvider(candidate.ProviderType, ruleResult.RuleInterface)
                    let criteria = criteriaFactory.GetNonGenericMessageCriteria(provider, ruleResult.RuleInterface)
                    let basePriority = candidate.Priority
                    let criteriaPriority = (criteria is AllowAllUsageCriteriaProvider) ? 0 : 10
                    let finalPriority = basePriority + criteriaPriority
                    orderby finalPriority descending
                    select new { Provider = provider, Criteria = criteria })
                .FirstOrDefault(x => x.Criteria.CanGetFailureMessage(ruleResult))
                ?.Provider;
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