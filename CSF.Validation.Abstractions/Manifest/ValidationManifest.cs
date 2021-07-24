using System;
using System.Collections.Generic;

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
        /// Gets or sets a collection of the rules contained within this validation manifest.
        /// </summary>
        public ICollection<ManifestRule> Rules { get; set; }
    }
}