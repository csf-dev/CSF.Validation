using System;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Manifest;
using CSF.Validation.ManifestModel;

namespace CSF.Validation.ValidatorValidation
{
    /// <summary>
    /// Default implementation of <see cref="IValidatesValidationManifest"/>.
    /// </summary>
    public class ValidationManifestValidator : IValidatesValidationManifest
    {
        readonly IGetsValidator validatorFactory;
        readonly ValidationManifestValidatorBuilder builder;
        readonly IGetsManifestFromBuilder manifestFromBuilderProvider;
        readonly IGetsValidationManifestFromModel manifestFromModelProvider;

        /// <inheritdoc/>
        public Task<IQueryableValidationResult<ValidationManifest>> ValidateAsync(ValidationManifest manifest,
                                                                                  ValidationOptions options = null,
                                                                                  CancellationToken cancellationToken = default)
        {
            var validator = validatorFactory.GetValidator(builder);
            return validator.ValidateAsync(manifest, options, cancellationToken);
        }

        /// <inheritdoc/>
        public Task<IQueryableValidationResult<ValidationManifest>> ValidateAsync<T>(IBuildsValidator<T> builder, ValidationOptions options = null, CancellationToken cancellationToken = default)
            => ValidateAsync(manifestFromBuilderProvider.GetManifest(builder), options, cancellationToken);

        /// <inheritdoc/>
        public Task<IQueryableValidationResult<ValidationManifest>> ValidateAsync(Value model, Type validatedType, ValidationOptions options = null, CancellationToken cancellationToken = default)
            => ValidateAsync(manifestFromModelProvider.GetValidationManifest(model, validatedType), options, cancellationToken);

        /// <summary>
        /// Initialises an instance of <see cref="ValidationManifestValidator"/>.
        /// </summary>
        /// <param name="validatorFactory">A validator factory.</param>
        /// <param name="builder">The builder for a validation manifest validator.</param>
        /// <param name="manifestFromBuilderProvider">A provider that gets a validation manifest from a builder.</param>
        /// <param name="manifestFromModelProvider">A provider that gets a validation manifest from a manifest model.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public ValidationManifestValidator(IGetsValidator validatorFactory,
                                           ValidationManifestValidatorBuilder builder,
                                           IGetsManifestFromBuilder manifestFromBuilderProvider,
                                           IGetsValidationManifestFromModel manifestFromModelProvider)
        {
            this.validatorFactory = validatorFactory ?? throw new System.ArgumentNullException(nameof(validatorFactory));
            this.builder = builder ?? throw new System.ArgumentNullException(nameof(builder));
            this.manifestFromBuilderProvider = manifestFromBuilderProvider ?? throw new System.ArgumentNullException(nameof(manifestFromBuilderProvider));
            this.manifestFromModelProvider = manifestFromModelProvider ?? throw new System.ArgumentNullException(nameof(manifestFromModelProvider));
        }
    }
}