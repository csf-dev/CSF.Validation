using System;
using System.IO;
using System.Text;
using CSF.Validation.ManifestModel;

namespace CSF.Validation
{
    /// <summary>
    /// Extension methods for <see cref="ISerializesManifestModelToFromXml"/>.
    /// </summary>
    public static class XmlManifestModelSerializerExtensions
    {
        /// <summary>
        /// Deserializes a string of XML text into a manifest model <see cref="Value"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The string must be encoded in UTF-8.
        /// </para>
        /// </remarks>
        /// <param name="serializer">The XML manifest model serializer.</param>
        /// <param name="jsonString">A string of XML text.</param>
        /// <returns>A manifest model value.</returns>
        public static Value DeserializeManifestModel(this ISerializesManifestModelToFromXml serializer, string jsonString)
        {
            if (serializer is null)
                throw new ArgumentNullException(nameof(serializer));
            if (jsonString is null)
                return null;

            using(var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
                return serializer.DeserializeManifestModel(stream);
        }

        /// <summary>
        /// Serializes the specified manifest model <see cref="Value"/> to a string.
        /// </summary>
        /// <param name="serializer">The XML manifest model serializer.</param>
        /// <param name="value">A manifest model value to be serialized.</param>
        /// <returns>A UTF-8 string which contains the serialized XML.</returns>
        public static string SerializeManifestModel(this ISerializesManifestModelToFromXml serializer, Value value)
        {
            if (serializer is null)
                throw new ArgumentNullException(nameof(serializer));
            if (value is null)
                return null;

            using(var stream = new MemoryStream())
            {
                serializer.SerializeManifestModel(value, stream);
                stream.Position = 0;

                using(var reader = new StreamReader(stream))
                    return reader.ReadToEnd();
            }
        }
    }
}