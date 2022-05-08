using System;
using System.Reflection;
using CSF.Reflection;
using CSF.Validation.Rules;

namespace CSF.Validation.Messages
{
    /// <summary>
    /// A service which gets a <see cref="IHasFailureMessageUsageCriteria"/> for the specified <see cref="IGetsFailureMessage"/>
    /// </summary>
    public class FailureMessageUsageCriteriaFactory : IGetsNonGenericMessageCriteria
    {
        static MethodInfo
            getSingleGenericCriteriaMethod = Reflect.Method<FailureMessageUsageCriteriaFactory>(x => x.GetSingleGenericCriteria<object>(default)).GetGenericMethodDefinition(),
            getDoubleGenericCriteriaMethod = Reflect.Method<FailureMessageUsageCriteriaFactory>(x => x.GetDoubleGenericCriteria<object, object>(default)).GetGenericMethodDefinition();
        static TypeInfo nonGenericCriteriaTypeInfo = typeof(IHasFailureMessageUsageCriteria).GetTypeInfo();

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
        public IHasFailureMessageUsageCriteria GetNonGenericMessageCriteria(MessageProviderInfo messageProviderInfo, Type ruleInterface)
        {
            if (messageProviderInfo is null)
                throw new ArgumentNullException(nameof(messageProviderInfo));
            if (ruleInterface is null)
                throw new ArgumentNullException(nameof(ruleInterface));

            var ruleInterfaceInfo = ruleInterface.GetTypeInfo();
            
            if(ImplementsDoubleGenericCriteriaInterface(messageProviderInfo.ProviderTypeInfo, ruleInterfaceInfo))
            {
                var method = getDoubleGenericCriteriaMethod.MakeGenericMethod(ruleInterfaceInfo.GenericTypeArguments[0],
                                                                              ruleInterfaceInfo.GenericTypeArguments[1]);
                return (IHasFailureMessageUsageCriteria)method.Invoke(this, new[] { messageProviderInfo.GetOriginalProvider() });
            }
            if(ImplementsSingleGenericCriteriaInterface(messageProviderInfo.ProviderTypeInfo, ruleInterfaceInfo))
            {
                var method = getSingleGenericCriteriaMethod.MakeGenericMethod(ruleInterfaceInfo.GenericTypeArguments[0]);
                return (IHasFailureMessageUsageCriteria)method.Invoke(this, new[] { messageProviderInfo.GetOriginalProvider() });
            }
            if(nonGenericCriteriaTypeInfo.IsAssignableFrom(messageProviderInfo.ProviderTypeInfo))
                return (IHasFailureMessageUsageCriteria) messageProviderInfo.GetOriginalProvider();

            return new AllowAllUsageCriteriaProvider();
        }

        /// <summary>
        /// Gets the message criteria service as a <see cref="IHasFailureMessageUsageCriteria{TValidated}"/> and wraps it in an adapter
        /// to be usable as a non-generic object.
        /// </summary>
        /// <typeparam name="TValue">The validated type.</typeparam>
        /// <param name="messageProvider">The message provider instance.</param>
        /// <returns>The message provider, as a criteria object, wrapped in an adapter.</returns>
        IHasFailureMessageUsageCriteria GetSingleGenericCriteria<TValue>(object messageProvider)
        {
            var genericProvider = (IHasFailureMessageUsageCriteria<TValue>) messageProvider;
            return new FailureMessageCriteriaAdapter<TValue>(genericProvider);
        }

        /// <summary>
        /// Gets the message criteria service as a <see cref="IHasFailureMessageUsageCriteria{TValidated, TParent}"/> and wraps it in an adapter
        /// to be usable as a non-generic object.
        /// </summary>
        /// <typeparam name="TValue">The validated type.</typeparam>
        /// <typeparam name="TParent">The parent validated type.</typeparam>
        /// <param name="messageProvider">The message provider instance.</param>
        /// <returns>The message provider, as a criteria object, wrapped in an adapter.</returns>
        IHasFailureMessageUsageCriteria GetDoubleGenericCriteria<TValue,TParent>(object messageProvider)
        {
            var genericProvider = (IHasFailureMessageUsageCriteria<TValue,TParent>) messageProvider;
            return new FailureMessageCriteriaAdapter<TValue,TParent>(genericProvider);
        }

        /// <summary>
        /// Gets a value indicating whether or not the <paramref name="providerTypeInfo"/> implements
        /// <see cref="IHasFailureMessageUsageCriteria{TValidated}"/> for the same generic type parameters
        /// as <paramref name="ruleInterfaceInfo"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The types are considered compatible if either the rule interface is a closed-generic form of <see cref="IRule{TValidated}"/>
        /// and the provider implements <see cref="IHasFailureMessageUsageCriteria{TValidated}"/> for the same type.  It is also considered compatible
        /// if the rule interface is a closed-generic form is <see cref="IRule{TValue, TParent}"/> and the provider implements
        /// <see cref="IHasFailureMessageUsageCriteria{TValidated}"/> for the same first generic type parameter of the rule interface.
        /// </para>
        /// </remarks>
        /// <param name="providerTypeInfo">Info about the message provider type</param>
        /// <param name="ruleInterfaceInfo">Info about the original rule interface</param>
        /// <returns><see langword="true" /> if the types are compatible; <see langword="false" /> otherwise.</returns>
        static bool ImplementsSingleGenericCriteriaInterface(TypeInfo providerTypeInfo, TypeInfo ruleInterfaceInfo)
        {
            if(!ruleInterfaceInfo.IsGenericType) return false;
            var genericRuleInterface = ruleInterfaceInfo.GetGenericTypeDefinition();
            if(genericRuleInterface != typeof(IRule<>) && genericRuleInterface != typeof(IRule<,>))
                return false;

            return typeof(IHasFailureMessageUsageCriteria<>)
                .MakeGenericType(ruleInterfaceInfo.GenericTypeArguments[0])
                .GetTypeInfo()
                .IsAssignableFrom(providerTypeInfo);
        }

        /// <summary>
        /// Gets a value indicating whether or not the <paramref name="providerTypeInfo"/> implements
        /// <see cref="IHasFailureMessageUsageCriteria{TValidated, TParent}"/> for the same generic type parameters
        /// as <paramref name="ruleInterfaceInfo"/>, assuming that it is an <see cref="IRule{TValue, TParent}"/>.
        /// </summary>
        /// <param name="providerTypeInfo">Info about the message provider type</param>
        /// <param name="ruleInterfaceInfo">Info about the original rule interface</param>
        /// <returns><see langword="true" /> if the types are compatible; <see langword="false" /> otherwise.</returns>
        static bool ImplementsDoubleGenericCriteriaInterface(TypeInfo providerTypeInfo, TypeInfo ruleInterfaceInfo)
        {
            if(!ruleInterfaceInfo.IsGenericType) return false;
            var genericRuleInterface = ruleInterfaceInfo.GetGenericTypeDefinition();
            if(genericRuleInterface != typeof(IRule<,>))
                return false;

            return typeof(IHasFailureMessageUsageCriteria<,>)
                .MakeGenericType(ruleInterfaceInfo.GenericTypeArguments[0], ruleInterfaceInfo.GenericTypeArguments[1])
                .GetTypeInfo()
                .IsAssignableFrom(providerTypeInfo);
        }
    }
}