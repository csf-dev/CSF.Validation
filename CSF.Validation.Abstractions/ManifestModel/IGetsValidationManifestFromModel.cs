using System;
using CSF.Validation.Manifest;

namespace CSF.Validation.ManifestModel
{
    /// <summary>
    /// An object which can get a <see cref="ValidationManifest"/> from a simplified (serialization-friendly)
    /// model.
    /// </summary>
    public interface IGetsValidationManifestFromModel
    {
        /// <summary>
        /// Gets a validation manifest for validating a specified type, from the specified simple validation model.
        /// </summary>
        /// <param name="rootValue">The <see cref="Value"/> that represents the primary object to be validated.</param>
        /// <param name="validatedType">The type of the primary object to be validated.</param>
        /// <returns>A validation manifest.</returns>
        ValidationManifest GetValidationManifest(Value rootValue, Type validatedType);
    }
}