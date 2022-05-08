using System;

namespace CSF.Validation.Messages
{
    /// <summary>
    /// Represents information about a message provider type.  An object which implements <see cref="IGetsFailureMessage"/>,
    /// possibly via an adapter.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Because the implementation at <see cref="MessageProvider"/> might be wrapped in an adapter, the original/unwrapped
    /// provider is available from <see cref="GetOriginalProvider"/>.
    /// </para>
    /// </remarks>
    public abstract class MessageProviderInfo : MessageProviderTypeInfo
    {
        /// <summary>
        /// Gets a reference to the original message provider object, removing any potential layers of 'wrapper'
        /// objects which might be present upon <see cref="MessageProvider"/>.
        /// </summary>
        /// <returns>The original/unwrapped message provider object.</returns>
        public object GetOriginalProvider()
        {
            object provider = MessageProvider;
            while(provider is IHasWrappedFailureMessageProvider wrapper)
                provider = wrapper.GetWrappedProvider();
            return provider;
        }
        
        /// <summary>
        /// Gets a reference to the message provider instance.
        /// </summary>
        public abstract IGetsFailureMessage MessageProvider { get; }

        /// <summary>
        /// Initialises a new instance of <see cref="MessageProviderInfo"/>.
        /// </summary>
        /// <param name="copyFrom">An instance to copy from.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="copyFrom"/> is <see langword="null" />.</exception>
        protected MessageProviderInfo(MessageProviderTypeInfo copyFrom) : base(copyFrom) {}
    }
}