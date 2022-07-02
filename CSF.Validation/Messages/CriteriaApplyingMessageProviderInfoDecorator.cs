using System.Collections.Generic;
using System.Linq;

namespace CSF.Validation.Messages
{
    /// <summary>
    /// A decorator for <see cref="IGetsMessageProviderInfo"/> which applies/filters the result of the
    /// wrapped service based upon arbitrary criteria if they are present.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This service uses an implementation of <see cref="IGetsNonGenericMessageCriteria"/> in order to determine
    /// the applicable criteria to apply for the message provider.  If that criteria is anything except an instance of
    /// <see cref="AllowAllUsageCriteriaProvider"/> then the returned <see cref="MessageProviderTypeInfo.Priority"/>
    /// of the corresponding result will be increased by ten.
    /// </para>
    /// <para>
    /// Thus, any message provider which implements an appropriate criteria-applying interface and which is permitted
    /// by that criteria will always have a higher priority than an equivalent message provider which does not implement
    /// a criteria interface.
    /// </para>
    /// </remarks>
    public class CriteriaApplyingMessageProviderInfoDecorator : IGetsMessageProviderInfo
    {
        const int priorityIncreaseForImplementingCriteria = 10;

        readonly IGetsMessageProviderInfo wrapped;
        readonly IGetsNonGenericMessageCriteria criteriaFactory;

        /// <inheritdoc/>
        public IEnumerable<MessageProviderInfo> GetMessageProviderInfo(ValidationRuleResult ruleResult)
        {
            return (from providerInfo in wrapped.GetMessageProviderInfo(ruleResult)
                    let criteria = criteriaFactory.GetNonGenericMessageCriteria(providerInfo, ruleResult.RuleInterface)
                    where criteria.CanGetFailureMessage(ruleResult)
                    select GetMessageProviderInfo(providerInfo, criteria))
                .ToList();
        }

        static MessageProviderInfo GetMessageProviderInfo(MessageProviderInfo providerInfo, IHasFailureMessageUsageCriteria criteria)
        {
            var priorityIncrease = (criteria is AllowAllUsageCriteriaProvider) ? 0 : priorityIncreaseForImplementingCriteria;
            var newPriority = providerInfo.Priority + priorityIncrease;
            return new LazyMessageProviderInfo(providerInfo, newPriority);
        }

        /// <summary>
        /// Initialises a new instance of <see cref="CriteriaApplyingMessageProviderInfoDecorator"/>.
        /// </summary>
        /// <param name="wrapped">A wrapped implementation</param>
        /// <param name="criteriaFactory">A criteria factory.</param>
        /// <exception cref="System.ArgumentNullException">If any parameter is <see langword="null" />.</exception>
        public CriteriaApplyingMessageProviderInfoDecorator(IGetsMessageProviderInfo wrapped, IGetsNonGenericMessageCriteria criteriaFactory)
        {
            this.wrapped = wrapped ?? throw new System.ArgumentNullException(nameof(wrapped));
            this.criteriaFactory = criteriaFactory ?? throw new System.ArgumentNullException(nameof(criteriaFactory));
        }
    }
}