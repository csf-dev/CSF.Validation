using System;
using System.Reflection;
using CSF.Validation.Rules;

namespace CSF.Validation.Bootstrap
{
    /// <summary>
    /// An implementation of <see cref="IResolvesServices"/> which attempts to use an <see cref="IServiceProvider"/>,
    /// but will fall back upon <see cref="Activator.CreateInstance(Type)"/> if the service provider was unable
    /// to resolve the service.
    /// </summary>
    public class ServiceProviderOrActivatorResolver : IResolvesServices, Rules.IResolvesRule
    {
        readonly IServiceProvider serviceProvider;

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
        public T ResolveService<T>(Type implementationType)
        {
            if (implementationType is null)
                throw new ArgumentNullException(nameof(implementationType));
            if(!typeof(T).GetTypeInfo().IsAssignableFrom(implementationType.GetTypeInfo()))
            {
                var message = String.Format(Resources.ExceptionMessages.GetExceptionMessage("ImplTypeMustBeAssignableToServiceType"),
                                            implementationType.FullName,
                                            typeof(T).FullName);
                throw new ArgumentException(message, nameof(implementationType));
            }

            if(serviceProvider.GetService(implementationType) is T resolved) return resolved;
            return ResolveUsingActivator<T>(implementationType);
        }

        static T ResolveUsingActivator<T>(Type implementationType)
        {
            try
            {
                var resolved = Activator.CreateInstance(implementationType, false);
                return (T) resolved;
            }
            catch (System.Exception ex)
            {
                throw new ResolutionException(GetResolutionExceptionMessage(implementationType), ex);
            }
        }

        static string GetResolutionExceptionMessage(Type implementationType)
            => String.Format(Resources.ExceptionMessages.GetExceptionMessage("CouldNotResolveImplementation"),
                             implementationType.FullName);

        object IResolvesRule.ResolveRule(Type ruleType) => ResolveService<object>(ruleType);

        /// <summary>
        /// Initialises an instance of <see cref="ServiceProviderOrActivatorResolver"/>.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="serviceProvider"/> is <see langword="null" />.</exception>
        public ServiceProviderOrActivatorResolver(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }
    }
}