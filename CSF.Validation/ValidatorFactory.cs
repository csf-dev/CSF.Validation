using System;
using System.Reflection;
using CSF.Validation.Manifest;
using CSF.Validation.ManifestModel;
using CSF.Validation.Messages;
using CSF.Validation.ValidatorBuilding;
using Microsoft.Extensions.DependencyInjection;

namespace CSF.Validation
{
    /// <summary>
    /// A service which gets a validator from any of a number of mechanisms.
    /// </summary>
    public class ValidatorFactory : IGetsValidator
    {
        static readonly MethodInfo
            getValidatorPrivateMethod = typeof(ValidatorFactory).GetTypeInfo().GetDeclaredMethod(nameof(GetValidatorPrivate)),
            getValidatorWithMessageSupportPrivateMethod = typeof(ValidatorFactory).GetTypeInfo().GetDeclaredMethod(nameof(GetValidatorWithMessageSupportPrivate));

        readonly IServiceProvider serviceProvider;
        
        Bootstrap.IResolvesServices Resolver => serviceProvider.GetService<Bootstrap.IResolvesServices>();

        IGetsManifestFromBuilder ManifestFromBuilderProvider => serviceProvider.GetService<IGetsManifestFromBuilder>();

        IGetsValidationManifestFromModel ManifestFromModelProvider => serviceProvider.GetService<IGetsValidationManifestFromModel>();

        IGetsValidatorFromManifest ValidatorFromManifestProvider => serviceProvider.GetService<IGetsValidatorFromManifest>();

        IGetsValidatedTypeForBuilderType BuilderTypeProvider => serviceProvider.GetService<IGetsValidatedTypeForBuilderType>();

        IAddsFailureMessagesToResult FailureMessageEnricher => serviceProvider.GetService<IAddsFailureMessagesToResult>();

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
            var validatedType = BuilderTypeProvider.GetValidatedType(builderType);
            var validatorBuilder = GetValidatorBuilder(builderType);

            var method = getValidatorPrivateMethod.MakeGenericMethod(validatedType);
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
        public IValidator GetValidator(ValidationManifest manifest) => ValidatorFromManifestProvider.GetValidator(manifest);

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
            var manifest = ManifestFromModelProvider.GetValidationManifest(manifestModel, validatedType);
            return GetValidator(manifest);
        }

        IValidator<TValidated> GetValidatorPrivate<TValidated>(IBuildsValidator<TValidated> builder)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            var manifest = ManifestFromBuilderProvider.GetManifest<TValidated>(builder);
            return (IValidator<TValidated>) GetValidator(manifest);
        }

        object GetValidatorBuilder(Type builderType) => Resolver.ResolveService<object>(builderType);

        /// <summary>
        /// Gets a validator instance which includes failure message support, using a specified builder type which specifies a validator via code.
        /// </summary>
        /// <param name="builderType">The type of a class which implements <see cref="IBuildsValidator{TValidated}"/> for the desired validated type.</param>
        /// <returns>A validator implementation.</returns>
        /// <exception cref="ArgumentNullException">If the <paramref name="builderType"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">If the <paramref name="builderType"/> is not a concrete (non-abstract) class
        /// derived from <see cref="IBuildsValidator{TValidated}"/> or if it implements that interface more than once for different generic types.</exception>
        public IValidatorWithMessages GetValidatorWithMessageSupport(Type builderType)
        {
            var validator = GetValidator(builderType);
            var method = getValidatorWithMessageSupportPrivateMethod.MakeGenericMethod(validator.ValidatedType);
            return (IValidatorWithMessages) method.Invoke(this, new[] { validator });
        }

        /// <summary>
        /// Gets a validator instance which includes failure message support, using a specified validator-builder instance which specifies a validator via code.
        /// </summary>
        /// <typeparam name="TValidated">The type of object to be validated.</typeparam>
        /// <param name="builder">An instance of a validator-builder.</param>
        /// <returns>A strongly-typed validator implementation.</returns>
        public IValidatorWithMessages<TValidated> GetValidatorWithMessageSupport<TValidated>(IBuildsValidator<TValidated> builder)
        {
            var validator = GetValidator(builder);
            return GetValidatorWithMessageSupportPrivate(validator);
        }

        /// <summary>
        /// Gets a validator instance which includes failure message support, using a specified validation manifest.
        /// A validation manifest is a model which may specify a validator using data.
        /// </summary>
        /// <param name="manifest">The validation manifest.</param>
        /// <returns>A validator implementation.</returns>
        /// <exception cref="ArgumentNullException">If the <paramref name="manifest"/> is <see langword="null"/>.</exception>
        /// <exception cref="ValidatorBuildingException">If the <paramref name="manifest"/> is not valid to create a validator instance.</exception>
        public IValidatorWithMessages GetValidatorWithMessageSupport(Manifest.ValidationManifest manifest)
        {
            var validator = GetValidator(manifest);
            var method = getValidatorWithMessageSupportPrivateMethod.MakeGenericMethod(validator.ValidatedType);
            return (IValidatorWithMessages) method.Invoke(this, new[] { validator });
        }

        /// <summary>
        /// Gets a validator instance which includes failure message support, using a specified validation manifest.
        /// A validation manifest is a model which may specify a validator using data.
        /// This overload uses a simplified/serialization-friendly model.
        /// </summary>
        /// <param name="manifestModel">A simplified validation manifest model.</param>
        /// <param name="validatedType">The type of object to be validated.</param>
        /// <returns>A validator implementation.</returns>
        /// <exception cref="ArgumentNullException">If the <paramref name="manifestModel"/> is <see langword="null"/>.</exception>
        /// <exception cref="ValidatorBuildingException">If the <paramref name="manifestModel"/> is not valid to create a validator instance.</exception>
        public IValidatorWithMessages GetValidatorWithMessageSupport(ManifestModel.Value manifestModel, Type validatedType)
        {
            var validator = GetValidator(manifestModel, validatedType);
            var method = getValidatorWithMessageSupportPrivateMethod.MakeGenericMethod(validator.ValidatedType);
            return (IValidatorWithMessages) method.Invoke(this, new[] { validator });
        }

        IValidatorWithMessages<TValidated> GetValidatorWithMessageSupportPrivate<TValidated>(IValidator<TValidated> validator)
            => new MessageEnrichingValidatorAdapter<TValidated>(validator, FailureMessageEnricher);

        /// <summary>
        /// Initialises a new instance of <see cref="ValidatorFactory"/>.
        /// </summary>
        /// <param name="serviceProvider">A service provider.</param>
        /// <exception cref="ArgumentNullException">If the <paramref name="serviceProvider"/> is <see langword="null" />.</exception>
        public ValidatorFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }
    }
}