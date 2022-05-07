using System.Linq;

namespace CSF.Validation.Messages
{
    /// <summary>
    /// A service which can get the appropriate failure message provider for a specified <see cref="ValidationRuleResult"/>.
    /// </summary>
    public class FailureMessageProviderSelector : IGetsFailureMessageProvider
    {
        readonly IGetsMessageProviderInfoFactory providerInfoFactoryFactory;

        /// <summary>
        /// Gets the most appropriate message provider implementation for getting a feedback message for the
        /// specified <see cref="ValidationRuleResult"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method uses an instance of <see cref="IGetsMessageProviderInfo"/> to get a collection of candidate
        /// <see cref="MessageProviderInfo"/> instances which could provide a message for the specified rule result.
        /// Once the message provider infos are retrieved, the provider with the highest numeric
        /// <see cref="MessageProviderTypeInfo.Priority"/> is selected and its <see cref="MessageProviderInfo.MessageProvider"/>
        /// implementation is returned as the result of this method.
        /// </para>
        /// <para>
        /// If this method returns a <see langword="null" /> reference then this indicates that there is no message provider
        /// suitable for providing the message for the specified rule result.
        /// If the <see cref="IGetsMessageProviderInfo"/> returned more than one <see cref="MessageProviderInfo"/>
        /// which are tied for the highest priority then any of these may be returned by this method, and results might not
        /// be stable.  Developers are encouraged to avoid message providers with tied priorities.
        /// </para>
        /// <para>
        /// The instance of <see cref="IGetsMessageProviderInfo"/> used is retrieved using a <see cref="IGetsMessageProviderInfoFactory"/>.
        /// </para>
        /// </remarks>
        /// <seealso cref="IGetsMessageProviderInfoFactory"/>
        /// <seealso cref="IGetsMessageProviderInfo"/>
        /// <seealso cref="MessageProviderInfo"/>
        /// <seealso cref="MessageProviderTypeInfo"/>
        /// <seealso cref="MessageProviderInfoFactory"/>
        /// <seealso cref="NullExcludingMessageProviderInfoDecorator"/>
        /// <seealso cref="CriteriaApplyingMessageProviderInfoDecorator"/>
        /// <param name="ruleResult">The validation rule result for which to get a message provider.</param>
        /// <returns>Either an implementation of <see cref="IGetsFailureMessage"/>, or a <see langword="null" />
        /// reference, if no message provider is suitable for the result.</returns>
        public IGetsFailureMessage GetProvider(ValidationRuleResult ruleResult)
        {
            var providerInfoFactory = providerInfoFactoryFactory.GetProviderInfoFactory();
            return (from providerInfo in providerInfoFactory.GetMessageProviderInfo(ruleResult)
                    orderby providerInfo.Priority descending
                    select providerInfo.MessageProvider)
                .FirstOrDefault();
        }

        /// <summary>
        /// Initialises a new instance of <see cref="FailureMessageProviderSelector"/>.
        /// </summary>
        /// <param name="providerInfoFactoryFactory">A factory service that gets the message provider info factory.</param>
        /// <exception cref="System.ArgumentNullException">If <paramref name="providerInfoFactoryFactory"/> is <see langword="null" />.</exception>
        public FailureMessageProviderSelector(IGetsMessageProviderInfoFactory providerInfoFactoryFactory)
        {
            this.providerInfoFactoryFactory = providerInfoFactoryFactory ?? throw new System.ArgumentNullException(nameof(providerInfoFactoryFactory));
        }
    }
}