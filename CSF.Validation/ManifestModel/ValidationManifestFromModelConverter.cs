using System;
using CSF.Validation.Manifest;

namespace CSF.Validation.ManifestModel
{
    /// <summary>
    /// A service which converts instances of <see cref="Value"/> into instances of <see cref="ValidationManifest"/>.
    /// </summary>
    public class ValidationManifestFromModelConverter : IGetsValidationManifestFromModel
    {
        readonly IConvertsModelValuesToManifestValues valueConverter;
        readonly IConvertsModelRulesToManifestRules ruleConverter;

        /// <summary>
        /// Gets a validation manifest for validating a specified type, from the specified simple validation model.
        /// </summary>
        /// <param name="rootValue">The <see cref="Value"/> that represents the primary object to be validated.</param>
        /// <param name="validatedType">The type of the primary object to be validated.</param>
        /// <returns>A validation manifest.</returns>
        public ValidationManifest GetValidationManifest(Value rootValue, Type validatedType)
        {
            var context = new ModelToManifestConversionContext
            {
                CurrentValue = rootValue ?? throw new ArgumentNullException(nameof(rootValue)),
                ValidatedType = validatedType ?? throw new ArgumentNullException(nameof(validatedType)),
            };

            var valueConversionResult = valueConverter.ConvertAllValues(context);
            ruleConverter.ConvertAllRulesAndAddToManifestValues(valueConversionResult.ConvertedValues);

            return new ValidationManifest
            {
                RootValue = valueConversionResult.RootValue,
                ValidatedType = validatedType,
            };
        }

        /// <summary>
        /// Initialises an instance of <see cref="ValidationManifestFromModelConverter"/>.
        /// </summary>
        /// <param name="valueConverter">A conversion service for model values to manifest values.</param>
        /// <param name="ruleConverter">A conversion service for model rules to manifest rules.</param>
        public ValidationManifestFromModelConverter(IConvertsModelValuesToManifestValues valueConverter,
                                                    IConvertsModelRulesToManifestRules ruleConverter)
        {
            this.valueConverter = valueConverter ?? throw new ArgumentNullException(nameof(valueConverter));
            this.ruleConverter = ruleConverter ?? throw new ArgumentNullException(nameof(ruleConverter));
        }
    }
}