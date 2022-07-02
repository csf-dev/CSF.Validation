using System;

namespace CSF.Validation.ManifestModel
{
    /// <summary>
    /// A service which may be used to get a <see cref="Type"/> from a string rule-type name.
    /// </summary>
    public class RuleTypeResolver : IResolvesRuleType
    {
        /// <summary>
        /// Gets the runtime rule type based upon the specified name.
        /// </summary>
        /// <param name="ruleTypeName">A rule-type name</param>
        /// <returns>A type, or <see langword="null"/> if the name could not be resolved.</returns>
        public Type GetRuleType(string ruleTypeName) => Type.GetType(ruleTypeName);
    }
}