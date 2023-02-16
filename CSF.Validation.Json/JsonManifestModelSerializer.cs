using System.IO;
using System.Threading.Tasks;
using CSF.Validation.ManifestModel;
using System.Text.Json;
using System.Threading;

namespace CSF.Validation
{
    /// <summary>
    /// Default implementation of <see cref="ISerializesManifestModelToFromJson"/> which uses <c>System.Text.Json</c>.
    /// </summary>
    public class JsonManifestModelSerializer : ISerializesManifestModelToFromJson
    {
        /// <inheritdoc/>
        public Task<Value> DeserializeManifestModelAsync(Stream jsonStream, CancellationToken token = default)
            => JsonSerializer.DeserializeAsync<Value>(jsonStream, cancellationToken: token).AsTask();

        /// <inheritdoc/>
        public Task SerializeManifestModelAsync(Value value, Stream destinationStream, CancellationToken token = default)
            => JsonSerializer.SerializeAsync(destinationStream, value, cancellationToken: token);
    }
}