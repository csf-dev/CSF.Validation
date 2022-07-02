using System;
using CSF.Validation.Manifest;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// An object which can get an instance of <see cref="IValidationLogic"/> for a specified
    /// <see cref="ManifestRule"/>.
    /// </summary>
    public interface IGetsValidationLogic
    {
        /// <summary>
        /// Gets a validation logic instance for the specified manifest rule definition.
        /// </summary>
        /// <param name="ruleDefinition">A manifest rule definition.</param>
        /// <returns>A validation logic instance by which the rule's logic may be executed in a generalised manner.</returns>
        IValidationLogic GetValidationLogic(ManifestRule ruleDefinition);
    }
}