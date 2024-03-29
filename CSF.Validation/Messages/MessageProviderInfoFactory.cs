using System;
using System.Collections.Generic;
using System.Linq;

namespace CSF.Validation.Messages
{
    /// <summary>
    /// A factory which gets a collection of <see cref="MessageProviderInfo"/> for a validation rule result.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This implementation only filters the candidate <see cref="MessageProviderInfo"/> which are returned by the
    /// manner in which the <see cref="IGetsCandidateMessageTypes"/> would filter them.
    /// </para>
    /// </remarks>
    public class MessageProviderInfoFactory : IGetsMessageProviderInfo
    {
        readonly IGetsCandidateMessageTypes typeRegistry;
        readonly IGetsMessageProviderFactoryStrategy factoryStrategySelector;

        /// <inheritdoc/>
        public IEnumerable<MessageProviderInfo> GetMessageProviderInfo(ValidationRuleResult ruleResult)
        {
            return (from typeInfo in typeRegistry.GetCandidateMessageProviderTypes(ruleResult)
                    let factoryStrategy = factoryStrategySelector.GetMessageProviderFactory(typeInfo, ruleResult.RuleInterface)
                    where !(factoryStrategy is null)
                    select GetMessageProviderInfo(factoryStrategy, typeInfo, ruleResult))
                .ToList();
        }

        static MessageProviderInfo GetMessageProviderInfo(IGetsNonGenericMessageProvider factory, MessageProviderTypeInfo typeInfo, ValidationRuleResult ruleResult)
            => new LazyMessageProviderInfo(typeInfo, new Lazy<IGetsFailureMessage>(GetProviderFactory(factory, typeInfo, ruleResult)));

        static Func<IGetsFailureMessage> GetProviderFactory(IGetsNonGenericMessageProvider factory, MessageProviderTypeInfo typeInfo, ValidationRuleResult ruleResult)
        {
            return () => factory.GetNonGenericFailureMessageProvider(typeInfo,
                                                                     ruleResult.RuleInterface);
        }

        /// <summary>
        /// Initialises a new instance of <see cref="MessageProviderInfoFactory"/>.
        /// </summary>
        /// <param name="typeRegistry">A provider type registry.</param>
        /// <param name="factoryStrategySelector">A service that gets strategies for message provider factories.</param>
        /// <exception cref="ArgumentNullException">If any parameter is <see langword="null" />.</exception>
        public MessageProviderInfoFactory(IGetsCandidateMessageTypes typeRegistry,
                                          IGetsMessageProviderFactoryStrategy factoryStrategySelector)
        {
            this.typeRegistry = typeRegistry ?? throw new ArgumentNullException(nameof(typeRegistry));
            this.factoryStrategySelector = factoryStrategySelector ?? throw new ArgumentNullException(nameof(factoryStrategySelector));
        }
    }
}