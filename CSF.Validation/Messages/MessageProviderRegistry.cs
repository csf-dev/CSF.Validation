using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;

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
    public class MessageProviderRegistry : IGetsCandidateMessageTypes
    {
        readonly IGetsRuleMatchingInfoForMessageProviderType matchingInfoProvider;
        readonly ConcurrentDictionary<Type, List<IGetsMessageProviderTypeMatchingInfoForRule>>
            typesAndMatchProviders = new ConcurrentDictionary<Type, List<IGetsMessageProviderTypeMatchingInfoForRule>>();

        /// <inheritdoc/>
        public IEnumerable<MessageProviderTypeInfo> GetCandidateMessageProviderTypes(ValidationRuleResult result)
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
                    select new MessageProviderTypeInfo(typeAndPriorityGroup.Key, typeAndPriorityGroup.Max()))
                .ToList();
        }
        void RegisterMessageProviderTypes(IEnumerable<Type> types)
        {
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
        /// Initialises a new instance of <see cref="MessageProviderRegistry"/>.
        /// </summary>
        /// <param name="matchingInfoProvider">A service that gets message provider matching info.</param>
        /// <param name="typeOptions">An options object which indicates the available message provider types.</param>
        /// <exception cref="ArgumentNullException">If any parameter is <see langword="null" />.</exception>
        public MessageProviderRegistry(IGetsRuleMatchingInfoForMessageProviderType matchingInfoProvider,
                                       IOptions<MessageProviderTypeOptions> typeOptions)
        {
            if (typeOptions is null)
                throw new ArgumentNullException(nameof(typeOptions));

            this.matchingInfoProvider = matchingInfoProvider ?? throw new ArgumentNullException(nameof(matchingInfoProvider));
            RegisterMessageProviderTypes(typeOptions.Value.MessageProviderTypes);
        }
    }
}