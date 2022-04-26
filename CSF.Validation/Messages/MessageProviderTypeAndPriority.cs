namespace CSF.Validation.Messages
{
    /// <summary>
    /// A <see cref="System.Type"/> of message provider and its numeric priority for usage.
    /// </summary>
    public class MessageProviderTypeAndPriority
    {
        /// <summary>
        /// Gets the message provider type.
        /// </summary>
        public System.Type ProviderType { get; }

        /// <summary>
        /// Gets the priority of that type's usage.  A higher numeric value means "more important" than a lower value.
        /// </summary>
        public int Priority { get; }

        /// <summary>
        /// Initialises a new instance of <see cref="MessageProviderTypeAndPriority"/>.
        /// </summary>
        /// <param name="providerType">The provider type.</param>
        /// <param name="priority">The priority of that type.</param>
        /// <exception cref="System.ArgumentNullException">If <paramref name="providerType"/> is <see langword="null" />.</exception>
        public MessageProviderTypeAndPriority(System.Type providerType, int priority)
        {
            ProviderType = providerType ?? throw new System.ArgumentNullException(nameof(providerType));
            Priority = priority;
        }
    }
}