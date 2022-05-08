using System;
using System.Reflection;
using CSF.Validation.Rules;
using Microsoft.Extensions.DependencyInjection;

namespace CSF.Validation.Messages
{
    /// <summary>
    /// A strategy-selector/factory class which gets the appropriate strategy implementation for getting a
    /// failure message provider implementation for a specified message provider type and rule interface.
    /// </summary>
    public class MessageProviderFactoryStrategyProvider : IGetsMessageProviderFactoryStrategy
    {
        static readonly TypeInfo nonGenericProviderTypeInfo = typeof(IGetsFailureMessage).GetTypeInfo();

        readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Gets a message provider factory which is appropriate for the specified provider type and rule interface.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method will examine the message provider interfaces which are implemented by the
        /// <paramref name="providerType"/>.  Specifically it will consider all implementations of any of:
        /// </para>
        /// <list type="number">
        /// <item><description><see cref="IGetsFailureMessage{TValidated, TParent}"/></description></item>
        /// <item><description><see cref="IGetsFailureMessage{TValidated}"/></description></item>
        /// <item><description><see cref="IGetsFailureMessage"/></description></item>
        /// </list>
        /// <para>
        /// It will then return a message-provider-factory strategy implementation which best-matches the interfaces
        /// implemented by the <paramref name="providerType"/>, based upon the actual validation rule interface
        /// which is in-use: <paramref name="ruleInterface"/>.
        /// </para>
        /// <para>
        /// This 'matching' process will look for matches in the order the message provider interfaces are listed
        /// above.  For example, if a message provider implements the interface with two generic types, and for types that
        /// are compatible with the generic types present upon the rule interface, then the double-generic strategy
        /// will be given preference over a single-generic strategy.  This is true even if the message provider type also
        /// implements a single-generic message provider interface that is compatible with the rule interface.
        /// </para>
        /// <para>
        /// Obviously, if the message provider type implements the non-generic <see cref="IGetsFailureMessage"/> then
        /// this will match every possible rule interface, as it is non-generic.  This behaviour may be leveraged as
        /// a fall-back option for unexpected scenarios (such as validating <see cref="RuleOutcome.Errored"/> results).
        /// </para>
        /// <para>
        /// If this method cannot find any suitable strategy for getting a message provider - the <paramref name="providerType"/>
        /// does not implement any interface which is compatible with the <paramref name="ruleInterface"/> - then it
        /// will return <see langword="null" />.  This means that no factory strategy is applicable and that the provider
        /// type is not compatible with the rule interface.
        /// </para>
        /// </remarks>
        /// <param name="providerType"></param>
        /// <param name="ruleInterface"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public IGetsNonGenericMessageProvider GetMessageProviderFactory(Type providerType, Type ruleInterface)
        {
            if (providerType is null)
                throw new ArgumentNullException(nameof(providerType));
            if (ruleInterface is null)
                throw new ArgumentNullException(nameof(ruleInterface));

            var providerTypeInfo = providerType.GetTypeInfo();
            var ruleInterfaceInfo = ruleInterface.GetTypeInfo();
            if(!IsValidRuleInterface(ruleInterfaceInfo))
                throw GetIncorrectRuleInterfaceException(ruleInterface);

            if(ImplementsDoubleGenericMessageInterface(providerTypeInfo, ruleInterfaceInfo))
                return serviceProvider.GetRequiredService<DoubleGenericMessageProviderStrategy>();
            if(ImplementsSingleGenericMessageInterface(providerTypeInfo, ruleInterfaceInfo))
                return serviceProvider.GetRequiredService<SingleGenericMessageProviderStrategy>();
            if(nonGenericProviderTypeInfo.IsAssignableFrom(providerTypeInfo))
                return serviceProvider.GetRequiredService<NonGenericMessageProviderStrategy>();

            return null;
        }

