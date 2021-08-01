using System;
using CSF.Validation.Manifest;

namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// A service which maps/converts instances of <see cref="RelativeRuleIdentifier"/> to <see cref="ManifestRuleIdentifier"/>.
    /// </summary>
    public class RelativeToManifestRuleIdentifierConverter : IGetsManifestRuleIdentifierFromRelativeIdentifier
    {
        /// <summary>
        /// Gets the manifest rule identifier from a relative rule identifier.
        /// </summary>
        /// <param name="baseIdentifier">The identifier from which the <paramref name="relativeIdentifier"/> should be derived.</param>
        /// <param name="relativeIdentifier">The relative rule identifier.</param>
        /// <returns>A manifest rule identifier.</returns>
        public ManifestRuleIdentifier GetManifestRuleIdentifier(ManifestRuleIdentifier baseIdentifier, RelativeRuleIdentifier relativeIdentifier)
        {
            if (baseIdentifier is null)
                throw new ArgumentNullException(nameof(baseIdentifier));
            if (relativeIdentifier is null)
                throw new ArgumentNullException(nameof(relativeIdentifier));

            // return new ManifestRuleIdentifier()?

            throw new System.NotImplementedException();
        }
    }
}