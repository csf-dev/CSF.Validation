using CSF.Validation.Manifest;

namespace CSF.Validation.ManifestModel
{
    /// <summary>
    /// A simple object that describes both a <see cref="ManifestValue"/> instanec and
    /// also the model <see cref="Value"/> instance which was used to create it.
    /// </summary>
    public class ModelAndManifestValuePair
    {
        /// <summary>
        /// Gets or sets the manifest value instance.
        /// </summary>
        public ManifestItem ManifestValue { get; set; }
        
        /// <summary>
        /// Gets or sets the model value instance.
        /// </summary>
        public ValueBase ModelValue { get; set; }
    }
}