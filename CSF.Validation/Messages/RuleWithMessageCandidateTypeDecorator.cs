using System;
using System.Collections.Generic;
using System.Linq;
using CSF.Validation.Rules;

namespace CSF.Validation.Messages
{
    /// <summary>
    /// A decorator for <see cref="IGetsCandidateMessageTypes"/> which adds an additional message provider for rules which implement
    /// a rule-with-message interface.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This deals with rules which implement either <see cref="IRuleWithMessage{TValidated}"/> or
    /// <see cref="IRuleWithMessage{TValidated, TParent}"/>.
    /// </para>
    /// </remarks>
    public class RuleWithMessageCandidateTypeDecorator : IGetsCandidateMessageTypes
    {
        const int priorityForRuleWithMessage = 100;

        readonly IGetsCandidateMessageTypes wrapped;

        /// <inheritdoc/>
        public IEnumerable<MessageProviderTypeInfo> GetCandidateMessageProviderTypes(ValidationRuleResult result)
        {
            var wrappedResult = wrapped.GetCandidateMessageProviderTypes(result);

            var extraRuleWithMessageProvider = GetRuleWithMessageProviderTypeInfo(result);
            if(extraRuleWithMessageProvider is null) return wrappedResult;

            return wrappedResult.Union(new[] { extraRuleWithMessageProvider });
        }

        static MessageProviderTypeInfo GetRuleWithMessageProviderTypeInfo(ValidationRuleResult result)
            => GetDoubleGenericMessageProvider(result) ?? GetSingleGenericMessageProvider(result);

        static MessageProviderTypeInfo GetDoubleGenericMessageProvider(ValidationRuleResult result)
            => GetNonGenericMessageProvider(result, typeof(IRule<,>), typeof(IRuleWithMessage<,>), typeof(FailureMessageProviderAdapter<,>));

        static MessageProviderTypeInfo GetSingleGenericMessageProvider(ValidationRuleResult result)
            => GetNonGenericMessageProvider(result, typeof(IRule<>), typeof(IRuleWithMessage<>), typeof(FailureMessageProviderAdapter<>));

        static MessageProviderTypeInfo GetNonGenericMessageProvider(ValidationRuleResult result,
                                                                    Type openGenericRuleInterface,
                                                                    Type openGenericRuleWithMessageInterface,
                                                                    Type openGenericAdapterType)
        {
            if(result.RuleInterface.GetGenericTypeDefinition() != openGenericRuleInterface) return null;

            var ruleWithMessageType = openGenericRuleWithMessageInterface.MakeGenericType(result.RuleInterface.GenericTypeArguments);
            if(!ruleWithMessageType.IsInstanceOfType(result.ValidationLogic.RuleObject)) return null;
            
            var adapterType = openGenericAdapterType.MakeGenericType(result.RuleInterface.GenericTypeArguments);
            var provider = (IGetsFailureMessage) Activator.CreateInstance(adapterType, new[] { result.ValidationLogic.RuleObject });
            var typeInfo = new MessageProviderTypeInfo(result.ValidationLogic.RuleObject.GetType(), priorityForRuleWithMessage);
            return new InstanceMessageProviderInfo(typeInfo, provider);
        }

        /// <summary>
        /// Initialises a new instance of <see cref="RuleWithMessageCandidateTypeDecorator"/>.
        /// </summary>
        /// <param name="wrapped">The wrapped candidate message type provider.</param>
        /// <exception cref="System.ArgumentNullException">If <paramref name="wrapped"/> is <see langword="null" />.</exception>
        public RuleWithMessageCandidateTypeDecorator(IGetsCandidateMessageTypes wrapped)
        {
            this.wrapped = wrapped ?? throw new System.ArgumentNullException(nameof(wrapped));
        }
    }
}