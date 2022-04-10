using System;
using System.Reflection;
using CSF.Validation.Manifest;
using CSF.Validation.ManifestModel;
using CSF.Validation.ValidatorBuilding;

namespace CSF.Validation
{
    /// <summary>
    /// A service which gets a validator from any of a number of mechanisms.
    /// </summary>
    public class ValidatorFactory : IGetsValidator
    {
        readonly Bootstrap.IResolvesServices resolver;
        readonly IGetsManifestFromBuilder manifestFromBuilderProvider;
        readonly IGetsValidationManifestFromModel manifestFromModelProvider;
        readonly IGetsValidatorFromManifest validatorFromManifestProvider;
        readonly IGetsValidatedTypeForBuilderType builderTypeProvider;

        /// <summary>
        /// Gets a validator instance using a specified builder type which specifies a validator via code.
        /// </summary>
        /// <param name="builderType">The type of a class which implements <see cref="IBuildsValidator{TValidated}"/> for the desired validated type.</param>
        /// <returns>A validator implementation.</returns>
        /// <exception cref="ArgumentNullException">If the <paramref name="builderType"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">If the <paramref name="builderType"/> is not a concrete (non-abstract) class
        /// derived from <see cref="IBuildsValidator{TValidated}"/> or if it implements that interface more than once for different generic types.</exception>
        public IValidator GetValidator(Type builderType)
        {
            var validatedType = builderTypeProvider.GetValidatedType(builderType);
            var validatorBuilder = GetValidatorBuilder(builderType);

            var method = GetType().GetTypeInfo().GetDeclaredMethod(nameof(GetValidatorPrivate)).MakeGenericMethod(validatedType);
            return (IValidator) method.Invoke(this, new[] { validatorBuilder });
        }

        /// <summary>
        /// Gets a validator instance using a specified validator-builder instance which specifies a validator via code.
        /// </summary>
        /// <typeparam name="TValidated">The type of object to be validated.</typeparam>
        /// <param name="builder">An instance of a validator-builder.</param>
        /// <returns>A strongly-typed validator implementation.</returns>
        public IValidator<TValidated> GetValidator<TValidated>(IBuildsValidator<TValidated> builder)
            => GetValidatorPrivate(builder);

        /// <summary>
        /// Gets a validator instance using a specified validation manifest.
        /// A validation manifest is a model which may specify a validator using data.
        /// </summary>
        /// <param name="manifest">The validation manifest.</param>
        /// <returns>A validator implementation.</returns>
        /// <exception cref="ArgumentNullException">If the <paramref name="manifest"/> is <see langword="null"/>.</exception>
        /// <exception cref="ValidatorBuildingException">If the <paramref name="manifest"/> is not valid to create a validator instance.</exception>
        public IValidator GetValidator(ValidationManifest manifest) => validatorFromManifestProvider.GetValidator(manifest);

        /// <summary>
        /// Gets a validator instance using a specified validation manifest.
        /// A validation manifest is a model which may specify a validator using data.
        /// This overload uses a simplified/serialization-friendly model.
        /// </summary>
        /// <param name="manifestModel">A simplified validation manifest model.</param>
        /// <param name="validatedType">The type of object to be validated.</param>
        /// <returns>A validator implementation.</returns>
        /// <exception cref="ArgumentNullException">If the <paramref name="manifestModel"/> is <see langword="null"/>.</exception>
        /// <exception cref="ValidatorBuildingException">If the <paramref name="manifestModel"/> is not valid to create a validator instance.</exception>
        public IValidator GetValidator(Value manifestModel, Type validatedType)
        {
            var manifest = manifestFromModelProvider.GetValidationManifest(manifestModel, validatedType);
            return GetValidator(manifest);
        }

        IValidator<TValidated> GetValidatorPrivate<TValidated>(IBuildsValidator<TValidated> builder)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            var manifest = manifestFromBuilderProvider.GetManifest<TValidated>(builder);
            return (IValidator<TValidated>) GetValidator(manifest);
        }

        object GetValidatorBuilder(Type builderType) => resolver.ResolveService<object>(builderType);

        /// <summary>
        /// Initialises a new instance of <see cref="ValidatorFactory"/>.
        /// </summary>
        /// <param name="resolver">A DI resolver.</param>
        /// <param name="manifestFromBuilderProvider">A service to get a validation manifest from a builder.</param>
        /// <param name="manifestFromModelProvider">A service to get a validation manifest from a model.</param>
        /// <param name="validatorFromManifestProvider">A service to get a validator from a validation manifest.</param>
        /// <param name="builderTypeProvider">A service that gets the generic type for a validator builder.</param>
        /// <exception cref="ArgumentNullException">If any parameter is <see langword="null" />.</exception>
        public ValidatorFactory(Bootstrap.IResolvesServices resolver,
                                IGetsManifestFromBuilder manifestFromBuilderProvider,
                                IGetsValidationManifestFromModel manifestFromModelProvider,
                                IGetsValidatorFromManifest validatorFromManifestProvider,
                                IGetsValidatedTypeForBuilderType builderTypeProvider)
        {
            this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            this.manifestFromBuilderProvider = manifestFromBuilderProvider ?? throw new ArgumentNullException(nameof(manifestFromBuilderProvider));
            this.manifestFromModelProvider = manifestFromModelProvider ?? throw new ArgumentNullException(nameof(manifestFromModelProvider));
            this.validatorFromManifestProvider = validatorFromManifestProvider ?? throw new ArgumentNullException(nameof(validatorFromManifestProvider));
            this.builderTypeProvider = builderTypeProvider ?? throw new ArgumentNullException(nameof(builderTypeProvider));
        }
    }
}