namespace CSF.Validation.Messages
{
    /// <summary>
    /// A specialisation of <see cref="MessageProviderInfo"/> which includes an existing instance of a message provider.
    /// </summary>
    public class InstanceMessageProviderInfo : MessageProviderInfo
    {
        /// <inheritdoc/>
        public override IGetsFailureMessage MessageProvider { get; }

        /// <summary>
        /// Initialises a new instance of <see cref="InstanceMessageProviderInfo"/>.
        /// </summary>
        /// <param name="copyFrom">Type info from which to copy.</param>
        /// <param name="messageProvider">The message provider instance.</param>
        /// <exception cref="System.ArgumentNullException">If any parameter is <see langword="null" />.</exception>
        public InstanceMessageProviderInfo(MessageProviderTypeInfo copyFrom, IGetsFailureMessage messageProvider) : base(copyFrom)
        {
            MessageProvider = messageProvider ?? throw new System.ArgumentNullException(nameof(messageProvider));
        }
    }
}