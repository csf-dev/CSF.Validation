using System;
using System.Collections.Generic;

namespace CSF.Validation.Messages
{
    /// <summary>
    /// An object which gets a collection of the <see cref="MessageProviderInfo"/> applicable
    /// for a specified rule result.
    /// </summary>
    public interface IGetsMessageProviderInfo
    {
        /// <summary>
        /// Gets a collection of <see cref="MessageProviderInfo"/> which are applicable for the specified
        /// rule result.
        /// </summary>
        /// <param name="ruleResult">A validation rule result.</param>
        /// <returns>A collection of message provider info objects.</returns>
        IEnumerable<MessageProviderInfo> GetMessageProviderInfo(ValidationRuleResult ruleResult);
    }
}