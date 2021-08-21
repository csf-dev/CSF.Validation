using System;
using System.Collections.Generic;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A model for contextual information related to the execution of a validation rule.
    /// </summary>
    public class RuleContext
    {
        /// <summary>
        /// Gets the identifier of the executed rule.
        /// </summary>
        public RuleIdentifier Identifier { get; }

        /// <summary>
        /// Gets the identity of the object associated with the current context.
        /// </summary>
        public object ObjectIdentity => Identifier.ObjectIdentity;

        /// <summary>
        /// Gets the type of object which the rule is validating, as selected by the validator itself.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This might not be the same as the actual runtime type of the first parameter of
        /// <see cref="IRule{TValidated}.GetResultAsync(TValidated, RuleContext, System.Threading.CancellationToken)"/>.
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
        /// Gets a collection of the ancestor validation contexts.
        /// </summary>
        /// <remarks>
        /// <para>
        /// When either of the following methods is used (when building a validator), a 'child' validation context is created
        /// and the rules which are added are placed into that new child content.
        /// </para>
        /// <list type="bullet">
        /// <item>
        /// <description><see cref="CSF.Validation.ValidatorBuilding.IConfiguresValidator{TValidated}.AddRules{TBuilder}"/></description>
        /// </item>
        /// <item>
        /// <description><see cref="CSF.Validation.ValidatorBuilding.IConfiguresValueAccessor{TValidated,TValue}.AddRules{TBuilder}"/></description>
        /// </item>
        /// </list>
        /// <para>
        /// If the currently-executing validation rule lies within a child context, this property provides access to all
        /// of the ancestor contexts, such as parents &amp; grandparents.
        /// This collection may be treated a little like a 'stack'.  The first item in this collection is the immediate parent
        /// context, the second item is the grandparent, the third the great-grandparent and so on.
        /// As many parent contexts are available from this collection as required in order to traverse back to the root
        /// validation context.  The root validation context is the validator that was initially built and not imported into
        /// an existing validator via an <c>AddRules</c> method.
        /// </para>
        /// <para>
        /// If the currently-executing validation rule is not part of a child context then this collection will be empty
        /// indicating that there are no ancestor contexts.
        /// </para>
        /// </remarks>
        public IReadOnlyList<AncestorRuleContext> AncestorContexts { get; }

        /// <summary>
        /// If the value being validated was retrieved from a specified member (for example a property
        /// or method) of the validated object, then this property gets the name of that member.
        /// Otherwise it will be <see langword="null"/>.
        /// </summary>
        public string MemberName { get; }

        /// <summary>
        /// If the value being validated was retrieved from a collection of items (from the validated object)
        /// then this property will be the zero-based collection index of the current item.  Otherwise, this
        /// property will be <see langword="null"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Please note that the index will represent the order in which the item came from the
        /// <see cref="System.Collections.Generic.IEnumerable{T}"/>.  In most implementations of enumerable, the index will
        /// be stable, but note that the interface does not guarantee a stable order.  In other words, enumerating the same
        /// collection twice is not certain to provide the results in the same order each time.
        /// </para>
        /// </remarks>
        public int? CollectionIndex { get; }

        /// <summary>
        /// Initialises a new instance of <see cref="RuleContext"/>.
        /// </summary>
        /// <param name="identifier">The rule identifier.</param>
        /// <param name="validatedType">The validated type.</param>
        /// <param name="ancestorContexts">An optional collection of ancestor contexts.</param>
        /// <param name="memberName">An optional name of the member which provides the current value being validated.</param>
        /// <param name="collectionIndex">An optional collection index.</param>
        public RuleContext(RuleIdentifier identifier,
                           Type validatedType,
                           IReadOnlyList<AncestorRuleContext> ancestorContexts = null,
                           string memberName = null,
                           int? collectionIndex = null)
        {
            Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
            ValidatedType = validatedType ?? throw new ArgumentNullException(nameof(validatedType));
            AncestorContexts = ancestorContexts ?? new AncestorRuleContext[0];
            MemberName = memberName;
            CollectionIndex = collectionIndex;
        }
    }
}