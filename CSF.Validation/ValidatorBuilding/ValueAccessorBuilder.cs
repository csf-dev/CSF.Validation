using System;
using System.Collections.Generic;
using System.Linq;
using CSF.Validation.Manifest;
using CSF.Validation.Rules;

namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// A builder which is used to configure how a member or other value of an object should be validated.
    /// </summary>
    /// <typeparam name="TValidated">The type of the overall object being validated.</typeparam>
    /// <typeparam name="TValue">The type of this specific value being validated.</typeparam>
    public class ValueAccessorBuilder<TValidated, TValue> : IBuildsValueAccessor<TValidated, TValue>
    {
        readonly ValidatorBuilderContext context;
        readonly IGetsRuleBuilder ruleBuilderFactory;
        readonly IGetsValidatorManifest validatorManifestFactory;
        readonly ICollection<IGetsManifestRules> ruleBuilders = new HashSet<IGetsManifestRules>();

        /// <summary>
        /// Adds a "value validation rule" to validate the value &amp; the validated object instance.
        /// The rule type must be a class that implements <see cref="IValueRule{TValue, TValidated}"/> for the same
        /// (or compatible contravariant) generic types <typeparamref name="TValue"/> &amp; <typeparamref name="TValidated"/>.
        /// </summary>
        /// <typeparam name="TRule">The concrete type of the validation rule.</typeparam>
        /// <param name="ruleDefinition">An optional action which defines &amp; configures the validation rule.</param>
        /// <returns>A reference to the same builder object, enabling chaining of calls if desired.</returns>
        public IConfiguresValueAccessor<TValidated, TValue> AddRule<TRule>(Action<IConfiguresRule<TRule>> ruleDefinition = null) where TRule : IValueRule<TValue, TValidated>
        {
            var ruleBuilder = ruleBuilderFactory.GetRuleBuilder(context, ruleDefinition);
            ruleBuilders.Add(ruleBuilder);
            return this;
        }

        /// <summary>
        /// Adds/imports rules from an object that implements <see cref="IBuildsValidator{TValidated}"/> for the generic
        /// type <typeparamref name="TValue"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This allows composition of validators, reuse of validation rules across differing validation scenarios and
        /// additionally the building of validators which operate across complex object graphs.
        /// All of the rules specified in the selected builder-type will be imported and added to the current validator,
        /// validating the type <typeparamref name="TValue"/>.
        /// </para>
        /// </remarks>
        /// <typeparam name="TBuilder">
        /// The type of a class implementing <see cref="IBuildsValidator{TValue}"/>, specifying how a validator should be built.
        /// </typeparam>
        /// <returns>A reference to the same builder object, enabling chaining of calls if desired.</returns>
        public IConfiguresValueAccessor<TValidated, TValue> AddRules<TBuilder>() where TBuilder : IBuildsValidator<TValue>
        {
            var importedRules = validatorManifestFactory.GetValidatorManifest(typeof(TBuilder), context);
            ruleBuilders.Add(importedRules);
            return this;
        }

        /// <summary>
        /// Gets a collection of manifest rules, aggregating all of the rules from all of the rule-builders
        /// which are referenced by this instance.
        /// </summary>
        /// <returns>A collection of manifest rules.</returns>
        public IEnumerable<ManifestRule> GetManifestRules()
            => ruleBuilders.SelectMany(x => x.GetManifestRules()).ToList();

        /// <summary>
        /// Initializes a new instance of <see cref="ValueAccessorBuilder{TValidated, TValue}"/>.
        /// </summary>
        /// <param name="context">The context which should be used for newly-added rule-builders.</param>
        /// <param name="ruleBuilderFactory">A factory for rule-builder instances.</param>
        /// <param name="validatorManifestFactory">A factory for validator manifest instances.</param>
        public ValueAccessorBuilder(ValidatorBuilderContext context, IGetsRuleBuilder ruleBuilderFactory, IGetsValidatorManifest validatorManifestFactory)
        {
            this.validatorManifestFactory = validatorManifestFactory ?? throw new ArgumentNullException(nameof(validatorManifestFactory));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.ruleBuilderFactory = ruleBuilderFactory ?? throw new ArgumentNullException(nameof(ruleBuilderFactory));
        }
    }
}