using System;

namespace CSF.Validation.Manifest
{
    /// <summary>
    /// An object which gets the validated type, to store in <see cref="ValidationManifest.ValidatedType"/>.
    /// </summary>
    public interface IGetsValidatedType
    {
        /// <summary>
        /// Gets the validated type from the value type, and whether or not items should be enumerated.
        /// </summary>
        /// <param name="type">The value type.</param>
        /// <param name="enumerateItems">A value indicating whether or not items within the value type are to be enumerated.</param>
        /// <returns>The validated type.</returns>
        Type GetValidatedType(Type type, bool enumerateItems);
    }
}