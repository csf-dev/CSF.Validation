using System;

namespace CSF.Validation.ManifestModel
{
    /// <summary>
    /// An object which may be used to get a <see cref="Type"/> from a string rule-type name.
    /// </summary>
    public interface IResolvesRuleType
    {
        /// <summary>
        /// Gets the runtime rule type based upon the specified name.
        /// </summary>
        /// <param name="ruleTypeName">A rule-type name</param>
        /// <returns>A type, or <see langword="null"/> if the name could not be resolved.</returns>
        Type GetRuleType(string ruleTypeName);
    }
}