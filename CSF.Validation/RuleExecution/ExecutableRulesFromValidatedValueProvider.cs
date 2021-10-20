using System.Collections.Generic;
using System.Linq;
using CSF.Validation.Manifest;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// A service that gets a flattened collection of executable rules from a manifest value and the object to be validated.
    /// </summary>
    public class ExecutableRulesFromValidatedValueProvider : IGetsAllExecutableRules
    {
        readonly IGetsValidatedValue validatedValueProvider;

        /// <summary>
        /// Gets a flattened collection of executable validation rules from a manifest value and object to be validated.
        /// </summary>
        /// <param name="manifestValue">The manifest value.</param>
        /// <param name="objectToBeValidated">The object to be validated.</param>
        /// <returns>A flattened collection of executable rules from the manifest value and the value's descendents.</returns>
        public IReadOnlyList<ExecutableRule> GetExecutableRules(ManifestValue manifestValue, object objectToBeValidated)
        {
            var validatedValue = validatedValueProvider.GetValidatedValue(manifestValue, objectToBeValidated);
            return GetFlattenedExecutableRules(validatedValue).ToList();
        }

        /// <summary>
        /// Gets all of the executable rules from the validated value and all of that value's descendents.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This is a linear, forwards-only breadth-first search which traverses the complete hierarchy of validated values
        /// and collects all of the <see cref="ExecutableRule"/> instances from all of them.
        /// </para>
        /// </remarks>
        /// <param name="validatedValue">The root validated value for which to get all rules.</param>
        /// <returns>A flattened collection of executable rules from the manifest value.</returns>
        IEnumerable<ExecutableRule> GetFlattenedExecutableRules(ValidatedValue validatedValue)
        {
            var openList = new Queue<ValidatedValue>(new [] { validatedValue });

            while(openList.Any())
            {
                var currentValue = openList.Dequeue();

                foreach(var rule in currentValue.Rules)
                    yield return rule;
                
                foreach(var child in currentValue.ChildValues)
                    openList.Enqueue(child);
            }
        }

        /// <summary>
        /// Initialises a new instance of <see cref="ExecutableRulesFromValidatedValueProvider"/>.
        /// </summary>
        /// <param name="validatedValueProvider">The provider for a validated value.</param>
        /// <exception cref="System.ArgumentNullException">If <paramref name="validatedValueProvider"/> is <see langword="null"/>.</exception>
        public ExecutableRulesFromValidatedValueProvider(IGetsValidatedValue validatedValueProvider)
        {
            this.validatedValueProvider = validatedValueProvider ?? throw new System.ArgumentNullException(nameof(validatedValueProvider));
        }
    }
}