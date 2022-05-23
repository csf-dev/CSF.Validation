using System;
using System.Reflection;

namespace CSF.Validation.Messages
{
    /// <summary>
    /// Represents information about a message provider type.
    /// </summary>
    public class MessageProviderTypeInfo
    {
        /// <summary>
        /// Gets a <see cref="Type"/> object relating to the message provider.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Where the current <see cref="MessageProviderTypeInfo"/> is a specialisation which also provides access to
        /// the message provider itself, this type relates to the original message provider type.
        /// Even if the actual message provider instance is wrapped using an adapter or similar, this type
        /// relates to the original/innermost type in the structure of wrappers.
        /// </para>
        /// </remarks>
        public Type ProviderType { get; }

        /// <summary>
        /// Gets a <see cref="TypeInfo"/> object relating to the message provider.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Where the current <see cref="MessageProviderTypeInfo"/> is a specialisation which also provides access to
        /// the message provider itself, this type info relates to the original message provider type.
        /// Even if the actual message provider instance is wrapped using an adapter or similar, this type info
        /// relates to the original/innermost type info in the structure of wrappers.
        /// </para>
        /// </remarks>
        public TypeInfo ProviderTypeInfo { get; }

        /// <summary>
        /// Gets the numeric priority of the current provider instane.  A higher number
        /// means a more important provider.
        /// </summary>
        public int Priority { get; protected set; }

        /// <summary>
        /// Initialises a new instance of <see cref="MessageProviderTypeInfo"/>.
        /// </summary>
        /// <param name="providerType">The provider type.</param>
        /// <param name="priority">The priority.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="providerType"/> is <see langword="null" />.</exception>
        public MessageProviderTypeInfo(Type providerType, int priority)
        {
            this.ProviderType = providerType ?? throw new ArgumentNullException(nameof(providerType));
            this.ProviderTypeInfo = providerType.GetTypeInfo();
            this.Priority = priority;
        }

        /// <summary>
        /// Initialises a new instance of <see cref="MessageProviderTypeInfo"/>.
        /// </summary>
        /// <param name="copyFrom">An instance to copy from.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="copyFrom"/> is <see langword="null" />.</exception>
        protected MessageProviderTypeInfo(MessageProviderTypeInfo copyFrom)
        {
            if (copyFrom is null)
                throw new ArgumentNullException(nameof(copyFrom));

            this.ProviderType = copyFrom.ProviderType;
            this.ProviderTypeInfo = copyFrom.ProviderTypeInfo;
            this.Priority = copyFrom.Priority;
        }
    }
}