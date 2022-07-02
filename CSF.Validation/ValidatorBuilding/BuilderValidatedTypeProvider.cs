using System;
using System.Linq;
using System.Reflection;
using CSF.Validation.Resources;
using CSF.Specifications;

namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// Default implementation of <see cref="IGetsValidatedTypeForBuilderType"/> which uses reflection
    /// and Linq to determine the builder type's generic type.
    /// </summary>
    public class BuilderValidatedTypeProvider : IGetsValidatedTypeForBuilderType
    {
        static string builderInterfaceName = $"{typeof(IBuildsValidator<>).Namespace}.{nameof(IBuildsValidator<object>)}<T>";

        static ISpecificationExpression<Type>
            isBuilderInterface = new IsClosedGenericBasedOnOpenGenericSpecification(typeof(IBuildsValidator<>));

        /// <summary>
        /// Gets the generic validated type appropriate for the specified <paramref name="builderType"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// A builder type is one which implements the interface <see cref="IBuildsValidator{TValidated}"/>.
        /// This method gets the value of the generic type parameter used for that interface.
        /// </para>
        /// <para>
        /// In order to be used with this method, the <paramref name="builderType"/> must implement
        /// <see cref="IBuildsValidator{TValidated}"/> for precisely one generic type.
        /// If the builder type implements the interface more than once then this method will be unable to return
        /// a definitive result and will throw an exception.
        /// </para>
        /// </remarks>
        /// <param name="builderType">A type that indicates a validator builder.</param>
        /// <returns>The <see cref="Type"/> for which the specified builder type validates.</returns>
        public Type GetValidatedType(Type builderType)
        {
            if (builderType is null)
                throw new ArgumentNullException(nameof(builderType));

            var validatedTypes = builderType
                .GetTypeInfo()
                .ImplementedInterfaces
                .Where(isBuilderInterface)
                .Select(x => x.GetTypeInfo().GenericTypeArguments.First())
                .ToList();
            
            if(validatedTypes.Count == 1)
                return validatedTypes.Single();
            
            if(!validatedTypes.Any())
            {
                var message = String.Format(ExceptionMessages.GetExceptionMessage("BuilderTypeDoesNotImplementBuilderInterface"),
                                            builderType.FullName,
                                            builderInterfaceName);
                throw new ArgumentException(message, nameof(builderType));
            }
            else
            {
                var message = String.Format(ExceptionMessages.GetExceptionMessage("BuilderTypeImplementsTooManyBuilderInterfaces"),
                                            builderType.FullName,
                                            builderInterfaceName);
                throw new ArgumentException(message, nameof(builderType));
            }
        }
    }
}