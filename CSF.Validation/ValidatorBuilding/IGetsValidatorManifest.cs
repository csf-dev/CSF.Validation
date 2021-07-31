using System;

namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// An object which gets an <see cref="IGetsManifestRules"/> (which in turn may get a collection of manifest validation rules)
    /// from a specified type that implements <see cref="IBuildsValidator{TValidated}"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is used when importing validation rules from another validator definition.
    /// </para>
    /// </remarks>
    public interface IGetsValidatorManifest
    {
        /// <summary>
        /// Gets an object which provides manifest rules from a specified validator-builder type.
        /// </summary>
        /// <param name="builderType">A type which must implement <see cref="IBuildsValidator{TValidated}"/>.</param>
        /// <param name="context">Contextual information about how a validator should be built.</param>
        /// <returns>An object which provides a collection of <see cref="Manifest.ManifestRule"/> instances.</returns>
        /// <exception cref="ArgumentException">If the <paramref name="builderType"/> does not implement <see cref="IBuildsValidator{TValidated}"/>.</exception>
        IGetsManifestRules GetValidatorManifest(Type builderType, ValidatorBuilderContext context);

        /// <summary>
        /// Gets an object which provides manifest rules from a specified validator-builder type.
        /// </summary>
        /// <param name="builderType">A type which must implement <see cref="IBuildsValidator{TValidated}"/>.</param>
        /// <param name="context">Contextual information about how validation rules should be built.</param>
        /// <returns>An object which provides a collection of <see cref="Manifest.ManifestRule"/> instances.</returns>
        /// <exception cref="ArgumentException">If the <paramref name="builderType"/> does not implement <see cref="IBuildsValidator{TValidated}"/>.</exception>
        IGetsManifestRules GetValidatorManifest(Type builderType, RuleBuilderContext context);
    }
}