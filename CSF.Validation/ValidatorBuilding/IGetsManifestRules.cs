using System.Collections.Generic;
using CSF.Validation.Manifest;

namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// An object which can get a collection of manifest rules.
    /// </summary>
    public interface IGetsManifestRules
    {
        /// <summary>
        /// Gets a collection of manifest rules.
        /// </summary>
        /// <returns>A collection of manifest rules.</returns>
        IEnumerable<ManifestRule> GetManifestRules();
    }
}