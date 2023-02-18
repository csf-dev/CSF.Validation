using Microsoft.Extensions.DependencyInjection;

namespace CSF.Validation
{
    /// <summary>
    /// Extension methods for a service collection which adds JSON-serialization services.
    /// </summary>
    public static class JsonServiceCollectionExtensions
    {
        /// <summary>
        /// Adds registrations to the service collection so that validators may be created-from and saved-to JSON.
        /// </summary>
        /// <param name="services">A service collection</param>
        /// <returns>The same service collection, so that calls may be chained</returns>
        public static IServiceCollection AddJsonValidationSerializer(this IServiceCollection services)
        {
            services.AddTransient<ISerializesManifestModelToFromJson, JsonManifestModelSerializer>();
            return services;
        }
    }
}