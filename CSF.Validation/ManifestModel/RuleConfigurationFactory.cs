using System;
using System.Collections.Generic;
using System.Linq;

namespace CSF.Validation.ManifestModel
{
    /// <summary>
    /// A factory service which can get an executable action that will configure a validation
    /// rule instance based on a series of specified property values.
    /// </summary>
    public class RuleConfigurationFactory : IGetsRuleConfiguration
    {
        readonly IGetsPropertySetterAction setterFactory;

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
        public Action<object> GetRuleConfigurationAction(Type ruleType, IDictionary<string, object> rulePropertyValues)
        {
            var setters = rulePropertyValues
                .Select(nameAndValue => new { Setter = setterFactory.GetSetterAction(ruleType, nameAndValue.Key), nameAndValue.Value })
                .ToList();

            return obj =>
            {
                if (obj is null)
                    throw new ArgumentNullException(nameof(obj));
                
                foreach (var setter in setters)
                    setter.Setter(obj, setter.Value);
            };
        }

        /// <summary>
        /// Initialises an instance of <see cref="RuleConfigurationFactory"/>.
        /// </summary>
        /// <param name="setterFactory"></param>
        public RuleConfigurationFactory(IGetsPropertySetterAction setterFactory)
        {
            this.setterFactory = setterFactory ?? throw new ArgumentNullException(nameof(setterFactory));
        }
    }
}