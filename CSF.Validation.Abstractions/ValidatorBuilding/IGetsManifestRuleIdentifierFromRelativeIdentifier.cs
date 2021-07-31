using CSF.Validation.Manifest;

namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// A service which may convert a <see cref="RelativeRuleIdentifier"/> into a <see cref="ManifestRuleIdentifier"/>.
    /// </summary>
    public interface IGetsManifestRuleIdentifierFromRelativeIdentifier
    {
        /// <summary>
        /// Gets the manifest rule identifier from a relative rule identifier.
        /// </summary>
        /// <param name="relativeIdentifier">The relative rule identifier.</param>
        /// <returns>A manifest rule identifier.</returns>
        ManifestRuleIdentifier GetManifestRuleIdentifier(RelativeRuleIdentifier relativeIdentifier);
    }
}