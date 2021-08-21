using System;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// An object which can get/resolve instances of validation rule types based upon their concrete <see cref="Type"/>.
    /// </summary>
    public interface IResolvesRule
    {
        /// <summary>
        /// Sets/resolves an instance of the specified <paramref name="ruleType"/>.
        /// </summary>
        /// <param name="ruleType">The concrete type of rule to resolve.</param>
        /// <returns>The resolved validation rule instance.</returns>
        object ResolveRule(Type ruleType);
    }
}