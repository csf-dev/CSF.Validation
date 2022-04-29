using System;
using System.Reflection;
using CSF.Reflection;
using CSF.Validation.Rules;

namespace CSF.Validation.Messages
{
    /// <summary>
    /// A service which acts as a factory for instances of <see cref="IGetsFailureMessage"/> based upon a message provider type and a rule interface.
    /// </summary>
    public class FailureMessageProviderFactory : IGetsNonGenericMessageProvider
    {
        static MethodInfo
            getSingleGenericFailureMessageMethod = Reflect.Method<FailureMessageProviderFactory>(x => x.GetSingleGenericFailureMessage<object>(default)).GetGenericMethodDefinition(),
            getDoubleGenericFailureMessageMethod = Reflect.Method<FailureMessageProviderFactory>(x => x.GetDoubleGenericFailureMessage<object, object>(default)).GetGenericMethodDefinition();

        readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Gets the failure message provider for the specified type and original rule interface.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If the <paramref name="providerType"/> implements more than one message-provider interface, then efforts will be made
        /// by this method to select the correct interface, based upon the <paramref name="ruleInterface"/>.
        /// The message provider interface will be selected based upon which is a closest match to the generic type parameters of the
        /// rule interface.
        /// A more specific message provider interface (taking more generic type parameters) will be selected over a less
        /// specific interface, assuming it is a match.
        /// </para>
        /// </remarks>
        /// <param name="providerType">The message provider type.</param>
        /// <param name="ruleInterface">The interface used for the original rule.</param>
        /// <returns>An implementation of <see cref="IGetsFailureMessage"/>.</returns>
        public IGetsFailureMessage GetNonGenericFailureMessageProvider(Type providerType, Type ruleInterface)
        {
            if (providerType is null)
                throw new ArgumentNullException(nameof(providerType));
            if (ruleInterface is null)
                throw new ArgumentNullException(nameof(ruleInterface));

            var ruleInterfaceInfo = ruleInterface.GetTypeInfo();
            var providerTypeInfo = providerType.GetTypeInfo();
            if(!ruleInterfaceInfo.IsGenericType)
                throw GetIncorrectRuleInterfaceException(ruleInterface);

            if(ImplementsDoubleGenericMessageInterface(providerTypeInfo, ruleInterfaceInfo))
            {
                var method = getDoubleGenericFailureMessageMethod.MakeGenericMethod(ruleInterfaceInfo.GenericTypeParameters[0],
                                                                                    ruleInterfaceInfo.GenericTypeParameters[1]);
                return (IGetsFailureMessage)method.Invoke(this, new[] { providerType });
            }
            if(ImplementsSingleGenericMessageInterface(providerTypeInfo, ruleInterfaceInfo))
            {
                var method = getSingleGenericFailureMessageMethod.MakeGenericMethod(ruleInterfaceInfo.GenericTypeParameters[0]);
                return (IGetsFailureMessage)method.Invoke(this, new[] { providerType });
            }
            if(typeof(IGetsFailureMessage).GetTypeInfo().IsAssignableFrom(providerTypeInfo))
                return (IGetsFailureMessage) serviceProvider.GetService(providerType);

            return null;
        }

        /// <summary>
        /// Gets a the message provider as a <see cref="IGetsFailureMessage{TValidated}"/> and wraps it in an adapter
        /// to be usable as a non-generic object.
        /// </summary>
        /// <typeparam name="TValue">The validated type.</typeparam>
        /// <param name="type">The concrete message provider type.</param>
        /// <returns>The message provider, wrapped in an adapter.</returns>
        IGetsFailureMessage GetSingleGenericFailureMessage<TValue>(Type type)
        {
            var genericProvider = (IGetsFailureMessage<TValue>) serviceProvider.GetService(type);
            return new FailureMessageProviderAdapter<TValue>(genericProvider);
        }

        /// <summary>
        /// Gets a the message provider as a <see cref="IGetsFailureMessage{TValidated, TParent}"/> and wraps it in an adapter
        /// to be usable as a non-generic object.
        /// </summary>
        /// <typeparam name="TValue">The validated type.</typeparam>
        /// <typeparam name="TParent">The parent validated type.</typeparam>
        /// <param name="type">The concrete message provider type.</param>
        /// <returns>The message provider, wrapped in an adapter.</returns>
        IGetsFailureMessage GetDoubleGenericFailureMessage<TValue,TParent>(Type type)
        {
            var genericProvider = (IGetsFailureMessage<TValue,TParent>) serviceProvider.GetService(type);
            return new FailureMessageProviderAdapter<TValue,TParent>(genericProvider);
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
            if(genericRuleInterface != typeof(IRule<>) || genericRuleInterface != typeof(IRule<,>))
                return false;

            return typeof(IGetsFailureMessage<>)
                .MakeGenericType(ruleInterfaceInfo.GenericTypeParameters[0])
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
            if(genericRuleInterface != typeof(IRule<,>))
                return false;

            return typeof(IGetsFailureMessage<,>)
                .MakeGenericType(ruleInterfaceInfo.GenericTypeParameters[0], ruleInterfaceInfo.GenericTypeParameters[1])
                .IsAssignableFrom(providerTypeInfo);
        }

        static Exception GetIncorrectRuleInterfaceException(Type ruleInterface)
        {
            var message = String.Format(Resources.ExceptionMessages.GetExceptionMessage("MustBeGenericRuleInterface"),
                                        nameof(IRule<object>),
                                        ruleInterface.FullName);
            throw new ArgumentException(message, nameof(ruleInterface));
        }

        /// <summary>
        /// Initialises a new instance of <see cref="FailureMessageProviderFactory"/>.
        /// </summary>
        /// <param name="serviceProvider">A service provider.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="serviceProvider"/> is <see langword="null" />.</exception>
        public FailureMessageProviderFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }
    }
}