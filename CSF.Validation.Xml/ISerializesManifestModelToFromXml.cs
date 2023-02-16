using System.IO;
using CSF.Validation.ManifestModel;

namespace CSF.Validation
{
    /// <summary>
    /// An object which can serialize &amp; deserialize a <see cref="Value"/> to or from XML.
    /// </summary>
    public interface ISerializesManifestModelToFromXml
    {
        /// <summary>
        /// Deserializes a stream containing UTF-8 encoded XML text into a manifest model <see cref="Value"/>.
        /// </summary>
        /// <param name="xmlStream">A stream containing XML text.</param>
        /// <returns>A manifest model value.</returns>
        Value DeserializeManifestModel(Stream xmlStream);

        /// <summary>
        /// Serializes the specified manifest model <see cref="Value"/> to the specified <paramref name="destinationStream"/>, with UTF-8 encoding.
        /// </summary>
        /// <param name="value">A manifest model value to be serialized.</param>
        /// <param name="destinationStream">A writable stream which shall serve as the destination of the XML text.</param>
        void SerializeManifestModel(Value value, Stream destinationStream);
    }
}