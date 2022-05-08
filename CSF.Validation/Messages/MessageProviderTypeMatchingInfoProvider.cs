using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CSF.Validation.Messages
{
    /// <summary>
    /// A service which gets a collection of the <see cref="IGetsMessageProviderTypeMatchingInfoForRule"/> for
    /// a message provider, based on its <see cref="FailureMessageStrategyAttribute"/>.
    /// </summary>
    public class MessageProviderTypeMatchingInfoProvider : IGetsRuleMatchingInfoForMessageProviderType
    {
        /// <summary>
        /// Gets a collection of <see cref="IGetsMessageProviderTypeMatchingInfoForRule"/> which are applicable to the specified
        /// message provider type.
        /// </summary>
        /// <param name="messageProviderType">A message-provider type.</param>
        /// <returns>A collection of matching info objects.</returns>
        public IEnumerable<IGetsMessageProviderTypeMatchingInfoForRule> GetMatchingInfo(Type messageProviderType)
        {
            if (messageProviderType is null)
                throw new ArgumentNullException(nameof(messageProviderType));

            var attributes = messageProviderType.GetTypeInfo()
                .GetCustomAttributes<FailureMessageStrategyAttribute>()
                .ToList();

            if(attributes.Any())
                return attributes;

            return new[] { new MatchesEverythingMatchProvider() };
        }

        internal class MatchesEverythingMatchProvider : IGetsMessageProviderTypeMatchingInfoForRule
        {
            public int GetPriority() => 0;

            public bool IsMatch(ValidationRuleResult result) => true;
        }
    }
}