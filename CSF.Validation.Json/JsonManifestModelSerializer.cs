using System.IO;
using System.Threading.Tasks;
using CSF.Validation.ManifestModel;
using System.Text.Json;
using System.Threading;
using System.Text.Json.Serialization;
using CSF.Validation.Manifest;

namespace CSF.Validation
{
    /// <summary>
    /// Default implementation of <see cref="ISerializesManifestModelToFromJson"/> which uses <c>System.Text.Json</c>.
    /// </summary>
    public class JsonManifestModelSerializer : ISerializesManifestModelToFromJson
    {
        static readonly JsonSerializerOptions options = GetOptions();

        /// <inheritdoc/>
        public Task<Value> DeserializeManifestModelAsync(Stream jsonStream, CancellationToken token = default)
            => JsonSerializer.DeserializeAsync<Value>(jsonStream, options, token).AsTask();

        /// <inheritdoc/>
        public Task SerializeManifestModelAsync(Value value, Stream destinationStream, CancellationToken token = default)
            => JsonSerializer.SerializeAsync(destinationStream, value, options, token);

        static JsonSerializerOptions GetOptions()
        {
            var options = new JsonSerializerOptions
            {
                IgnoreNullValues = true,
            };
            options.Converters.Add(new JsonStringEnumConverter());
            return options;
        }
    }
}