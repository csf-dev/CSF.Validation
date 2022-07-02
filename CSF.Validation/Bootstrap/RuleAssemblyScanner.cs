using CSF.Specifications;
using CSF.Reflection;
using CSF.Validation.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CSF.Validation.Bootstrap
{
    /// <summary>
    /// A service, intended only for use during dependency injection configuration, which provides
    /// assembly-scanning logic for validation rule types.
    /// </summary>
    public static class RuleAssemblyScanner
    {
        /// <summary>
        /// Gets a collection of types which are exported by any of the specified <paramref name="searchAssemblies"/>
        /// and which meet the criteria to be a validation rule class.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Validation rule classes are concrete (non-abstract) classes which derive from a closed-generic form of
        /// either <see cref="IRule{TValidated}"/> or <see cref="IRule{TValue, TParent}"/>.
        /// </para>
        /// </remarks>
        /// <param name="searchAssemblies">The assemblies to search.</param>
        /// <returns>A collection of validation rule types.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="searchAssemblies"/> is <see langword="null" />.</exception>
        public static IEnumerable<Type> GetRuleTypesFromAssemblies(IEnumerable<Assembly> searchAssemblies)
        {
            if (searchAssemblies is null)
                throw new ArgumentNullException(nameof(searchAssemblies));

            return searchAssemblies
                .SelectMany(x => x.ExportedTypes)
                .Where(IsValidationRuleClass)
                .ToList();
        }

        static ISpecificationExpression<Type> IsValidationRuleClass
        {
            get {
                var isConcreteClass = new IsConcreteClassSpecification();
                var derivesFromSingleGenericRule = new DerivesFromOpenGenericSpecification(typeof(IRule<>));
                var derivesFromDoubleGenericRule = new DerivesFromOpenGenericSpecification(typeof(IRule<,>));

                return isConcreteClass
                    .And(derivesFromSingleGenericRule.Or(derivesFromDoubleGenericRule));
            }
        }
    }
}