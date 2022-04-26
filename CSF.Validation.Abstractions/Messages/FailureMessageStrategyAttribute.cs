using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CSF.Validation.Rules;

namespace CSF.Validation.Messages
{
    /// <summary>
    /// Decorates a failure message strategy class to indicate where that strategy class may be used.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Using <c>[FailureMessageStrategy]</c> upon a class that provides validation failure messages is a
    /// high-performance way to indicate to the framework when that class should be used to provide the failure
    /// message for any given validation result.
    /// </para>
    /// <para>
    /// A failure message class is one which implements one of the following interfaces:
    /// </para>
    /// <list type="bullet">
    /// <item><description><see cref="IGetsFailureMessage"/></description></item>
    /// <item><description><see cref="IGetsFailureMessage{TValidated}"/></description></item>
    /// <item><description><see cref="IGetsFailureMessage{TValidated, TParent}"/></description></item>
    /// </list>
    /// <para>
    /// When used, every one of the predicate values which is set to a non-<see langword="null" /> value
    /// must be satisfied by the failure (or error) result - a logical AND of all of the values for the result
    /// to match.
    /// This attribute may be used more then once however, and each separate usage of the attribute upon the same
    /// class represents an alternative scenario - a logical OR between each of the times that this attribute is used
    /// upon the same class.
    /// </para>
    /// <para>
    /// Every one of the properties of this attribute represents a predicate value.
    /// </para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class FailureMessageStrategyAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets a <see cref="System.Type"/> which the <see cref="RuleIdentifierBase.RuleType"/> of the
        /// rule identifier at <see cref="ValidationRuleResult.Identifier"/> must match, if this property is non-<see langword="null" />.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If this is a non-generic type or a closed generic type then the rule type must match this type precisely.
        /// If this is an open generic type then the rule type must be a closed version of that same open generic type,
        /// although the precise generic type parameters of that closed version are irrelevant.
        /// </para>
        /// </remarks>
        public Type RuleType { get; set; }
        
        /// <summary>
        /// Gets or sets a string which the <see cref="RuleIdentifier.MemberName"/> of the
        /// rule identifier at <see cref="ValidationRuleResult.Identifier"/> must match, if this property is non-<see langword="null" />.
        /// </summary>
        public string MemberName { get; set; }
        
        /// <summary>
        /// Gets or sets a string which the <see cref="RuleIdentifierBase.RuleName"/> of the
        /// rule identifier at <see cref="ValidationRuleResult.Identifier"/> must match, if this property is non-<see langword="null" />.
        /// </summary>
        public string RuleName { get; set; }
        
        /// <summary>
        /// Gets or sets a <see cref="System.Type"/> which the <see cref="RuleIdentifierBase.ValidatedType"/> of the
        /// rule identifier at <see cref="ValidationRuleResult.Identifier"/> must match, if this property is non-<see langword="null" />.
        /// </summary>
        public Type ValidatedType { get; set; }
        
        /// <summary>
        /// Gets or sets a <see cref="System.Type"/> which the validated type of the parent validated object must
        /// match, if this property is non-<see langword="null" />.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The parent validated object is typically a model object from which the current validated value was accessed/derived.
        /// Most commonly this is a property access, so the parent validated object will be the object with the property from which
        /// the currently-validated value is accessed.
        /// </para>
        /// <para>
        /// More technically, the parent validated object type is found from the <see cref="ValidationRuleResult"/> by accessing
        /// the <see cref="ValidationRuleResult.RuleContext"/>, traversing to the first item within <see cref="RuleContext.AncestorContexts"/>
        /// (which must exist), to <see cref="ValueContext.ValueInfo"/> and then reading <see cref="ManifestValueInfo.ValidatedType"/>.
        /// Developers need not do this themselves though, as the matching process handles this automatically.
        /// </para>
        /// </remarks>
        public Type ParentValidatedType { get; set; }
        
        /// <summary>
        /// Gets or sets a <see cref="System.Type"/> which the interface used by the current rule must match, if this property
        /// is non-<see langword="null" />.
        /// </summary>
        public Type RuleInterface { get; set; }
        
