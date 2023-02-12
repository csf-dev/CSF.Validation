using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CSF.Validation.Rules;
using Microsoft.Extensions.DependencyInjection;

namespace CSF.Validation.Bootstrap
{
    internal static class StandardRulesServiceCollectionExtensions
    {
        internal static IServiceCollection AddStandardRules(this IServiceCollection serviceCollection)
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