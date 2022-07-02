using System;
using CSF.Validation.ValidatorBuilding;
using static CSF.Validation.Resources.ExceptionMessages;

namespace CSF.Validation
{
    /// <summary>
    /// Extension methods for instances of <see cref="IConfiguresRule{TRule}"/>.
    /// </summary>
    public static class RuleBuilderExtensions
    {
        /// <summary>
        /// Adds a dependency to the <see cref="IConfiguresRule{TRule}.Dependencies"/> collection using a
        /// helper/builder function to specify the relative rule identity.
        /// Use of this method is equivalent to adding to the dependencies collection with an
        /// appropriately-constructed instance of <see cref="RelativeRuleIdentifier"/>.
        /// </summary>
        /// <typeparam name="T">The type of the rule being configured.</typeparam>
        /// <param name="ruleBuilder">The rule builder object.</param>
        /// <param name="dependencyBuilderFunc">A function which specifies the relative identifier of the rule being added as a dependency.</param>
        public static void AddDependency<T>(this IConfiguresRule<T> ruleBuilder, Func<IBuildsRelativeRuleIdentifierType,IBuildsRelativeRuleIdentifier> dependencyBuilderFunc)
        {
            if (ruleBuilder is null)
                throw new ArgumentNullException(nameof(ruleBuilder));
            if (dependencyBuilderFunc is null)
                throw new ArgumentNullException(nameof(dependencyBuilderFunc));

            var dependencyBuilder = new RelativeRuleIdentifierBuilder();
            var returnedBuilder = dependencyBuilderFunc(dependencyBuilder);

            // This is technically an LSP violation but should be OK here
            if(!(returnedBuilder is RelativeRuleIdentifierBuilder relativeIdentifierProvider))
            {
                var message = String.Format(GetExceptionMessage("RelativeIdentityBuilderFuncMustReturnCorrectType"), nameof(RelativeRuleIdentifierBuilder));
                throw new ArgumentException(message, nameof(dependencyBuilderFunc));
            }

            ruleBuilder.Dependencies.Add(relativeIdentifierProvider.GetIdentifier());
        }
    }
}