        /// <summary>
        /// Gets or sets a <see cref="RuleOutcome"/> which the outcome of the current rule must match, if this property
        /// is non-<see langword="null" />.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Whilst validation messages are only generated by failure results by default, it is possible to use this property to
        /// cause validation messages to be generated other outcomes such as <see cref="RuleOutcome.Errored"/>
        /// or <see cref="RuleOutcome.DependencyFailed"/>.
        /// </para>
        /// <para>
        /// If this property is set to either <see langword="null" /> or <see cref="RuleOutcome.Failed"/> then the decorated class will
        /// be used to generate messages only for <see cref="RuleOutcome.Failed"/> outcomes; this is the default behaviour.
        /// If this property is set to any value other than failed then the class may be used to generate messages for that other
        /// outcome.
        /// </para>
        /// </remarks>
        public RuleOutcome? Outcome { get; set; }

        /// <summary>
        /// Gets a numeric priority indicating the preference that this attribute will take over other attributes, should
        /// multiple attributes match.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If more than one instance of <see cref="FailureMessageStrategyAttribute"/> matches a <see cref="ValidationRuleResult"/>,
        /// for example the attributes across a number of message-providing classes match, then this priority will be used to tie-break
        /// and decide which class to actually use.
        /// </para>
        /// <para>
        /// The rules for calculating priority are simple.  Each of the properties upon the attribute that is set to a non-null value
        /// increases the priority by one.  The attribute with the highest priority 'wins the tie' if there are multiple matches.
        /// So, higher priority will be awarded to attributes which have more specific matching criteria (more predicate values present).
        /// </para>
        /// </remarks>
        /// <returns>A numeric priority value.</returns>
        public int GetPriority()
        {
            int priority = 0;
            priority += RuleType is null ? 0 : 1;
            priority += MemberName is null ? 0 : 1;
            priority += RuleName is null ? 0 : 1;
            priority += ValidatedType is null ? 0 : 1;
            priority += ParentValidatedType is null ? 0 : 1;
            priority += RuleInterface is null ? 0 : 1;
            priority += Outcome is null ? 0 : 1;
            return priority;
        }

        /// <summary>
        /// Gets a value that indicates whether this attribute is a match for the specified validation rule result.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is used to determine whether a class decorated with this attribute is eligible to be used to
        /// provide a message for the specified result.
        /// </para>
        /// </remarks>
        /// <param name="result">A validation rule result.</param>
        /// <returns><see langword="true" /> if this attribute matches the specified result; <see langword="false" /> otherwise.</returns>
        /// <exception cref="ArgumentNullException">If the <paramref name="result"/> is <see langword="null" />.</exception>
        public bool IsMatch(ValidationRuleResult result)
        {
            if (result is null)
                throw new ArgumentNullException(nameof(result));

            return ValidationFunctions.All(func => func(result));
        }

        IEnumerable<Func<ValidationRuleResult, bool>> ValidationFunctions => new Func<ValidationRuleResult, bool>[] {
            DoesMemberNameMatch,
            DoesRuleNameMatch,
            DoesValidatedTypeMatch,
            DoesParentValidatedTypeMatch,
            DoesRuleInterfaceMatch,
            DoesOutcomeMatch,
        };

        bool DoesMemberNameMatch(ValidationRuleResult result)
            => MemberName is null || StringEquals(MemberName, result.Identifier.MemberName);

        bool DoesRuleNameMatch(ValidationRuleResult result)
            => RuleName is null || StringEquals(MemberName, result.Identifier.RuleName);

        bool DoesValidatedTypeMatch(ValidationRuleResult result)
            => ValidatedType is null || ValidatedType.GetTypeInfo().IsAssignableFrom(result.Identifier.ValidatedType.GetTypeInfo());

        bool DoesParentValidatedTypeMatch(ValidationRuleResult result)
        {
            if(ParentValidatedType is null) return true;
            var parentContext = result.RuleContext.AncestorContexts.FirstOrDefault();
            return !(parentContext is null) && ParentValidatedType.GetTypeInfo().IsAssignableFrom(parentContext.ValueInfo.ValidatedType.GetTypeInfo());
        }

        bool DoesRuleInterfaceMatch(ValidationRuleResult result)
            => RuleInterface is null || RuleInterface == result.RuleInterface;

        bool DoesOutcomeMatch(ValidationRuleResult result)
            => !Outcome.HasValue || Outcome.Value == result.Outcome;

        static bool StringEquals(string first, string second)
        {
#if NETSTANDARD1_1
            return String.Equals(first, second);
#else
            return String.Equals(first, second, StringComparison.InvariantCulture);
#endif
        }
    }
}