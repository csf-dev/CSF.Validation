using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CSF.Reflection;
using CSF.Specifications;

namespace CSF.Validation.Bootstrap
{
    /// <summary>
    /// A service, intended only for use during dependency injection configuration, which provides
    /// assembly-scanning logic for validation message provider types.
    /// </summary>
    public static class BuilderAssemblyScanner
    {
        /// <summary>
        /// Gets a collection of types which are exported by any of the specified <paramref name="searchAssemblies"/>
        /// and which meet the criteria to be a validator builder.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Validator builder classes are concrete (non-abstract) classes which derive from
        /// <see cref="IBuildsValidator{TValidated}"/>, for any validated type.
        /// </para>
        /// </remarks>
        /// <param name="searchAssemblies">The assemblies to search.</param>
        /// <returns>A collection of validator builder types.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="searchAssemblies"/> is <see langword="null" />.</exception>
        public static IEnumerable<Type> GetValidatorBuilderTypesFromAssemblies(IEnumerable<Assembly> searchAssemblies)
        {
            if (searchAssemblies is null)
                throw new ArgumentNullException(nameof(searchAssemblies));

            return searchAssemblies
                .SelectMany(x => x.ExportedTypes)
                .Where(IsValidatorBuilderClass)
                .ToList();
        }

        static ISpecificationExpression<Type> IsValidatorBuilderClass
        {
            get {
                var isConcreteClass = new IsConcreteClassSpecification();
                var derivesFromBuilder = new DerivesFromOpenGenericSpecification(typeof(IBuildsValidator<>));

                return isConcreteClass.And(derivesFromBuilder);
            }
        }
    }
}