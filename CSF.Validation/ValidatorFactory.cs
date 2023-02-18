using System;
using System.Reflection;
using CSF.Validation.Manifest;
using CSF.Validation.ManifestModel;
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
            getValidatorPrivateMethod = typeof(ValidatorFactory).GetTypeInfo().GetDeclaredMethod(nameof(GetValidatorPrivate));

        readonly IServiceProvider serviceProvider;

        Bootstrap.IResolvesServices Resolver => serviceProvider.GetRequiredService<Bootstrap.IResolvesServices>();
        IGetsValidationManifestFromModel ManifestFromModelProvider => serviceProvider.GetRequiredService<IGetsValidationManifestFromModel>();
        IGetsValidatedTypeForBuilderType BuilderTypeProvider => serviceProvider.GetRequiredService<IGetsValidatedTypeForBuilderType>();
        IGetsBaseValidator BaseValidatorFactory => serviceProvider.GetRequiredService<IGetsBaseValidator>();
        IWrapsValidatorWithExceptionBehaviour ExceptionBehaviourWrapper => serviceProvider.GetRequiredService<IWrapsValidatorWithExceptionBehaviour>();
        IWrapsValidatorWithMessageSupport MessageSupportWrapper => serviceProvider.GetRequiredService<IWrapsValidatorWithMessageSupport>();
        IWrapsValidatorWithInstrumentationSupport InstrumentingWrapper => serviceProvider.GetRequiredService<IWrapsValidatorWithInstrumentationSupport>();

        #region Without message support

        /// <inheritdoc/>
        public IValidator GetValidator(Type builderType)
        {
            var validatedType = BuilderTypeProvider.GetValidatedType(builderType);
            var validatorBuilder = GetValidatorBuilder(builderType);
            var method = getValidatorPrivateMethod.MakeGenericMethod(validatedType);
            return (IValidator) method.Invoke(this, new[] { validatorBuilder });
        }

        /// <inheritdoc/>
        public IValidator<TValidated> GetValidator<TValidated>(IBuildsValidator<TValidated> builder)
            => GetValidatorPrivate(builder);

        /// <inheritdoc/>
        public IValidator GetValidator(ValidationManifest manifest)
        {
            var validator = BaseValidatorFactory.GetValidator(manifest);
            var messaageValidator = MessageSupportWrapper.GetValidatorWithMessageSupport(validator);
            var exceptionValidator = ExceptionBehaviourWrapper.WrapValidator(messaageValidator);
            return InstrumentingWrapper.WrapValidator(exceptionValidator);
        }

        /// <inheritdoc/>
        public IValidator GetValidator(Value manifestModel, Type validatedType)
        {
            var manifest = ManifestFromModelProvider.GetValidationManifest(manifestModel, validatedType);
            return GetValidator(manifest);
        }

        #endregion
        
        object GetValidatorBuilder(Type builderType) => Resolver.ResolveService<object>(builderType);
        
        IValidator<TValidated> GetValidatorPrivate<TValidated>(IBuildsValidator<TValidated> builder)
        {
            var validator = BaseValidatorFactory.GetValidator(builder);
            var messaageValidator = MessageSupportWrapper.GetValidatorWithMessageSupport(validator);
            var exceptionValidator = ExceptionBehaviourWrapper.WrapValidator(messaageValidator);
            return InstrumentingWrapper.WrapValidator(exceptionValidator);
        }

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