using System;
using System.Collections.Generic;
using System.Reflection;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A model for contextual information related to the execution of a value-oriented validation rule.
    /// </summary>
    public class ValueRuleContext : RuleContext
    {
        /// <summary>
        /// If the value being validated was retrieved from a specified member (for example a property
        /// or method) of the validated object, then this property is a reference to that member.
        /// Otherwise it will be <see langword="null"/>.
        /// </summary>
        public MemberInfo Member { get; }

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
        /// Initialises a new instance of <see cref="ValueRuleContext"/>.
        /// </summary>
        /// <param name="identifier">The rule identifier.</param>
        /// <param name="validatedType">The validated type.</param>
        /// <param name="ancestorContexts">An optional collection of ancestor contexts.</param>
        /// <param name="member">An optional value-providing member.</param>
        /// <param name="collectionIndex">An optional collection index.</param>
        public ValueRuleContext(RuleIdentifier identifier,
                                Type validatedType,
                                IReadOnlyList<AncestorRuleContext> ancestorContexts = null,
                                MemberInfo member = null,
                                int? collectionIndex = null) : base(identifier, validatedType, ancestorContexts)
        {
            Member = member;
            CollectionIndex = collectionIndex;
        }

    }
}