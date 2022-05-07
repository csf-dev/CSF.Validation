using System;
using System.Reflection;
using CSF.Reflection;

namespace CSF.Validation.Messages
{
    /// <summary>
    /// A strategy for getting <see cref="IGetsFailureMessage"/> from a type that implements the
    /// generic <see cref="IGetsFailureMessage{TValidated, TParent}"/>.
    /// </summary>
    public class DoubleGenericMessageProviderStrategy : IGetsNonGenericMessageProvider
    {
        static MethodInfo
            getDoubleGenericFailureMessageMethod = Reflect.Method<DoubleGenericMessageProviderStrategy>(x => x.GetDoubleGenericFailureMessage<object, object>(default)).GetGenericMethodDefinition();

        readonly IServiceProvider serviceProvider;

        /// <inheritdoc/>
        public IGetsFailureMessage GetNonGenericFailureMessageProvider(Type providerType, Type ruleInterface)
        {
            var ruleInterfaceInfo = providerType.GetTypeInfo();
            var method = getDoubleGenericFailureMessageMethod.MakeGenericMethod(ruleInterfaceInfo.GenericTypeParameters[0],
                                                                                ruleInterfaceInfo.GenericTypeParameters[1]);
            return (IGetsFailureMessage)method.Invoke(this, new[] { providerType });
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
        /// Initialises a new instance of <see cref="DoubleGenericMessageProviderStrategy"/>.
        /// </summary>
        /// <param name="serviceProvider">A service provider.</param>
        /// <exception cref="ArgumentNullException">If the <paramref name="serviceProvider"/> is <see langword="null" />.</exception>
        public DoubleGenericMessageProviderStrategy(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }
    }
}