using System;

namespace CSF.Validation.Messages
{
    /// <summary>
    /// A <see cref="System.Type"/> of message provider and its numeric priority for usage.
    /// </summary>
    public sealed class MessageProviderTypeAndPriority : IEquatable<MessageProviderTypeAndPriority>
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
        /// Gets a value indicating whether the current instance is equal to the specified <see langword="object" />.
        /// </summary>
        /// <param name="obj">An object</param>
        /// <returns><see langword="true" /> if the current instance is equal to the <paramref name="obj"/>; <see langword="false" /> otherwise.</returns>
        public override bool Equals(object obj) => Equals(obj as MessageProviderTypeAndPriority);

        /// <summary>
        /// Gets a value indicating whether the current instance is equal to the specified <see langword="object" />.
        /// </summary>
        /// <param name="other">An other <see cref="MessageProviderTypeAndPriority"/>.</param>
        /// <returns><see langword="true" /> if the current instance is equal to the <paramref name="other"/>; <see langword="false" /> otherwise.</returns>
        public bool Equals(MessageProviderTypeAndPriority other)
        {
            if(other is null) return false;
            return other.ProviderType == ProviderType && other.Priority == Priority;
        }

        /// <summary>
        /// Gets a hash value for the current instance.
        /// </summary>
        /// <returns>A hash value.</returns>
        public override int GetHashCode() => ProviderType.GetHashCode() ^ Priority;

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