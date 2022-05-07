using System;

namespace CSF.Validation.Messages
{
    /// <summary>
    /// A specialisation of <see cref="MessageProviderInfo"/> which resolves the <see cref="MessageProviderInfo.MessageProvider"/>
    /// lazily, only instantiating it upon usage.
    /// </summary>
    public class LazyMessageProviderInfo : MessageProviderInfo
    {
        readonly Lazy<IGetsFailureMessage> messageProvider;

        /// <inheritdoc/>
        public override IGetsFailureMessage MessageProvider => messageProvider.Value;

        /// <summary>
        /// Initialises a new instance of <see cref="LazyMessageProviderInfo"/>.
        /// </summary>
        /// <param name="typeInfo">Message provider type info.</param>
        /// <param name="messageProvider">A lazy message provider.</param>
        /// <exception cref="ArgumentNullException">If either <paramref name="typeInfo"/> or <paramref name="messageProvider"/> is <see langword="null" />.</exception>
        public LazyMessageProviderInfo(MessageProviderTypeInfo typeInfo,
                                       Lazy<IGetsFailureMessage> messageProvider) : base(typeInfo)
        {
            this.messageProvider = messageProvider ?? throw new ArgumentNullException(nameof(messageProvider));
        }

        /// <summary>
        /// Initialises a new instance of <see cref="LazyMessageProviderInfo"/>.
        /// </summary>
        /// <param name="copyFrom">Message provider info to copy-from.</param>
        /// <param name="priority">A new priority value, replacing that within <paramref name="copyFrom"/>.</param>
        public LazyMessageProviderInfo(MessageProviderInfo copyFrom, int priority) : base(copyFrom)
        {
            this.messageProvider = new Lazy<IGetsFailureMessage>(() => copyFrom.MessageProvider);
            Priority = priority;
        }
    }
}