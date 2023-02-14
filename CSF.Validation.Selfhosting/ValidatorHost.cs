using System;
using CSF.Validation.Manifest;
using CSF.Validation.ManifestModel;
using CSF.Validation.ValidatorValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CSF.Validation
{
    /// <summary>
    /// Default implementation of <see cref="IHostsValidationFramework"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// To create a new instance of this type please use the static
    /// <see cref="Build(Action{IServiceCollection}, Action{ValidationOptions})"/> method.
    /// </para>
    /// </remarks>
    public class ValidatorHost : IHostsValidationFramework
    {
        readonly Lazy<IGetsValidator> validatorFactory;
        readonly Lazy<IGetsManifestFromBuilder> manifestFromBuilderProvider;
        readonly Lazy<IGetsValidationManifestFromModel> manifestFromModelProvider;
        readonly Lazy<IValidatesValidationManifest> manifestValidator;

        /// <inheritdoc/>
        public IGetsValidator ValidatorFactory => validatorFactory.Value;

        /// <inheritdoc/>
        public IGetsManifestFromBuilder ManifestFromBuilderProvider => manifestFromBuilderProvider.Value;

        /// <inheritdoc/>
        public IGetsValidationManifestFromModel ManifestFromModelProvider => manifestFromModelProvider.Value;

        /// <inheritdoc/>
        public IValidatesValidationManifest ManifestValidator => manifestValidator.Value;

        /// <summary>
        /// Initialises a new instance of <see cref="ValidatorHost"/>.
        /// </summary>
        /// <param name="services">DI services.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="services"/> is <see langword="null" />.</exception>
        internal ValidatorHost(IServiceProvider services)
        {
            if (services is null)
                throw new ArgumentNullException(nameof(services));

            validatorFactory = LazyFactory<IGetsValidator>(services);
            manifestFromBuilderProvider = LazyFactory<IGetsManifestFromBuilder>(services);
            manifestFromModelProvider = LazyFactory<IGetsValidationManifestFromModel>(services);
            manifestValidator = LazyFactory<IValidatesValidationManifest>(services);
        }

        static Lazy<T> LazyFactory<T>(IServiceProvider services) => new Lazy<T>(() => services.GetRequiredService<T>());

        /// <summary>
        /// Builds a new validator host and returns it.  Parameters may be used to add &amp; configure other services such as
        /// rules &amp; message providers.
        /// </summary>
        /// <remarks>
        /// <para>
        /// See other methods available upon <see cref="ServiceCollectionExtensions"/> for more information about how to register
        /// rules and/or message providers.
        /// </para>
        /// <para>
        /// When using the self-hosting/self-contained validator host, the standard validation rules:
        /// <see cref="StandardRulesServiceCollectionExtensions.UseStandardValidationRules(IServiceCollection)"/> are always
        /// automatically added by default.
        /// </para>
        /// </remarks>
        /// <param name="serviceCollectionConfig">An object permitting further configuration of the self-contained service collection.</param>
        /// <param name="optionsAction">An optional configuration action permitting the setting of default validation options.</param>
        /// <returns>A self-hosting validation service locator object.</returns>
        public static IHostsValidationFramework Build(Action<IServiceCollection> serviceCollectionConfig = null, Action<ValidationOptions> optionsAction = null)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.UseValidationFramework(optionsAction).UseStandardValidationRules();
            if(serviceCollectionConfig != null)
                serviceCollectionConfig(serviceCollection);
            var services = serviceCollection.BuildServiceProvider();

            return new ValidatorHost(services);
        }
    }
}