using System;
using System.Collections.Generic;

namespace CSF.Validation.Messages
{
    /// <summary>
    /// Extension method for <see cref="IRegistryOfMessageTypes"/>.
    /// </summary>
    public static class MessageTypeRegistryExtensions
    {
        /// <summary>
        /// Add a number of types (of message provider classes) to this registry.
        /// </summary>
        /// <param name="registry">The registry.</param>
        /// <param name="types">The types to register.</param>
        public static void RegisterMessageProviderTypes(this IRegistryOfMessageTypes registry, params Type[] types)
        {
            if (registry is null)
                throw new ArgumentNullException(nameof(registry));
            if (types is null)
                throw new ArgumentNullException(nameof(types));

            registry.RegisterMessageProviderTypes((IEnumerable<Type>) types);
        }
    }
}