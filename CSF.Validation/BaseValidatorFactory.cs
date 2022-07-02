using System;
using CSF.Validation.Manifest;

namespace CSF.Validation
{
    /// <summary>
    /// A factory service that gets instances of <see cref="Validator{TValidated}"/>.
    /// </summary>
    public class BaseValidatorFactory : IGetsBaseValidator
    {
        readonly IGetsManifestFromBuilder manifestFromBuilderFactory;
        readonly IGetsValidatorFromManifest validatorFromManifestProvider;
        
        /// <inheritdoc/>
        public IValidator<TValidated> GetValidator<TValidated>(IBuildsValidator<TValidated> builder)
        {
            var manifest = manifestFromBuilderFactory.GetManifest<TValidated>(builder);
            return (IValidator<TValidated>) validatorFromManifestProvider.GetValidator(manifest);
        }

        /// <inheritdoc/>
        public IValidator GetValidator(ValidationManifest manifest)
            => validatorFromManifestProvider.GetValidator(manifest);

        /// <summary>
        /// Initialises a new instance of <see cref="BaseValidatorFactory"/>.
        /// </summary>
        /// <param name="manifestFromBuilderProvider">A validatio- manifest-from-builder provider.</param>
        /// <param name="validatorFromManifestFactory">A validator-from-manifest factory.</param>
        /// <exception cref="ArgumentNullException">If any parameter is <see langword="null" />.</exception>
        public BaseValidatorFactory(IGetsManifestFromBuilder manifestFromBuilderProvider, IGetsValidatorFromManifest validatorFromManifestFactory)
        {
            this.manifestFromBuilderFactory = manifestFromBuilderProvider ?? throw new ArgumentNullException(nameof(manifestFromBuilderProvider));
            this.validatorFromManifestProvider = validatorFromManifestFactory ?? throw new ArgumentNullException(nameof(validatorFromManifestFactory));
        }
    }
}