using CSF.Validation.Bootstrap;
using CSF.Validation.Rules;
using CSF.Validation.ValidatorValidation;
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
        /// Adds the standard validation rules to the service collection, so that they may be used in validators.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method adds all of the validation rule types defined in the assembly/NuGet package
        /// <c>CSF.Validation.StandardRules</c> to dependency injection, so that they may be used by validators.
        /// </para>
        /// </remarks>
        /// <param name="serviceCollection">The service collection to which the standard rules should be added.</param>
        /// <returns>The service collection, so that calls may be chained.</returns>
        public static IServiceCollection UseStandardValidationRules(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddStandardRules()
                .AddValidatorValidationServices()
                ;
        }
    }
}