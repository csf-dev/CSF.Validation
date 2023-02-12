using System.Collections.Generic;
using CSF.Validation.Manifest;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// An object which can get all of the executable rules from a manifest value and object to be validated.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This includes the traversal of any descendent values from the object to be validated, as dictated by the
    /// descendents of the specified <see cref="ManifestItem"/>.
    /// </para>
    /// </remarks>
    public interface IGetsAllExecutableRules
    {
        /// <summary>
        /// Gets a flattened collection of executable validation rules from a manifest value and object to be validated.
        /// </summary>
        /// <param name="manifestValue">The manifest value.</param>
        /// <param name="objectToBeValidated">The object to be validated.</param>
        /// <param name="options">The validation options.</param>
        /// <returns>A flattened collection of executable rules from the manifest value and the value's descendents.</returns>
        IReadOnlyList<ExecutableRule> GetExecutableRules(ManifestItem manifestValue,
                                                         object objectToBeValidated,
                                                         ResolvedValidationOptions options);
    }
}