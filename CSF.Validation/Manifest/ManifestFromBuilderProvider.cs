using System;
using CSF.Validation.ValidatorBuilding;

namespace CSF.Validation.Manifest
{
    /// <summary>
    /// A service which gets a <see cref="ValidationManifest"/> from an instance of <see cref="IBuildsValidator{TValidated}"/>.
    /// </summary>
    public class ManifestFromBuilderProvider : IGetsManifestFromBuilder
    {
        readonly IGetsValidatorBuilder builderFactory;

        /// <summary>
        /// Gets a validation manifest from the validator-builder.
        /// </summary>
        /// <typeparam name="TValidated">The validated object type.</typeparam>
        /// <param name="builder">The validation builder instance.</param>
        /// <returns>A validation manifest, created using the validation-builder.</returns>
        /// <exception cref="System.ArgumentNullException">If <paramref name="builder"/> is <see langword="null"/>.</exception>
        public ValidationManifest GetManifest<TValidated>(IBuildsValidator<TValidated> builder)
        {
            if (builder is null)
                throw new System.ArgumentNullException(nameof(builder));

            var validatorBuilder = builderFactory.GetValidatorBuilder<TValidated>();
            builder.ConfigureValidator(validatorBuilder);
            return ValidatorBuilderContext.GetContextProvider(validatorBuilder).Context.GetManifest();
        }

        /// <summary>
        /// Initialises a new instance of <see cref="ManifestFromBuilderProvider"/>.
        /// </summary>
        /// <param name="builderFactory">A factory for getting instances of <see cref="ValidatorBuilder{TValidated}"/>.</param>
        /// <exception cref="System.ArgumentNullException">If the <paramref name="builderFactory"/> is <see langword="null" />.</exception>
        public ManifestFromBuilderProvider(IGetsValidatorBuilder builderFactory)
        {
            this.builderFactory = builderFactory ?? throw new System.ArgumentNullException(nameof(builderFactory));
        }
    }
}