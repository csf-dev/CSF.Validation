using Microsoft.Extensions.DependencyInjection;

namespace CSF.Validation
{
    /// <summary>
    /// Extension methods for a service collection which adds XML-serialization services.
    /// </summary>
    public static class XmlServiceCollectionExtensions
    {
        /// <summary>
        /// Adds registrations to the service collection so that validators may be created-from and saved-to XML.
        /// </summary>
        /// <param name="services">A service collection</param>
        /// <returns>The same service collection, so that calls may be chained</returns>
        public static IServiceCollection AddXmlValidationSerializer(this IServiceCollection services)
        {
            services.AddTransient<ISerializesManifestModelToFromXml, XmlManifestModelSerializer>();
            return services;
        }
    }
}