        /// <summary>
        /// Gets a value indicating whether or not the <paramref name="providerTypeInfo"/> implements
        /// <see cref="IGetsFailureMessage{TValidated}"/> for the same generic type parameters
        /// as <paramref name="ruleInterfaceInfo"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The types are considered compatible if either the rule interface is a closed-generic form of <see cref="IRule{TValidated}"/>
        /// and the provider implements <see cref="IGetsFailureMessage{TValidated}"/> for the same type.  It is also considered compatible
        /// if the rule interface is a closed-generic form is <see cref="IRule{TValue, TParent}"/> and the provider implements
        /// <see cref="IGetsFailureMessage{TValidated}"/> for the same first generic type parameter of the rule interface.
        /// </para>
        /// </remarks>
        /// <param name="providerTypeInfo">Info about the message provider type</param>
        /// <param name="ruleInterfaceInfo">Info about the original rule interface</param>
        /// <returns><see langword="true" /> if the types are compatible; <see langword="false" /> otherwise.</returns>
        static bool ImplementsSingleGenericMessageInterface(TypeInfo providerTypeInfo, TypeInfo ruleInterfaceInfo)
        {
            var genericRuleInterface = ruleInterfaceInfo.GetGenericTypeDefinition();
            if(!IsSingleGenericRuleInterface(genericRuleInterface) && !IsDoubleGenericRuleInterface(genericRuleInterface))
                return false;

            return typeof(IGetsFailureMessage<>)
                .MakeGenericType(ruleInterfaceInfo.GenericTypeArguments[0])
                .GetTypeInfo()
                .IsAssignableFrom(providerTypeInfo);
        }

        /// <summary>
        /// Gets a value indicating whether or not the <paramref name="providerTypeInfo"/> implements
        /// <see cref="IGetsFailureMessage{TValidated, TParent}"/> for the same generic type parameters
        /// as <paramref name="ruleInterfaceInfo"/>, assuming that it is an <see cref="IRule{TValue, TParent}"/>.
        /// </summary>
        /// <param name="providerTypeInfo">Info about the message provider type</param>
        /// <param name="ruleInterfaceInfo">Info about the original rule interface</param>
        /// <returns><see langword="true" /> if the types are compatible; <see langword="false" /> otherwise.</returns>
        static bool ImplementsDoubleGenericMessageInterface(TypeInfo providerTypeInfo, TypeInfo ruleInterfaceInfo)
        {
            var genericRuleInterface = ruleInterfaceInfo.GetGenericTypeDefinition();
            if(!IsDoubleGenericRuleInterface(genericRuleInterface))
                return false;

            return typeof(IGetsFailureMessage<,>)
                .MakeGenericType(ruleInterfaceInfo.GenericTypeArguments[0], ruleInterfaceInfo.GenericTypeArguments[1])
                .GetTypeInfo()
                .IsAssignableFrom(providerTypeInfo);
        }

        static bool IsValidRuleInterface(TypeInfo ruleInterfaceInfo)
        {
            if(!ruleInterfaceInfo.IsGenericType)
                return false;

            var openGenericInterface = ruleInterfaceInfo.GetGenericTypeDefinition();
            return IsDoubleGenericRuleInterface(openGenericInterface) || IsSingleGenericRuleInterface(openGenericInterface);
        }

        static bool IsSingleGenericRuleInterface(Type openGenericRuleInterface)
            => openGenericRuleInterface == typeof(IRule<>);

        static bool IsDoubleGenericRuleInterface(Type openGenericRuleInterface)
            => openGenericRuleInterface == typeof(IRule<,>);

        static Exception GetIncorrectRuleInterfaceException(Type ruleInterface)
        {
            var message = String.Format(Resources.ExceptionMessages.GetExceptionMessage("MustBeGenericRuleInterface"),
                                        nameof(IRule<object>),
                                        ruleInterface.FullName);
            return new ArgumentException(message, nameof(ruleInterface));
        }

        /// <summary>
        /// Initialises a new instance of <see cref="MessageProviderFactoryStrategyProvider"/>.
        /// </summary>
        /// <param name="serviceProvider">A service provider.</param>
        /// <exception cref="ArgumentNullException">If the <paramref name="serviceProvider"/> is <see langword="null" />.</exception>
        public MessageProviderFactoryStrategyProvider(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }
    }
}