using System;
using CSF.Validation.Manifest;

namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// A model for contextual information about a validator-builder.
    /// </summary>
    public class ValidatorBuilderContext
    {
        /// <summary>
        /// Gets the <see cref="ManifestValue"/> instance associated with the current context.
        /// </summary>
        public ManifestValue ManifestValue { get; }

        /// <summary>
        /// Initialises a new instance of <see cref="ValidatorBuilderContext"/>.
        /// </summary>
        /// <param name="manifestValue">The manifest value associated with the current context.</param>
        public ValidatorBuilderContext(ManifestValue manifestValue)
        {
            ManifestValue = manifestValue ?? throw new ArgumentNullException(nameof(manifestValue));
        }
    }
}