using System;
using CSF.Validation.Manifest;
using CSF.Validation.Rules;

namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// A builder service which configures how a value (retrieved from a different validated object) should be
    /// validated.
    /// </summary>
    /// <typeparam name="TValidated">The type of the validated object.</typeparam>
    /// <typeparam name="TValue">The type of the value, retrieved from the <typeparamref name="TValidated"/> object.</typeparam>
    public interface IConfiguresValueAccessor<TValidated, TValue>
    {
        /// <summary>
        /// Adds a "value validation rule" to validate the value &amp; the validated object instance.
        /// The rule type must be a class that implements <see cref="IRule{TValue, TValidated}"/> for the same
        /// (or compatible contravariant) generic types <typeparamref name="TValue"/> &amp; <typeparamref name="TValidated"/>.
        /// </summary>
        /// <typeparam name="TRule">The concrete type of the validation rule.</typeparam>
        /// <param name="ruleDefinition">An optional action which defines &amp; configures the validation rule.</param>
        /// <returns>A reference to the same builder object, enabling chaining of calls if desired.</returns>
        IConfiguresValueAccessor<TValidated, TValue> AddRuleWithParent<TRule>(Action<IConfiguresRule<TRule>> ruleDefinition = default) where TRule : IRule<TValue,TValidated>;

        /// <summary>
        /// Adds a validation rule to validate the value indicated by the value accessor.
        /// The rule type must be a class that implements <see cref="IRule{TValue}"/> for the same
        /// (or compatible contravariant) generic type <typeparamref name="TValue"/>.
        /// </summary>
        /// <typeparam name="TRule">The concrete type of the validation rule.</typeparam>
        /// <param name="ruleDefinition">An optional action which defines &amp; configures the validation rule.</param>
        /// <returns>A reference to the same builder object, enabling chaining of calls if desired.</returns>
        IConfiguresValueAccessor<TValidated, TValue> AddRule<TRule>(Action<IConfiguresRule<TRule>> ruleDefinition = default) where TRule : IRule<TValue>;

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
        IConfiguresValueAccessor<TValidated, TValue> AddRules<TBuilder>() where TBuilder : IBuildsValidator<TValue>;

        /// <summary>
        /// Configures the validator with a behaviour to use should the value-accessor for the current value raise an exception..
        /// </summary>
        /// <remarks>
        /// <para>
        /// This option will override the behaviour specified at <see cref="ResolvedValidationOptions.AccessorExceptionBehaviour"/>
        /// for the current value, if the specified behaviour is any non-<see langword="null" /> value.
        /// If this method is unused then it will be treated as if the specified <see cref="ValueAccessExceptionBehaviour"/>
        /// specified at this point were <see langword="null" /> and the <see cref="ResolvedValidationOptions.AccessorExceptionBehaviour"/>
        /// will be used instead.
        /// </para>
        /// </remarks>
        /// <seealso cref="ResolvedValidationOptions.AccessorExceptionBehaviour"/>
        /// <returns>A reference to the same builder object, enabling chaining of calls if desired.</returns>
        IConfiguresValueAccessor<TValidated, TValue> AccessorExceptionBehaviour(ValueAccessExceptionBehaviour? behaviour);

        /// <summary>
        /// Adds validation configuration for polymorphic validatation when the runtime type
        /// of the validated value is <typeparamref name="TDerived"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Where the current validator configuration works to validate an object of type <typeparamref name="TValue"/>,
        /// the value being validated might be a derived type of <typeparamref name="TValue"/> at runtime.
        /// Polymorphic validation allows a developer to specify how a derived type should be validated, if that is the runtime
        /// type of the value to be validated.
        /// </para>
        /// <para>
        /// This method may be used multiple times, each time for a different type that is derived from <typeparamref name="TValue"/>.
        /// </para>
        /// </remarks>
        /// <typeparam name="TDerived">The derived (child) type to be configured for validation in this polymorphic validation configuration.</typeparam>
        /// <param name="derivedConfig">Configuration which indicates what validation will be performed upon the value if its runtime type is <typeparamref name="TDerived"/>.</param>
        /// <returns>A reference to the same builder object, enabling chaining of calls if desired.</returns>
        IConfiguresValueAccessor<TValidated, TValue> WhenValueIs<TDerived>(Action<IConfiguresValueAccessor<TValidated,TDerived>> derivedConfig) where TDerived : TValue;
    }
}