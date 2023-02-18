using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CSF.Reflection;
using CSF.Specifications;
using CSF.Validation.Messages;

namespace CSF.Validation.Bootstrap
{
    /// <summary>
    /// A service, intended only for use during dependency injection configuration, which provides
    /// assembly-scanning logic for validation message provider types.
    /// </summary>
    public static class MessageAssemblyScanner
    {
        /// <summary>
        /// Gets a collection of types which are exported by any of the specified <paramref name="searchAssemblies"/>
        /// and which meet the criteria to be a validation message provider class.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Validation rule classes are concrete (non-abstract) classes which derive from
        /// any of the following (the closed-generic forms of the interfaces, where applicable):
        /// </para>
        /// <list type="bullet">
        /// <item><description><see cref="IGetsFailureMessage"/></description></item>
        /// <item><description><see cref="IGetsFailureMessage{TValidated}"/></description></item>
        /// <item><description><see cref="IGetsFailureMessage{TValidated, TParent}"/></description></item>
        /// </list>
        /// </remarks>
        /// <param name="searchAssemblies">The assemblies to search.</param>
        /// <returns>A collection of validation message provider types.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="searchAssemblies"/> is <see langword="null" />.</exception>
        public static IEnumerable<Type> GetMessageProviderTypesFromAssemblies(IEnumerable<Assembly> searchAssemblies)
        {
            if (searchAssemblies is null)
                throw new ArgumentNullException(nameof(searchAssemblies));

            return searchAssemblies
                .SelectMany(x => x.ExportedTypes)
                .Where(IsValidationMessageProviderClass)
                .ToList();
        }

        static ISpecificationExpression<Type> IsValidationMessageProviderClass
        {
            get {
                var isConcreteClass = new IsConcreteClassSpecification();
                var isNotGeneric = new IsOpenGenericTypeSpecification().Not();
                var isNonGenericConcreteClass = isConcreteClass.And(isNotGeneric);
                var derivesFromNonGenericProvider = new DerivesFromSpecification(typeof(IGetsFailureMessage));
                var derivesFromSingleGenericProvider = new DerivesFromOpenGenericSpecification(typeof(IGetsFailureMessage<>));
                var derivesFromDoubleGenericProvider = new DerivesFromOpenGenericSpecification(typeof(IGetsFailureMessage<,>));

                return isNonGenericConcreteClass
                    .And(derivesFromNonGenericProvider.Or(derivesFromSingleGenericProvider).Or(derivesFromDoubleGenericProvider));
            }
        }
    }

}