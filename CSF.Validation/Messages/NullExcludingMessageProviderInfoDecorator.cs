using System.Collections.Generic;
using System.Linq;

namespace CSF.Validation.Messages
{
    /// <summary>
    /// A decorator for <see cref="IGetsMessageProviderInfo"/> which excludes message providers which have a
    /// <see langword="null" /> implementation.
    /// </summary>
    public class NullExcludingMessageProviderInfoDecorator : IGetsMessageProviderInfo
    {
        readonly IGetsMessageProviderInfo wrapped;

        /// <inheritdoc/>
        public IEnumerable<MessageProviderInfo> GetMessageProviderInfo(ValidationRuleResult ruleResult)
        {
            return (from providerInfo in wrapped.GetMessageProviderInfo(ruleResult)
                    where !(providerInfo.MessageProvider is null)
                    select providerInfo)
                .ToList();
        }

        /// <summary>
        /// Initialises a new instance of <see cref="NullExcludingMessageProviderInfoDecorator"/>.
        /// </summary>
        /// <param name="wrapped">The wrapped service.</param>
        /// <exception cref="System.ArgumentNullException">If <paramref name="wrapped"/> is <see langword="null" />.</exception>
        public NullExcludingMessageProviderInfoDecorator(IGetsMessageProviderInfo wrapped)
        {
            this.wrapped = wrapped ?? throw new System.ArgumentNullException(nameof(wrapped));
        }
    }
}