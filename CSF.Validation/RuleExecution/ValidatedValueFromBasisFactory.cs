using System.Linq;
using CSF.Validation.Manifest;
using CSF.Validation.Rules;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// A service which creates an instance of <see cref="ValidatedValue"/> from a <see cref="ValidatedValueBasis"/>.
    /// </summary>
    public class ValidatedValueFromBasisFactory : IGetsValidatedValueFromBasis
    {
        readonly IGetsValidationLogic validationLogicFactory;

        /// <summary>
        /// Gets the validated value.
        /// </summary>
        /// <param name="basis">A validated-value basis.</param>
        /// <returns>A validated value.</returns>
        public ValidatedValue GetValidatedValue(ValidatedValueBasis basis)
        {
            var valueIdentity = GetIdentity(basis);
            
            var value = new ValidatedValue
            {
                ManifestValue = basis.ManifestValue,
                ValueResponse = basis.ValidatedValueResponse,
                ValueIdentity = valueIdentity,
                ParentValue = basis.Parent,
                CollectionItemOrder = basis.CollectionOrder,
            };

            value.Rules = basis.GetManifestRules()
                .Select(manifestRule => new ExecutableRule
                        {
                            ValidatedValue = value,
                            ManifestRule = manifestRule,
                            RuleLogic = validationLogicFactory.GetValidationLogic(manifestRule),
                            RuleIdentifier = new RuleIdentifier(manifestRule, valueIdentity),
                        })
                .ToList();

            return value;
        }

        static object GetIdentity(ValidatedValueBasis basis)
        {
            var actualValue = basis.GetActualValue();
            if(actualValue is null) return null;

            return basis.ManifestValue.IdentityAccessor is null
                ? null
                : basis.ManifestValue.IdentityAccessor(actualValue);
        }

        /// <summary>
        /// Initialises a new instance of <see cref="ValidatedValueFromBasisFactory"/>.
        /// </summary>
        /// <param name="validationLogicFactory">A validation logic factory.</param>
        /// <exception cref="System.ArgumentNullException">If <paramref name="validationLogicFactory"/> is <see langword="null" />.</exception>
        public ValidatedValueFromBasisFactory(IGetsValidationLogic validationLogicFactory)
        {
            this.validationLogicFactory = validationLogicFactory ?? throw new System.ArgumentNullException(nameof(validationLogicFactory));
        }
    }
}