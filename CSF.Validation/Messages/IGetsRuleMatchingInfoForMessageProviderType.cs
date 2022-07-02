using System;
using System.Collections.Generic;

namespace CSF.Validation.Messages
{
    /// <summary>
    /// An object which gets a collection of the <see cref="IGetsMessageProviderTypeMatchingInfoForRule"/> applicable
    /// for a specified message provider type.
    /// </summary>
    public interface IGetsRuleMatchingInfoForMessageProviderType
    {
        /// <summary>
        /// Gets a collection of <see cref="IGetsMessageProviderTypeMatchingInfoForRule"/> which are applicable to the specified
        /// message provider type.
        /// </summary>
        /// <param name="messageProviderType">A message-provider type.</param>
        /// <returns>A collection of matching info objects.</returns>
        IEnumerable<IGetsMessageProviderTypeMatchingInfoForRule> GetMatchingInfo(Type messageProviderType);
    }
}