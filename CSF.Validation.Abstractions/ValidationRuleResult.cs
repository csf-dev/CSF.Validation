using System;
using System.Collections.Generic;
using CSF.Validation.RuleExecution;
using CSF.Validation.Rules;

namespace CSF.Validation
{
    /// <summary>
    /// Represents information about the result of running a single validation rule.
    /// Typically this is a class which implements either <see cref="IRule{TValidated}"/>
    /// or <see cref="IRule{TValue, TValidated}"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class is a more complete version of <see cref="RuleResult"/>.  Where the rule result only contains
    /// the information which would be directly output from the rule itself, this class provides more contextual
    /// information which may be 'backfilled' by the validation framework.
    /// </para>
    /// </remarks>
    public class ValidationRuleResult : RuleResult
    {
        /// <summary>
        /// Gets a unique identifier for the rule to which this result corresponds.
        /// </summary>
        public RuleIdentifier Identifier { get; }

        /// <summary>
        /// Gets a reference to the <see cref="Type"/> of rule interface which was used for this rule.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This will be a closed-generic form of either <see cref="IRule{TValidated}"/> or
        /// <see cref="IRule{TValue, TParent}"/>.
        /// </para>
        /// </remarks>
        public Type RuleInterface { get; }

        /// <summary>
        /// Gets the actual value which was validated by this rule.
        /// </summary>
        public object ValidatedValue { get; }

        /// <summary>
        /// Gets contextual information about this rule.
        /// </summary>
        public RuleContext RuleContext { get; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationRuleResult"/> class.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This constructor copies-information-from and then enriches a <see cref="RuleResult"/> with further information.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">If any parameter is <see langword="null"/>.</exception>
        public ValidationRuleResult(RuleResult result,
                                    RuleContext ruleContext) : base(result)
        {
            RuleContext = ruleContext ?? throw new ArgumentNullException(nameof(ruleContext));
            Identifier = ruleContext.RuleIdentifier;
            RuleInterface = ruleContext.RuleInterface;
            ValidatedValue = ruleContext.ActualValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationRuleResult"/> class.
        /// </summary>
        /// <param name="result">A validation rule result instance to copy.</param>
        /// <exception cref="ArgumentNullException">If the <paramref name="result"/> is <see langword="null"/>.</exception>
        protected ValidationRuleResult(ValidationRuleResult result) : base(result)
        {
            RuleContext = result.RuleContext;
            Identifier = result.Identifier;
            RuleInterface = result.RuleInterface;
            ValidatedValue = result.ValidatedValue;
        }
    }
}