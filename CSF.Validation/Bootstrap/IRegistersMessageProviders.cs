using System;
using System.Collections.Generic;
using System.Reflection;
using CSF.Validation.Messages;

namespace CSF.Validation.Bootstrap
{
    /// <summary>
    /// An object used to indicate which assemblies &amp; types should be searched to find validation message providers.
    /// </summary>
    public interface IRegistersMessageProviders
    {
        /// <summary>
        /// Scans the specified <paramref name="assembly"/> for validation message providers and registers every one of
        /// them, so that they may be dependency-injected.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Use this method, <see cref="AddMessageProvidersInAssemblies(Assembly[])"/> or
        /// <see cref="AddMessageProvidersInAssemblies(IEnumerable{Assembly})"/> as a convenient
        /// way to add many validation message providers to your dependency injection container.
        /// </para>
        /// <para>
        /// This method also registers the found message provider types so that they may be used with
        /// an <see cref="IGetsCandidateMessageTypes"/>.
        /// </para>
        /// </remarks>
        /// <param name="assembly">An assembly to scan for validation message provider classes.</param>
        /// <returns>The registration helper, so that calls may be chained.</returns>
        IRegistersMessageProviders AddMessageProvidersInAssembly(Assembly assembly);

        /// <summary>
        /// Scans the specified <paramref name="assemblies"/> for validation message providers and registers every one of
        /// them, so that they may be dependency-injected.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Use this method, <see cref="AddMessageProvidersInAssembly(Assembly)"/> or
        /// <see cref="AddMessageProvidersInAssemblies(IEnumerable{Assembly})"/> as a convenient
        /// way to add many validation message providers to your dependency injection container.
        /// </para>
        /// <para>
        /// This method also registers the found message provider types so that they may be used with
        /// an <see cref="IGetsCandidateMessageTypes"/>.
        /// </para>
        /// </remarks>
        /// <param name="assemblies">A collection of assemblies to scan for validation message provider classes.</param>
        /// <returns>The registration helper, so that calls may be chained.</returns>
        IRegistersMessageProviders AddMessageProvidersInAssemblies(params Assembly[] assemblies);

        /// <summary>
        /// Scans the specified <paramref name="assemblies"/> for validation message providers and registers every one of
        /// them, so that they may be dependency-injected.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Use this method, <see cref="AddMessageProvidersInAssembly(Assembly)"/> or
        /// <see cref="AddMessageProvidersInAssemblies(Assembly[])"/> as a convenient
        /// way to add many validation message providers to your dependency injection container.
        /// </para>
        /// <para>
        /// This method also registers the found message provider types so that they may be used with
        /// an <see cref="IGetsCandidateMessageTypes"/>.
        /// </para>
        /// </remarks>
        /// <param name="assemblies">A collection of assemblies to scan for validation message provider classes.</param>
        /// <returns>The registration helper, so that calls may be chained.</returns>
        IRegistersMessageProviders AddMessageProvidersInAssemblies(IEnumerable<Assembly> assemblies);

        /// <summary>
        /// Adds a single validation message provider type to the the service collection, so that it may be dependency-injected.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Use this method only when you wish to add individual message providers to dependency injection.  It is usually more convenient
        /// to use one of the following methods to add message providers in bulk, using assembly-scanning techniques.
        /// </para>
        /// <list type="bullet">
        /// <item><description><see cref="AddMessageProvidersInAssembly(Assembly)"/></description></item>
        /// <item><description><see cref="AddMessageProvidersInAssemblies(Assembly[])"/></description></item>
        /// <item><description><see cref="AddMessageProvidersInAssemblies(IEnumerable{Assembly})"/></description></item>
        /// </list>
        /// <para>
        /// This method also registers the message provider type so that it may be used with
        /// an <see cref="IGetsCandidateMessageTypes"/>.
        /// </para>
        /// </remarks>
        /// <param name="providerType">The type of validation message provider to add to DI.</param>
        /// <returns>The registration helper, so that calls may be chained.</returns>
        IRegistersMessageProviders AddMessageProvider(Type providerType);
    }
}