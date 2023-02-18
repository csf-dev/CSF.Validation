using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.ManifestModel;

namespace CSF.Validation
{
    /// <summary>
    /// Extension methods for <see cref="ISerializesManifestModelToFromJson"/>.
    /// </summary>
    public static class JsonManifestModelSerializerExtensions
    {
        /// <summary>
        /// Deserializes a string of JSON text into a manifest model <see cref="Value"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The string must be encoded in UTF-8.
        /// </para>
        /// </remarks>
        /// <param name="serializer">The JSON manifest model serializer.</param>
        /// <param name="jsonString">A string of JSON text.</param>
        /// <param name="token">An optional cancellation token.</param>
        /// <returns>A manifest model value.</returns>
        public static Task<Value> DeserializeManifestModelAsync(this ISerializesManifestModelToFromJson serializer, string jsonString, CancellationToken token = default)
        {
            if (serializer is null)
                throw new ArgumentNullException(nameof(serializer));
            if (jsonString is null)
                return Task.FromResult<Value>(null);

            using(var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
                return serializer.DeserializeManifestModelAsync(stream, token);
        }

        /// <summary>
        /// Serializes the specified manifest model <see cref="Value"/> to a string.
        /// </summary>
        /// <param name="serializer">The JSON manifest model serializer.</param>
        /// <param name="value">A manifest model value to be serialized.</param>
        /// <param name="token">An optional cancellation token.</param>
        /// <returns>A UTF-8 string which contains the serialized JSON.</returns>
        public static async Task<string> SerializeManifestModelAsync(this ISerializesManifestModelToFromJson serializer, Value value, CancellationToken token = default)
        {
            if (serializer is null)
                throw new ArgumentNullException(nameof(serializer));
            if (value is null)
                return null;

            using(var stream = new MemoryStream())
            {
                await serializer.SerializeManifestModelAsync(value, stream, token).ConfigureAwait(false);
                stream.Position = 0;

                using(var reader = new StreamReader(stream))
                    return await reader.ReadToEndAsync().ConfigureAwait(false);
            }
        }
    }
}