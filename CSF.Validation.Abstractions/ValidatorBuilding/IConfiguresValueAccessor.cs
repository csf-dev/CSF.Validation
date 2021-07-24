using System;
using CSF.Validation.Rules;

namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// A builder service which configures how a value (derived from a different validated object) should be
    /// validated.
    /// </summary>
    /// <typeparam name="TValidated">The type of the validated object.</typeparam>
    /// <typeparam name="TValue">The type of the value, derived from the <typeparamref name="TValidated"/> object.</typeparam>
    public interface IConfiguresValueAccessor<TValidated, TValue> : IConfiguresObjectIdentity<TValue>
    {

        /// <summary>
        /// Adds a "value validation rule" to validate the value &amp; the validated object instance.
        /// The rule type must be a class that implements <see cref="IValueRule{TValue, TValidated}"/> for the same
        /// (or compatible contravariant) generic types <typeparamref name="TValue"/> &amp; <typeparamref name="TValidated"/>.
        /// </summary>
        /// <typeparam name="TRule">The concrete type of the validation rule.</typeparam>
        /// <param name="ruleDefinition">An optional action which defines &amp; configures the validation rule.</param>
        void AddRule<TRule>(Action<IConfiguresRule<TRule>> ruleDefinition = default) where TRule : IValueRule<TValue,TValidated>;

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
        void AddRules<TBuilder>() where TBuilder : IBuildsValidator<TValue>;
    }
}