using System;
using System.Collections.Generic;
using CSF.Validation.Manifest;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A read-only model for contextual information related to the execution of a validation rule.
    /// </summary>
    public class RuleContext : ValueContext
    {
        /// <summary>
        /// Gets a <see cref="ManifestRuleInfo"/> with information about the configuration of the current rule.
        /// </summary>
        public ManifestRuleInfo RuleInfo { get; }

        /// <summary>
        /// Gets the identifier of the executed rule.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This object provides identifying information about the currently-executing validation
        /// rule.
        /// </para>
        /// </remarks>
        public RuleIdentifier RuleIdentifier { get; }

        /// <summary>
        /// Gets a reference to the rule interface which was used in order to select &amp; execute this rule.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This will be a closed-generic form of either:
        /// </para>
        /// <list type="bullet">
        /// <item><description><see cref="IRule{TValidated}"/></description></item>
        /// <item><description><see cref="IRule{TValue, TParent}"/></description></item>
        /// </list>
        /// </remarks>
        public Type RuleInterface { get; }

        /// <summary>
        /// Gets a collection of the ancestor validation contexts.
        /// </summary>
        /// <remarks>
        /// <para>
        /// When either of the following methods is used (using a validator builder), a 'child' validation context is created
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
        /// Equivalents to the above exist for the Manifest Model: <see cref="CSF.Validation.ManifestModel.ValueBase.Children"/>
        /// and also for a Validation Manifest: <see cref="CSF.Validation.Manifest.ManifestValueBase.Children"/>.
        /// </para>
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
        /// <para>
        /// When using the <see cref="IRule{TValue, TParent}"/> interface, the first <see cref="ValueContext.ActualValue"/> is
        /// made available as the second parameter to <see cref="IRule{TValue, TParent}.GetResultAsync(TValue, TParent, RuleContext, System.Threading.CancellationToken)"/>.
        /// </para>
        /// </remarks>
        public IReadOnlyList<ValueContext> AncestorContexts { get; }

        /// <summary>
        /// Initialises a new instance of <see cref="RuleContext"/>.
        /// </summary>
        /// <param name="manifestRule">The manifest rule from which this context was created.</param>
        /// <param name="ruleIdentifier">The rule identifier.</param>
        /// <param name="actualValue">The actual validated value.</param>
        /// <param name="ancestorContexts">A collection of ancestor contexts.</param>
        /// <param name="ruleInterface">The rule interface used for this rule execution.</param>
        /// <param name="collectionItemOrder">An optional collection item order.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public RuleContext(ManifestRule manifestRule,
                           RuleIdentifier ruleIdentifier,
                           object actualValue,
                           IEnumerable<ValueContext> ancestorContexts,
                           Type ruleInterface,
                           long? collectionItemOrder = null)
            : base(ruleIdentifier?.ObjectIdentity, actualValue, manifestRule?.ManifestValue, collectionItemOrder)
        {
            if (manifestRule is null)
                throw new ArgumentNullException(nameof(manifestRule));
            if (ancestorContexts is null)
                throw new ArgumentNullException(nameof(ancestorContexts));

            RuleInfo = new ManifestRuleInfo(manifestRule);
            RuleIdentifier = ruleIdentifier ?? throw new ArgumentNullException(nameof(ruleIdentifier));
            RuleInterface = ruleInterface ?? throw new ArgumentNullException(nameof(ruleInterface));
            AncestorContexts = new List<ValueContext>(ancestorContexts);
        }
    }
}