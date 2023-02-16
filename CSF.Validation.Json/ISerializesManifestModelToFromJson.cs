using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.ManifestModel;

namespace CSF.Validation
{
    /// <summary>
    /// An object which can serialize &amp; deserialize a <see cref="Value"/> to or from JSON.
    /// </summary>
    public interface ISerializesManifestModelToFromJson
    {
        /// <summary>
        /// Deserializes a stream containing UTF-8 encoded JSON text into a manifest model <see cref="Value"/>.
        /// </summary>
        /// <param name="jsonStream">A stream containing JSON text.</param>
        /// <param name="token">An optional cancellation token.</param>
        /// <returns>A manifest model value.</returns>
        Task<Value> DeserializeManifestModelAsync(Stream jsonStream, CancellationToken token = default);

        /// <summary>
        /// Serializes the specified manifest model <see cref="Value"/> to the specified <paramref name="destinationStream"/>, with UTF-8 encoding.
        /// </summary>
        /// <param name="value">A manifest model value to be serialized.</param>
        /// <param name="destinationStream">A writable stream which shall serve as the destination of the JSON text.</param>
        /// <param name="token">An optional cancellation token.</param>
        /// <returns>A task which completes when the serialization is done.</returns>
        Task SerializeManifestModelAsync(Value value, Stream destinationStream, CancellationToken token = default);
    }
}