using System;

namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// A factory service which gets instances of <see cref="IBuildsValueAccessor{TValidated, TValue}"/>.
    /// </summary>
    public class ValueAccessorBuilderFactory : IGetsValueAccessorBuilder
    {
        readonly Func<IGetsRuleBuilder> ruleBuilderFactory;
        readonly Func<IGetsValidatorManifest> validatorManifestFactory;
        readonly Func<IGetsValidatorBuilderContext> contextFactory;

        /// <summary>
        /// Gets a builder for validating a value of a validated object (typically retrieved via member access).
        /// </summary>
        /// <typeparam name="TValidated">The type of the primary object under validation.</typeparam>
        /// <typeparam name="TValue">The type of the derived value to be validated.</typeparam>
        /// <param name="ValidatorBuilderContext">Contextual information about how validation rules should be built.</param>
        /// <param name="valueConfig">An action which configures the value accessor-builder.</param>
        /// <returns>A builder for validating the derived value.</returns>
        public IBuildsValueAccessor<TValidated, TValue> GetValueAccessorBuilder<TValidated, TValue>(ValidatorBuilderContext ValidatorBuilderContext,
                                                                                                    Action<IConfiguresValueAccessor<TValidated, TValue>> valueConfig)
        {
            var builder = new ValueAccessorBuilder<TValidated, TValue>(ValidatorBuilderContext,
                                                                       ruleBuilderFactory(),
                                                                       validatorManifestFactory(),
                                                                       contextFactory());
            if(!(valueConfig is null))
                valueConfig(builder);

            return builder;
        }

        /// <summary>
        /// Initialises a new instance of <see cref="ValueAccessorBuilderFactory"/>.
        /// </summary>
        /// <param name="ruleBuilderFactory">A function which gets a rule builder factory.</param>
        /// <param name="validatorManifestFactory">A function which gets a validator manifest factory.</param>
        /// <param name="contextFactory">A function which gets a validation context factory.</param>
        public ValueAccessorBuilderFactory(Func<IGetsRuleBuilder> ruleBuilderFactory,
                                           Func<IGetsValidatorManifest> validatorManifestFactory,
                                           Func<IGetsValidatorBuilderContext> contextFactory)
        {
            this.ruleBuilderFactory = ruleBuilderFactory ?? throw new ArgumentNullException(nameof(ruleBuilderFactory));
            this.validatorManifestFactory = validatorManifestFactory ?? throw new ArgumentNullException(nameof(validatorManifestFactory));
            this.contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        }
    }
}