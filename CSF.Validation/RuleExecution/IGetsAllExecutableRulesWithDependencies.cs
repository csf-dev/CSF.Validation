using System.Collections.Generic;
using CSF.Validation.Manifest;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// An object which gets a collection of <see cref="ExecutableRuleAndDependencies"/> from
    /// a manifest value and an object to be validated.
    /// </summary>
    public interface IGetsAllExecutableRulesWithDependencies
    {
        /// <summary>
        /// Gets a collection of the executable rules and their dependencies from the specified
        /// manifest value and an object to be validated.
        /// </summary>
        /// <param name="manifestValue">The manifest value.</param>
        /// <param name="objectToBeValidated">The object to be validated.</param>
        /// <param name="validationOptions">The validation options.</param>
        /// <returns>A collection of all of the executable rules and the dependencies for each rule.</returns>
        IReadOnlyList<ExecutableRuleAndDependencies> GetRulesWithDependencies(ManifestItem manifestValue,
                                                                              object objectToBeValidated,
                                                                              ResolvedValidationOptions validationOptions);
    }
}