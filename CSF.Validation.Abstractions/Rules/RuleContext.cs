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
        /// Initialises a new instance of <see cref="RuleContext"/>.
        /// </summary>
        /// <param name="identifier">The rule identifier.</param>
        /// <param name="ancestorContexts">An optional collection of ancestor contexts.</param>
        public RuleContext(RuleIdentifier identifier,
                           IReadOnlyList<AncestorRuleContext> ancestorContexts = null)
        {
            Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
            AncestorContexts = ancestorContexts ?? new AncestorRuleContext[0];
        }
    }
}