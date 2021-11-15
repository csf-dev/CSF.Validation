using System;
using System.Reflection;
using CSF.Validation.Manifest;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A model providing contextual information about an 'ancestor context' for an executing validation rule.
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
        /// Gets the validation manifest value for the current ancestor value.
        /// </summary>
        public ManifestValue ManifestValue { get; }

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
        /// <description><see cref="CSF.Validation.ValidatorBuilding.IConfiguresValidator{TValidated}.ForMemberItems{TValue}(System.Linq.Expressions.Expression{Func{TValidated, System.Collections.Generic.IEnumerable{TValue}}}, Action{ValidatorBuilding.IConfiguresValueAccessor{TValidated, TValue}})"/></description>
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
        public long? CollectionIndex { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="AncestorRuleContext"/>.
        /// </summary>
        /// <param name="objectIdentity">The object identity associated with this ancestor context.</param>
        /// <param name="obj">The object being validated in this ancestor context.</param>
        /// <param name="manifestValue">The manifest value.</param>
        /// <param name="collectionIndex">The collection index by which you would traverse from this ancestor context to its immediate child (where applicable).</param>
        public AncestorRuleContext(object objectIdentity, object obj, ManifestValue manifestValue, long? collectionIndex = null)
        {
            ObjectIdentity = objectIdentity;
            Object = obj ?? throw new ArgumentNullException(nameof(obj));
            ManifestValue = manifestValue ?? throw new ArgumentNullException(nameof(manifestValue));
            CollectionIndex = collectionIndex;
        }
    }
}