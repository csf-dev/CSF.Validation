using System;
using CSF.Validation.Manifest;

namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// A factory service which gets instances of <see cref="ValidatorBuilder{TValidated}"/>.
    /// </summary>
    public class ValidatorBuilderFactory : IGetsValidatorBuilder
    {
        readonly Func<IGetsValidatorBuilderContext> ruleContextFactory;
        readonly Func<IGetsRuleBuilder> ruleBuilderFactory;
        readonly Func<IGetsValueAccessorBuilder> valueBuilderFactory;
        readonly Func<IGetsValidatorManifest> validatorManifestFactory;
        
        /// <summary>
        /// Gets the validator builder object, optionally for a specified validator builder context.
        /// </summary>
        /// <typeparam name="TValidated">The type of object to be validated.</typeparam>
        /// <param name="context">An optional validator builder context; if <see langword="null"/> then a new/empty context will be created.</param>
        /// <returns>A validator builder.</returns>
        public ValidatorBuilder<TValidated> GetValidatorBuilder<TValidated>(ValidatorBuilderContext context = null)
        {
            var builderContext = context ?? CreateEmptyContext();

            return new ValidatorBuilder<TValidated>(ruleContextFactory(),
                                                    ruleBuilderFactory(),
                                                    valueBuilderFactory(),
                                                    validatorManifestFactory(),
                                                    builderContext);
        }

        static ValidatorBuilderContext CreateEmptyContext()
        {
            var value = new ManifestValue();
            return new ValidatorBuilderContext(value);
        }

        /// <summary>
        /// Initialises a new instance of <see cref="ValidatorBuilderFactory"/>.
        /// </summary>
        /// <param name="ruleContextFactory">A factory delegate for a rule context factory.</param>
        /// <param name="ruleBuilderFactory">A factory delegate for a rule builder factory.</param>
        /// <param name="valueBuilderFactory">A factory delegate for a value-accessor builder factory.</param>
        /// <param name="validatorManifestFactory">A factory delegate for a validation manifest factory.</param>
        public ValidatorBuilderFactory(Func<IGetsValidatorBuilderContext> ruleContextFactory,
                                       Func<IGetsRuleBuilder> ruleBuilderFactory,
                                       Func<IGetsValueAccessorBuilder> valueBuilderFactory,
                                       Func<IGetsValidatorManifest> validatorManifestFactory)
        {
            this.ruleContextFactory = ruleContextFactory ?? throw new ArgumentNullException(nameof(ruleContextFactory));
            this.ruleBuilderFactory = ruleBuilderFactory ?? throw new ArgumentNullException(nameof(ruleBuilderFactory));
            this.valueBuilderFactory = valueBuilderFactory ?? throw new ArgumentNullException(nameof(valueBuilderFactory));
            this.validatorManifestFactory = validatorManifestFactory ?? throw new ArgumentNullException(nameof(validatorManifestFactory));
        }
    }
}