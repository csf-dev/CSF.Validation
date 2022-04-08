using System;

namespace CSF.Validation.Bootstrap
{
    /// <summary>
    /// An object which may be used to resolve types that might be available in a <see cref="IServiceProvider"/>
    /// or which might need to be constructed using <see cref="Activator.CreateInstance(Type)"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Generally-speaking, this should not be used for validation framework services.  Instead, use plain
    /// dependency injection or an <see cref="IServiceProvider"/>.
    /// The purpose of this service is for resolution of third-party types/services which might not have been
    /// correctly set-up within dependency injection.
    /// This service provides other mechanisms by which attempts may be made to resolve the implementation,
    /// which would fail if we used just a service provider alone.
    /// </para>
    /// </remarks>
    public interface IResolvesServices
    {
        /// <summary>
        /// Attempts to resolve an instance of the specified implementation type.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Attempts to resolve an instance of the specified <paramref name="implementationType"/>, or raises an
        /// exception if this is not possible.
        /// This might occur for user/developer-provided services, particularly if they are not correctly
        /// registered via dependency injection.
        /// This method will make attempts to resolve the service regardless, and might succeed where a plain
        /// <see cref="IServiceProvider"/> would fail.
        /// </para>
        /// </remarks>
        /// <typeparam name="T">The expected type of the service.</typeparam>
        /// <param name="implementationType">The type of the concrete implementation for the service.</param>
        /// <returns>An instance of the requested service; this method will not return <see langword="null" />, it will raise an exception instead.</returns>
        /// <exception cref="ResolutionException">If a service instance could not be resolved successfully.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="implementationType"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">If the <paramref name="implementationType"/> does not derive from <typeparamref name="T"/>.</exception>
        T ResolveService<T>(Type implementationType);
    }
}