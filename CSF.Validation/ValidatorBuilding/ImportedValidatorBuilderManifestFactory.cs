using System;
using System.Linq;
using System.Reflection;
using CSF.Reflection;
using CSF.Specifications;
using Microsoft.Extensions.DependencyInjection;
using static CSF.Validation.Resources.ExceptionMessages;

namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// A factory service which creates an implementation of <see cref="IGetsManifestRules"/> based upon a
    /// type that implements <see cref="IBuildsValidator{TValidated}"/>.
    /// </summary>
    public class ImportedValidatorBuilderManifestFactory : IGetsValidatorManifest
    {
        static readonly ISpecificationExpression<Type> openGenericSpec
            = new IsClosedGenericBasedOnOpenGenericSpecification(typeof(IBuildsValidator<>));

        static readonly MethodInfo getValidatorManifestGenericMethod
            = Reflect.Method<ImportedValidatorBuilderManifestFactory>(x => x.GetValidatorManifestGeneric<object>(default, default)).GetGenericMethodDefinition();

        readonly IServiceProvider serviceProvider;

        IGetsValidatorBuilderContext RuleContextFactory => serviceProvider.GetService<IGetsValidatorBuilderContext>();

        IGetsRuleBuilder RuleBuilderFactory => serviceProvider.GetService<IGetsRuleBuilder>();

        IGetsValueAccessorBuilder ValueBuilderFactory => serviceProvider.GetService<IGetsValueAccessorBuilder>();

        IGetsValidatorManifest ValidatorManifestFactory => serviceProvider.GetService<IGetsValidatorManifest>();

        /// <summary>
        /// Gets an object which provides manifest rules from a specified validator-builder type.
        /// </summary>
        /// <param name="definitionType">A type which must implement <see cref="IBuildsValidator{TValidated}"/>.</param>
        /// <param name="context">Contextual information about how a validator should be built.</param>
        /// <returns>An object which provides a collection of <see cref="Manifest.ManifestRule"/> instances.</returns>
        /// <exception cref="ArgumentException">If the <paramref name="definitionType"/> does not implement <see cref="IBuildsValidator{TValidated}"/>.</exception>
        public IGetsManifestRules GetValidatorManifest(Type definitionType, ValidatorBuilderContext context)
        {
            var validatedType = GetValidatedType(definitionType);

            var method = getValidatorManifestGenericMethod.MakeGenericMethod(validatedType);
            return (IGetsManifestRules) method.Invoke(this, new object[] { definitionType, context });
        }

        IGetsManifestRules GetValidatorManifestGeneric<T>(Type definitionType, ValidatorBuilderContext context)
        {
            var definition = GetValidatorBuilder<T>(definitionType);
            var builder = new ValidatorBuilder<T>(RuleContextFactory, RuleBuilderFactory, ValueBuilderFactory, ValidatorManifestFactory, context);
            definition.ConfigureValidator(builder);
            return builder;
        }

        IBuildsValidator<T> GetValidatorBuilder<T>(Type definitionType)
        {
            var builderFromDi = serviceProvider.GetService(definitionType);
            if(builderFromDi is IBuildsValidator<T> output) return output;

            try
            {
                return (IBuildsValidator<T>) Activator.CreateInstance(definitionType);
            }
            catch(Exception e)
            {
                var message = String.Format(GetExceptionMessage("CannotGetImportedValidatorBuilder"),
                                            nameof(ImportedValidatorBuilderManifestFactory),
                                            definitionType.FullName);
                throw new ArgumentException(message, nameof(definitionType), e);
            }
        }

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