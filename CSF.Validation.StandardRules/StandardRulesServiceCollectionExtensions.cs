using CSF.Validation.Rules;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CSF.Validation
{
    /// <summary>
    /// Extension methods to add validation to a dependency injection container.
    /// </summary>
    public static class StandardRulesServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the standard validation rules to the service collection, so that they may be used.
        /// </summary>
        /// <param name="serviceCollection">The service collection to which the standard rules should be added.</param>
        /// <returns>The service collection, so that calls may be chained.</returns>
        public static IServiceCollection UseStandardValidationRules(this IServiceCollection serviceCollection)
        {
            var ruleTypes = GetRuleTypes();

            foreach(var type in ruleTypes)
                serviceCollection.AddTransient(type);

            return serviceCollection;
        }

        static IEnumerable<Type> GetRuleTypes()
        {
            return (from type in typeof(StandardRulesServiceCollectionExtensions).GetTypeInfo().Assembly.DefinedTypes
                    where type.Namespace == typeof(NotNull).Namespace
                    select type.AsType())
                .ToList();

        }
    }
}