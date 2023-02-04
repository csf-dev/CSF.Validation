using System;
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
        /// Gets a reference to the validation rule logic.
        /// </summary>
        public IValidationLogic ValidationLogic { get; }

        /// <summary>
        /// Gets a human-readable feedback message to be associated with this rule result.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property will always contain a <see langword="null" /> reference unless <see cref="ResolvedValidationOptions"/>
        /// are specified and the <see cref="ResolvedValidationOptions.EnableMessageGeneration"/> is set to <see langword="true" />.
        /// If the options are not specified or message generation is not enabled, then generation of feedback messages
        /// is disabled/skipped.
        /// </para>
        /// </remarks>
        public string Message { get; }

        /// <inheritdoc/>
        public override string ToString()
        {
            if(Outcome == RuleOutcome.Errored)
                return $"[{nameof(ValidationRuleResult)}: {nameof(RuleResult.Outcome)} = {Outcome}, {nameof(Identifier)} = {Identifier}, {nameof(Exception)} = {Exception}]";
            
            return $"[{nameof(ValidationRuleResult)}: {nameof(RuleResult.Outcome)} = {Outcome}, {nameof(Identifier)} = {Identifier}, {nameof(Message)} = {Message}]";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationRuleResult"/> class.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This constructor copies-information-from and then enriches a <see cref="RuleResult"/> with further information.
        /// </para>
        /// </remarks>
        /// <param name="result">The rule result.</param>
        /// <param name="ruleContext">The rule context.</param>
        /// <param name="validationLogic">The validation logic for the rule.</param>
        /// <param name="message">The human-readable validation feedback message.</param>
        public ValidationRuleResult(RuleResult result,
                                    RuleContext ruleContext,
                                    IValidationLogic validationLogic,
                                    string message = default) : base(result)
        {
            RuleContext = ruleContext;
            ValidationLogic = validationLogic;
            Identifier = ruleContext?.RuleIdentifier;
            RuleInterface = ruleContext?.RuleInterface;
            ValidatedValue = ruleContext?.ActualValue;
            Message = message;
        }
    }
}