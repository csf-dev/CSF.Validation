using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace CSF.Validation.Messages
{
    /// <summary>
    /// A registry of the available <see cref="Type"/> of message-provider services.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A message provider service is class which implements one of the following interfaces.
    /// It is OK if a single class implements more of these interfaces.  It is also acceptable if it
    /// implements more than one closed generic form of either (or both) of the generic interfaces.
    /// </para>
    /// <list type="bullet">
    /// <item><description><see cref="IGetsFailureMessage"/></description></item>
    /// <item><description><see cref="IGetsFailureMessage{TValidated}"/></description></item>
    /// <item><description><see cref="IGetsFailureMessage{TValidated, TParent}"/></description></item>
    /// </list>
    /// <para>
    /// The class may optionally also implement any (or all) of the following, again perhaps implementing multiple
    /// closed generic forms of the same generic interface an unlimited number of times.
    /// </para>
    /// <list type="bullet">
    /// <item><description><see cref="IHasFailureMessageUsageCriteria"/></description></item>
    /// <item><description><see cref="IHasFailureMessageUsageCriteria{TValidated}"/></description></item>
    /// <item><description><see cref="IHasFailureMessageUsageCriteria{TValidated, TParent}"/></description></item>
    /// </list>
    /// <para>
    /// The implementation of this service should be registered in dependency injection as a singleton.
    /// </para>
    /// </remarks>
    public class MessageProviderRegistry : IRegistryOfMessageTypes
    {
        readonly IGetsRuleMatchingInfoForMessageProviderType matchingInfoProvider;
        readonly ConcurrentDictionary<Type, List<IGetsMessageProviderTypeMatchingInfoForRule>>
            typesAndMatchProviders = new ConcurrentDictionary<Type, List<IGetsMessageProviderTypeMatchingInfoForRule>>();

        /// <summary>
        /// Add a number of types (of message provider classes) to this registry.
        /// </summary>
        /// <param name="types">The types to register.</param>
        public void RegisterMessageProviderTypes(IEnumerable<Type> types)
        {
            if (types is null)
                throw new ArgumentNullException(nameof(types));

            foreach (var type in types)
            {
                var matchProviders = matchingInfoProvider.GetMatchingInfo(type).ToList();

                var added = typesAndMatchProviders.TryAdd(type, matchProviders);
                if (!added)
                {
                    var message = String.Format(Resources.ExceptionMessages.GetExceptionMessage("DuplicateTypesNotAllowed"), type.FullName, nameof(MessageProviderRegistry));
                    throw new InvalidOperationException(message);
                }
            }
        }

        /// <summary>
        /// Gets a collection of message provider types from the registry which are candidates to
        /// provide a message for the specified validation rule result.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method does not take into account the <see cref="IHasFailureMessageUsageCriteria.CanGetFailureMessage(ValidationRuleResult)"/>
        /// method, or any of the equivalent methods upon the generic versions of the <see cref="IHasFailureMessageUsageCriteria"/> interface.
        /// This method only filters/selects candidate types based upon the <see cref="FailureMessageStrategyAttribute"/> and the
        /// predicate values stored there.
        /// </para>
        /// <para>
        /// Further logic should be executed afterward to determine whether or not the instantiated instances of the message provider types
        /// are suitable to provide a message for the result.
        /// </para>
        /// </remarks>
        /// <param name="result">A validation rule result.</param>
        /// <returns>A collection of candidate message provider types and their priority.</returns>
        public IEnumerable<MessageProviderTypeAndPriority> GetCandidateMessageProviderTypes(ValidationRuleResult result)
        {
            if (result is null)
                throw new ArgumentNullException(nameof(result));

            return (from typeAndAttribs in typesAndMatchProviders
                    let matchProviders = typeAndAttribs.Value
                    let type = typeAndAttribs.Key
                    from matchProvider in matchProviders
                    where matchProvider.IsMatch(result)
                    let priority = matchProvider.GetPriority()
                    group priority by type into typeAndPriorityGroup
                    select new MessageProviderTypeAndPriority(typeAndPriorityGroup.Key, typeAndPriorityGroup.Max()))
                .ToList();
        }

        /// <summary>
        /// Initialises a new instance of <see cref="MessageProviderRegistry"/>.
        /// </summary>
        /// <param name="matchingInfoProvider">A service that gets message provider matching info.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="matchingInfoProvider"/> is <see langword="null" />.</exception>
        public MessageProviderRegistry(IGetsRuleMatchingInfoForMessageProviderType matchingInfoProvider)
        {
            this.matchingInfoProvider = matchingInfoProvider ?? throw new ArgumentNullException(nameof(matchingInfoProvider));
        }
    }
}