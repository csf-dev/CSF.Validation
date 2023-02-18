using System;
using System.Linq;
using System.Reflection;
using CSF.Reflection;
using CSF.Specifications;
using Microsoft.Extensions.DependencyInjection;

namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// A factory service which gets an implementation of <see cref="ValidatorBuilderContext"/> based upon a
    /// type that implements <see cref="IBuildsValidator{TValidated}"/>.
    /// </summary>
    public class ImportedValidatorBuilderManifestFactory : IGetsValidatorBuilderContextFromBuilder
    {
        static readonly ISpecificationExpression<Type> openGenericSpec
            = new IsClosedGenericBasedOnOpenGenericSpecification(typeof(IBuildsValidator<>));

        static readonly MethodInfo getValidatorManifestGenericMethod
            = Reflect.Method<ImportedValidatorBuilderManifestFactory>(x => x.GetValidatorManifestGeneric<object>(default, default)).GetGenericMethodDefinition();

        readonly IServiceProvider serviceProvider;

        IGetsValidatorBuilderContext RuleContextFactory => serviceProvider.GetRequiredService<IGetsValidatorBuilderContext>();

        IGetsRuleBuilder RuleBuilderFactory => serviceProvider.GetRequiredService<IGetsRuleBuilder>();

        IGetsValueAccessorBuilder ValueBuilderFactory => serviceProvider.GetRequiredService<IGetsValueAccessorBuilder>();

        IGetsValidatorBuilderContextFromBuilder ValidatorManifestFactory => serviceProvider.GetRequiredService<IGetsValidatorBuilderContextFromBuilder>();

        /// <summary>
        /// Gets an object which provides manifest rules from a specified validator-builder type.
        /// </summary>
        /// <param name="definitionType">A type which must implement <see cref="IBuildsValidator{TValidated}"/>.</param>
        /// <param name="context">Contextual information about how a validator should be built.</param>
        /// <returns>An object which provides a collection of <see cref="Manifest.ManifestRule"/> instances.</returns>
        /// <exception cref="ArgumentException">If the <paramref name="definitionType"/> does not implement <see cref="IBuildsValidator{TValidated}"/>.</exception>
        public ValidatorBuilderContext GetValidatorBuilderContext(Type definitionType, ValidatorBuilderContext context)
        {
            var validatedType = GetValidatedType(definitionType);

            var method = getValidatorManifestGenericMethod.MakeGenericMethod(validatedType);
            return (ValidatorBuilderContext) method.Invoke(this, new object[] { definitionType, context });
        }

        ValidatorBuilderContext GetValidatorManifestGeneric<T>(Type definitionType, ValidatorBuilderContext context)
        {
            var definition = GetValidatorBuilder<T>(definitionType);
            var builder = new ValidatorBuilder<T>(RuleContextFactory, RuleBuilderFactory, ValueBuilderFactory, ValidatorManifestFactory, context);
            definition.ConfigureValidator(builder);
            return context;
        }

        IBuildsValidator<T> GetValidatorBuilder<T>(Type definitionType)
            => serviceProvider.GetRequiredService<Bootstrap.IResolvesServices>().ResolveService<IBuildsValidator<T>>(definitionType);

        static Type GetValidatedType(Type definitionType)
        {
            if (definitionType is null)
                throw new ArgumentNullException(nameof(definitionType));

            var validatorBuilderInterfaces = definitionType
                .GetTypeInfo()
                .ImplementedInterfaces
                .Where(openGenericSpec)
                .ToList();

            if(validatorBuilderInterfaces.Count == 0)
            {
                var message = String.Format(Resources.ExceptionMessages.GetExceptionMessage("BuilderTypeMustImplementOpenGenericIBuildsValidator"), nameof(IBuildsValidator<string>));
                throw new ArgumentException(message, nameof(definitionType));
            }
            if(validatorBuilderInterfaces.Count > 1)
            {
                var message = String.Format(Resources.ExceptionMessages.GetExceptionMessage("BuilderTypeMustOnlyImplementIBuildsValidatorOnce"), nameof(IBuildsValidator<string>));
                throw new ArgumentException(message, nameof(definitionType));
            }

            return validatorBuilderInterfaces.Single().GetTypeInfo().GenericTypeArguments.Single();
        }

        /// <summary>
        /// Initialises a new instance of <see cref="ImportedValidatorBuilderManifestFactory"/>.
        /// </summary>
        /// <param name="serviceProvider">A service provider.</param>
        public ImportedValidatorBuilderManifestFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }
    }
}