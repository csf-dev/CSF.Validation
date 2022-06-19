using System;
using System.Collections.Generic;

namespace CSF.Validation.Manifest
{
    /// <summary>
    /// An object within a validation manifest which supports polymorphic validation.
    /// </summary>
    public interface IHasPolymorphicTypes
    {
        /// <summary>
        /// Gets or sets a mapping of the runtime types to polymorphic validation manifest definitions for those types.
        /// </summary>
        ICollection<ManifestPolymorphicType> PolymorphicTypes { get; set; }
    }
}