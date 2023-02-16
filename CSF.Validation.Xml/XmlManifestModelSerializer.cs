using System.IO;
using System.Xml.Serialization;
using CSF.Validation.ManifestModel;

namespace CSF.Validation
{
    /// <summary>
    /// Default implementation of <see cref="ISerializesManifestModelToFromXml"/> which uses <c>System.Text.Json</c>.
    /// </summary>
    public class XmlManifestModelSerializer : ISerializesManifestModelToFromXml
    {
        static XmlSerializer serializer = new XmlSerializer(typeof(Value));

        /// <inheritdoc/>
        public Value DeserializeManifestModel(Stream xmlStream)
            => (Value)serializer.Deserialize(xmlStream);

        /// <inheritdoc/>
        public void SerializeManifestModel(Value value, Stream destinationStream)
            => serializer.Serialize(destinationStream, value);
    }
}