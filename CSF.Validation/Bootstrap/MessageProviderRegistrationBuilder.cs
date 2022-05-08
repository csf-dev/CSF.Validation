using System;
using System.Collections.Generic;
using System.Reflection;
using CSF.Validation.Messages;
using Microsoft.Extensions.DependencyInjection;

namespace CSF.Validation.Bootstrap
{
    /// <summary>
    /// A helper/builder service which coordinates both the registration of message provider types with
    /// the service collection, and also registers them with an options type so that they may be found by
    /// <see cref="MessageProviderRegistry"/>.
    /// </summary>
    public class MessageProviderRegistrationBuilder : IRegistersMessageProviders
    {
        private readonly IServiceCollection serviceCollection;
        readonly List<Type> messageProviderTypes = new List<Type>();

        /// <summary>
        /// Gets a collection of the message provider types which have been found and registered with the current builder.
        /// </summary>
        public IReadOnlyCollection<Type> MessageProviderTypes => messageProviderTypes;

        /// <inheritdoc/>
        public IRegistersMessageProviders AddMessageProvidersInAssembly(Assembly assembly)
            => AddMessageProvidersInAssemblies(assembly);

        /// <inheritdoc/>
        public IRegistersMessageProviders AddMessageProvidersInAssemblies(params Assembly[] assemblies)
            => AddMessageProvidersInAssemblies((IEnumerable<Assembly>)assemblies);

        /// <inheritdoc/>
        public IRegistersMessageProviders AddMessageProvidersInAssemblies(IEnumerable<Assembly> assemblies)
        {
            if (assemblies is null)
                throw new ArgumentNullException(nameof(assemblies));

            foreach (var providerType in MessageAssemblyScanner.GetMessageProviderTypesFromAssemblies(assemblies))
                AddMessageProvider(providerType);

            return this;
        }

        /// <inheritdoc/>
        public IRegistersMessageProviders AddMessageProvider(Type providerType)
        {
            if (providerType is null)
                throw new ArgumentNullException(nameof(providerType));

            messageProviderTypes.Add(providerType);
            serviceCollection.AddTransient(providerType);
            return this;
        }

        /// <summary>
        /// Initialises a new instance of <see cref="MessageProviderRegistrationBuilder"/>.
        /// </summary>
        /// <param name="serviceCollection">A service collection.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public MessageProviderRegistrationBuilder(IServiceCollection serviceCollection)
        {
            this.serviceCollection = serviceCollection ?? throw new ArgumentNullException(nameof(serviceCollection));
        }
    }
}