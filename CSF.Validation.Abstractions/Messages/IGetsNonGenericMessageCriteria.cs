using System;

namespace CSF.Validation.Messages
{
    /// <summary>
    /// An object which can get an instance of <see cref="IHasFailureMessageUsageCriteria"/>
    /// from an instance of <see cref="MessageProviderInfo"/>.
    /// </summary>
    public interface IGetsNonGenericMessageCriteria
    {
        /// <summary>
        /// Gets the usage criteria from the specified message provider and interface type.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method should never return <see langword="null" />.  If the message provider cannot get a
        /// suitable implementation of <see cref="IHasFailureMessageUsageCriteria"/> then a matches-all criteria
        /// instance should be returned.
        /// </para>
        /// </remarks>
        /// <param name="messageProviderInfo">Message provider info.</param>
        /// <param name="ruleInterface">The interface used for the original validation rule.</param>
        /// <returns>An instance of <see cref="IHasFailureMessageUsageCriteria"/>.</returns>
        IHasFailureMessageUsageCriteria GetNonGenericMessageCriteria(MessageProviderInfo messageProviderInfo, Type ruleInterface);
    }
}