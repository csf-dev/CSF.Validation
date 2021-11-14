using System;

namespace CSF.Validation.Manifest
{
    /// <summary>
    /// A model that describes how a validator should operate, such as which
    /// rules it should execute and how they should be configured.
    /// </summary>
    public class ValidationManifest
    {
        /// <summary>
        /// Gets or sets the type of object which the validator should validate.
        /// </summary>
        public Type ValidatedType { get; set; }

        /// <summary>
        /// Gets or sets the root value for the current manifest.
        /// </summary>
        public ManifestValue RootValue { get; set; }
    }
}