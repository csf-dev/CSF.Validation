using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CSF.Validation.Rules;

namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// A builder service which may be used to configure a validator as it is being built.
    /// </summary>
    /// <typeparam name="TValidated">The validated object type.</typeparam>
    public interface IConfiguresValidator<TValidated>
    {
        /// <summary>
        /// Specifies an accessor function which should be used to get the identity of the validated object.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Object identities are useful &amp; important when validating complex object models which include
        /// object graphs (tree-structures) of related/connected objects.  This is particularly true where
        /// collections are child objects are involved.
        /// The identity is used to differentiate object instances.  For example, so that the validation rule
        /// results may be matched back to the object to which they relate.
        /// </para>
        /// </remarks>
        /// <example>
        /// <para>
        /// For example, if validating an instance of the following object:
        /// </para>
        /// <code>
        /// using System;
        /// using System.Collections.Generic;
        /// 
        /// public class HumanBeing
        /// {
        ///     public Guid Identity { get; set; } = new Guid();
        ///     public string Name { get; set; }
        ///     public List&lt;HumanBeing&gt; Children { get; } = new List&lt;HumanBeing&gt;();
        /// }
        /// </code>
        /// <para>
        /// Without any kind of identity, if we had a validation rule that validated the <c>Name</c> property is not null
        /// or whitespace-only, then what if more than one child failed this same rule?  In our feedback (for example some
        /// user interface), how would we indicate which children failed the validation and which children did not.
        /// </para>
        /// <para>
        /// By using (for example) the <c>Identity</c> property to uniquely identity each instance of <c>HumanBeing</c>
        /// we will recieve the related identities back with the validation rule results, allowing us to associate the
        /// results back to the appropriate object within the object graph.
        /// </para>
        /// </example>
        /// <param name="identityAccessor">The accessor function.</param>
        /// <returns>A reference to the same builder object, enabling chaining of calls if desired.</returns>
        IConfiguresValidator<TValidated> UseObjectIdentity(Func<TValidated, object> identityAccessor);

        /// <summary>
        /// Adds a validation rule to validate the validated object instance.
        /// The rule type must be a class that implements <see cref="IRule{TValidated}"/> for the same
        /// (or a compatible contravariant) generic type <typeparamref name="TValidated"/>.
        /// </summary>
        /// <typeparam name="TRule">The concrete type of the validation rule.</typeparam>
        /// <param name="ruleDefinition">An optional action which defines &amp; configures the validation rule.</param>
        /// <returns>A reference to the same builder object, enabling chaining of calls if desired.</returns>
        IConfiguresValidator<TValidated> AddRule<TRule>(Action<IConfiguresRule<TRule>> ruleDefinition = default) where TRule : IRule<TValidated>;

        /// <summary>
        /// Adds/imports rules from an object that implements <see cref="IBuildsValidator{TValidated}"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This allows composition of validators and reuse of validation rules across differing validation scenarios.
        /// All of the rules specified in the selected builder-type will be imported and added to the current validator.
        /// </para>
        /// </remarks>
        /// <typeparam name="TBuilder">
        /// The type of a class implementing <see cref="IBuildsValidator{TValidated}"/>, specifying how a validator should be built.
        /// </typeparam>
        /// <returns>A reference to the same builder object, enabling chaining of calls if desired.</returns>
        IConfiguresValidator<TValidated> AddRules<TBuilder>() where TBuilder : IBuildsValidator<TValidated>;

        /// <summary>
        /// Adds/imports rules from an object that implements <see cref="IBuildsValidator{TBase}"/>.  These rules should be for validation
        /// of a type from which <typeparamref name="TValidated"/> derives.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This allows composition of validators and use of rules declared to validate a base type, reusing validation rules across differing
        /// validation scenarios. All of the rules specified in the selected builder-type will be imported and added to the current validator.
        /// </para>
        /// </remarks>
        /// <typeparam name="TBuilder">
        /// The type of a class implementing <see cref="IBuildsValidator{TBase}"/>, specifying how a validator for the base type should be built.
        /// </typeparam>
        /// <typeparam name="TBase">
        /// The base type, for which validation rules are to be imported.  The type <typeparamref name="TValidated"/> must derive from
        /// the type specified in this generic type parameter.
        /// </typeparam>
        /// <returns>A reference to the same builder object, enabling chaining of calls if desired.</returns>
        /// <exception cref="InvalidCastException">
        /// If the type specified as <typeparamref name="TBase"/> is not an ancestor type to <typeparamref name="TValidated"/>.
        /// </exception>
        IConfiguresValidator<TValidated> AddBaseRules<TBase, TBuilder>() where TBuilder : IBuildsValidator<TBase>;

        /// <summary>
        /// Allows addition of validation which will work upon the value of a specific member of the validated object.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Most commonly the member is a property, but it may also be a field or parameterless method (which has a non-void
        /// return value).
        /// </para>
        /// <para>
        /// When it is possible to describe a validated value by a simple member access then this mechanism is preferred over
        /// <see cref="ForValue{TValue}(Func{TValidated, TValue}, Action{IConfiguresValueAccessor{TValidated, TValue}})"/> because
        /// the member name will form a part of the validation rule's identifier.
        /// </para>
        /// </remarks>
        /// <typeparam name="TValue">The type (property/field/method-return-type) of the member which is being validated.</typeparam>
        /// <param name="memberAccessor">An expression which describes the accessor for the member.</param>
        /// <param name="valueConfig">Configuration which indicates what validation will be performed upon the member's value.</param>
        /// <exception cref="ArgumentNullException">If either <paramref name="memberAccessor"/> or <paramref name="valueConfig"/> are <see langword="null"/>.</exception>
        /// <returns>A reference to the same builder object, enabling chaining of calls if desired.</returns>
        IConfiguresValidator<TValidated> ForMember<TValue>(Expression<Func<TValidated, TValue>> memberAccessor, Action<IConfiguresValueAccessor<TValidated, TValue>> valueConfig);

        /// <summary>
        /// Allows addition of validation which will work upon each of the items exposed by a specific member of the
        /// validated object, where that member's type implements <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Most commonly the member is a property, but it may also be a field or parameterless method (which has a non-void
        /// return value).
        /// </para>
        /// <para>
        /// When it is possible to describe the collection which should have its items validated by using a simple member access
        /// then this mechanism is preferred over
        /// <see cref="ForValues{TValue}(Func{TValidated, IEnumerable{TValue}}, Action{IConfiguresValueAccessor{TValidated, TValue}})"/>
        /// because the member name will form a part of the validation rule's identifier.
        /// </para>
        /// </remarks>
        /// <typeparam name="TValue">
        /// The type of the items within the collection which shall be validated, where the type (property/field/method-return-type)
        /// of the member implements <see cref="IEnumerable{T}"/> of this generic type.
        /// </typeparam>
        /// <param name="memberAccessor">An expression which describes the accessor for the collection member.</param>
        /// <param name="valueConfig">Configuration which indicates what validation will be performed upon the collection items.</param>
        /// <exception cref="ArgumentNullException">If either <paramref name="memberAccessor"/> or <paramref name="valueConfig"/> are <see langword="null"/>.</exception>
        /// <returns>A reference to the same builder object, enabling chaining of calls if desired.</returns>
        IConfiguresValidator<TValidated> ForMemberItems<TValue>(Expression<Func<TValidated, IEnumerable<TValue>>> memberAccessor, Action<IConfiguresValueAccessor<TValidated, TValue>> valueConfig);

        /// <summary>
        /// Allows addition of validation which will work upon an arbitrary value derived from the validated object.
        /// </summary>
        /// <remarks>
        /// <para>
        /// When it is possible to describe/acces the derived value by a simple member access then please prefer
        /// <see cref="ForMember{TValue}(Expression{Func{TValidated, TValue}}, Action{IConfiguresValueAccessor{TValidated, TValue}})"/>
        /// over this method.
        /// </para>
        /// </remarks>
        /// <typeparam name="TValue">The type of the value which is to be validated.</typeparam>
        /// <param name="valueAccessor">A function which gets the value to be validated.</param>
        /// <param name="valueConfig">Configuration which indicates what validation will be performed upon the value.</param>
        /// <exception cref="ArgumentNullException">If either <paramref name="valueAccessor"/> or <paramref name="valueConfig"/> are <see langword="null"/>.</exception>
        /// <returns>A reference to the same builder object, enabling chaining of calls if desired.</returns>
        IConfiguresValidator<TValidated> ForValue<TValue>(Func<TValidated, TValue> valueAccessor, Action<IConfiguresValueAccessor<TValidated, TValue>> valueConfig);

        /// <summary>
        /// Allows addition of validation which will work upon each of the items exposed by an arbitrary collection, derived from the
        /// validated object, which implements <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// When it is possible to describe/access the collection by a simple member access then please prefer
        /// <see cref="ForMemberItems{TValue}(Expression{Func{TValidated, IEnumerable{TValue}}}, Action{IConfiguresValueAccessor{TValidated, TValue}})"/>
        /// over this method.
        /// </para>
        /// </remarks>
        /// <typeparam name="TValue">
        /// The type of the items within the collection which shall be validated, where the collection itself
        /// implements <see cref="IEnumerable{T}"/> of this generic type.
        /// </typeparam>
        /// <param name="valuesAccessor">A function which gets the collection, for which its items should be validated.</param>
        /// <param name="valueConfig">Configuration which indicates what validation will be performed upon the collection items.</param>
        /// <exception cref="ArgumentNullException">If either <paramref name="valuesAccessor"/> or <paramref name="valueConfig"/> are <see langword="null"/>.</exception>
        /// <returns>A reference to the same builder object, enabling chaining of calls if desired.</returns>
        IConfiguresValidator<TValidated> ForValues<TValue>(Func<TValidated, IEnumerable<TValue>> valuesAccessor, Action<IConfiguresValueAccessor<TValidated, TValue>> valueConfig);

        /// <summary>
        /// Configures the current value to be validated in the same manner as an ancestor value which already has validation configuration.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If this method is used then the current value will be configured for recursive (or re-entrant) validation.  It will be converted as a
        /// <see cref="CSF.Validation.Manifest.RecursiveManifestValue"/> rather than a normal value.
        /// </para>
        /// <para>
        /// The numeric value of <paramref name="depth"/> must be a positive integer &amp; indicates the level of ancestor
        /// value is used to provide the recursive validation.  For example a value of 1 indicates that the immediate
        /// parent value should be used as the wrapped value for the recursive manifest value.  A value of 2 would indicate the
        /// grandparent value.
        /// </para>
        /// <para>
        /// When recursive validation is configured, no other methods upon this builder may be called.  Doing so will raise an exception.
        /// The validation of the current value will be treated in the same way as the indicated ancestor value.
        /// </para>
        /// </remarks>
        /// <param name="depth">The ancestor number to use when validating the current value.</param>
        /// <returns>A reference to the same builder object, enabling chaining of calls if desired.</returns>
        IConfiguresValidator<TValidated> ValidateAsAncestor(int depth);

        /// <summary>
        /// Adds validation configuration for polymorphic validatation when the runtime type
        /// of the validated value is <typeparamref name="TDerived"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Where the current validator configuration works to validate an object of type <typeparamref name="TValidated"/>,
        /// the value being validated might be a derived type of <typeparamref name="TValidated"/> at runtime.
        /// Polymorphic validation allows a developer to specify how a derived type should be validated, if that is the runtime
        /// type of the value to be validated.
        /// </para>
        /// <para>
        /// This method may be used multiple times, each time for a different type that is derived from <typeparamref name="TValidated"/>.
        /// </para>
        /// </remarks>
        /// <typeparam name="TDerived">The derived (child) type to be configured for validation in this polymorphic validation configuration.</typeparam>
        /// <param name="derivedConfig">Configuration which indicates what validation will be performed upon the value if its runtime type is <typeparamref name="TDerived"/>.</param>
        /// <returns>A reference to the same builder object, enabling chaining of calls if desired.</returns>
        IConfiguresValidator<TValidated> WhenValueIs<TDerived>(Action<IConfiguresValidator<TDerived>> derivedConfig) where TDerived : TValidated;
    }
}