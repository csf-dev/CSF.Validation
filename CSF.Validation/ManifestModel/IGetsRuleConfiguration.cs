using System;
using System.Collections.Generic;

namespace CSF.Validation.ManifestModel
{
    /// <summary>
    /// An object which can get an executable action that will configure a validation
    /// rule instance based on a series of specified property values.
    /// </summary>
    public interface IGetsRuleConfiguration
    {
        /// <summary>
        /// Gets the configuration action which sets the specified property values upon a rule instance.
        /// </summary>
        /// <param name="ruleType">The <see cref="Type"/> of validation rule for which the action should operate.</param>
        /// <param name="rulePropertyValues">A collection of named property values to be set upon the rule instance by the action.</param>
        /// <returns>An executable action which sets the specified property values into the rule instance.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="ruleType"/> is null.</exception>
        /// <exception cref="ArgumentException">
        /// If the <paramref name="rulePropertyValues"/> contains any entries which correspond to either non-existent or non-settable
        /// properties of the <paramref name="ruleType"/>.
        /// </exception>
        Action<object> GetRuleConfigurationAction(Type ruleType, IDictionary<string, object> rulePropertyValues);
    }
}