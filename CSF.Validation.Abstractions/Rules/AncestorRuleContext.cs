using System;
using System.Reflection;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// Represents contextual information about an 'ancestor context' for an executing validation rule.
    /// </summary>
    public class AncestorRuleContext
    {
        /// <summary>
        /// Gets the identity of the object which is associated with the current ancestor context.
        /// </summary>
        public object ObjectIdentity { get; }

        /// <summary>
        /// Gets a reference to the validated object which is associated with the current ancestor context.
        /// </summary>
        public object Object { get; }

        /// <summary>
        /// Gets the type of object is validated by this ancestor context, as selected by the validator itself.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This might not be the same as the actual runtime type of <see cref="Object"/>.
        /// However, that validated object is certain to be a type which is either the same as
        /// this Validated Type property or a type derived from this Validated Type property.
        /// </para>
        /// </remarks>
        /// <example>
        /// <para>
        /// For example, if this ValidatedType property is <c>typeof(Animal)</c> then the actual validated object
        /// might be an instance of <c>Animal</c> or it might be an instance of a more derived type such as
        /// <c>Dog</c> or <c>Cat</c>.
        /// </para>
        /// </example>
        public Type ValidatedType { get; }

        /// <summary>
        /// Where the immediate child of this ancestor context may be accessed via a specific member of the <see cref="Object"/>
        /// then this property contains a reference to that member.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property gets the member which would be used from the <see cref="Object"/> in order to reach its immediate
        /// child context.  That might be another ancestor context (if the current ancestor represents a grandparent or more
        /// distant ancestor) or it might be the current rule context (if the current ancestor is the direct parent).
        /// </para>
        /// <para>
        /// This property will be present with a value when the child context was created via either:
        /// </para>
        /// <list type="bullet">
        /// <item>
        /// <description><see cref="CSF.Validation.ValidatorBuilding.IConfiguresValidator{TValidated}.ForMember{TValue}(System.Linq.Expressions.Expression{Func{TValidated, TValue}}, Action{ValidatorBuilding.IConfiguresValueAccessor{TValidated, TValue}})"/></description>
        /// </item>
        /// <item>
        /// <description><see cref="CSF.Validation.ValidatorBuilding.IConfiguresValidator{TValidated}.ForItemsMember{TValue}(System.Linq.Expressions.Expression{Func{TValidated, System.Collections.Generic.IEnumerable{TValue}}}, Action{ValidatorBuilding.IConfiguresValueAccessor{TValidated, TValue}})"/></description>
        /// </item>
        /// </list>
        /// <para>
        /// If the child context was created simply by adding rules directly, or for an arbitrary value derived from the validated
        /// object then this property will be <see langword="null"/>.
        /// </para>
        /// </remarks>
        public MemberInfo ChildMember { get; }

        /// <summary>
        /// Where the immediate child of this ancestor context is created from a collection item, this property provides
        /// the zero-based collection index of that item.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property gets the collection index of the member or enumerable value derived from the <see cref="Object"/>
        /// from which its immediate child context is created.  That might be another ancestor context (if the current
        /// ancestor represents a grandparent or more distant ancestor) or it might be the current rule context (if the
        /// current ancestor is the direct parent).
        /// </para>
        /// <para>
        /// This property will be present with a value when the child context was created via either:
        /// </para>
        /// <list type="bullet">
        /// <item>
        /// <description><see cref="CSF.Validation.ValidatorBuilding.IConfiguresValidator{TValidated}.ForItemsMember{TValue}(System.Linq.Expressions.Expression{Func{TValidated, System.Collections.Generic.IEnumerable{TValue}}}, Action{ValidatorBuilding.IConfiguresValueAccessor{TValidated, TValue}})"/></description>
        /// </item>
        /// <item>
        /// <description><see cref="CSF.Validation.ValidatorBuilding.IConfiguresValidator{TValidated}.ForValues{TValue}(Func{TValidated, System.Collections.Generic.IEnumerable{TValue}}, Action{ValidatorBuilding.IConfiguresValueAccessor{TValidated, TValue}})"/></description>
        /// </item>
        /// </list>
        /// <para>
        /// If the child context was not created from a collection then then this property will be <see langword="null"/>.
        /// Please note that the index will represent the order in which the item came from the
        /// <see cref="System.Collections.Generic.IEnumerable{T}"/>.  In most implementations of enumerable, the index will
        /// be stable, but note that the interface does not guarantee a stable order.  In other words, enumerating the same
        /// collection twice is not certain to provide the results in the same order each time.
        /// </para>
        /// </remarks>
        public int? CollectionIndex { get; }

        public AncestorRuleContext(object objectIdentity, object obj, Type validatedType, MemberInfo childMember = null, int? collectionIndex = null)
        {
            ObjectIdentity = objectIdentity ?? throw new ArgumentNullException(nameof(objectIdentity));
            Object = obj ?? throw new ArgumentNullException(nameof(obj));
            ValidatedType = validatedType ?? throw new ArgumentNullException(nameof(validatedType));
            ChildMember = childMember;
            CollectionIndex = collectionIndex;
        }
    }
}