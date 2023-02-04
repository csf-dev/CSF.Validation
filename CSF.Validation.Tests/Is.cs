using NUnit.Framework.Constraints;
using CSF.Validation.Rules;

namespace CSF.Validation
{
    public class Is : NUnit.Framework.Is
    {
        /// <summary>
        /// Gets an NUnit constraint which asserts that the result is a passing result for a single validation rule.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This constraint actually accepts any of:
        /// </para>
        /// <list type="bullet">
        /// <item><description><see cref="CSF.Validation.Rules.RuleResult"/></description></item>
        /// <item><description><see cref="CSF.Validation.Rules.RuleOutcome"/></description></item>
        /// <item><description>A <see cref="System.Threading.Tasks.Task{T}"/> of <see cref="CSF.Validation.Rules.RuleResult"/></description></item>
        /// <item><description>A <see cref="System.Threading.Tasks.Task{T}"/> of <see cref="CSF.Validation.Rules.RuleOutcome"/></description></item>
        /// </list>
        /// <para>
        /// This means that it reduces the complexity of usage, because test logic does not need to deal with
        /// the async nature of validation rules.
        /// </para>
        /// </remarks>
        public static IConstraint PassingRuleResult => new RuleResultConstraint(RuleOutcome.Passed);

        /// <summary>
        /// Gets an NUnit constraint which asserts that the result is a failing result for a single validation rule.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This constraint actually accepts any of:
        /// </para>
        /// <list type="bullet">
        /// <item><description><see cref="CSF.Validation.Rules.RuleResult"/></description></item>
        /// <item><description><see cref="CSF.Validation.Rules.RuleOutcome"/></description></item>
        /// <item><description>A <see cref="System.Threading.Tasks.Task{T}"/> of <see cref="CSF.Validation.Rules.RuleResult"/></description></item>
        /// <item><description>A <see cref="System.Threading.Tasks.Task{T}"/> of <see cref="CSF.Validation.Rules.RuleOutcome"/></description></item>
        /// </list>
        /// <para>
        /// This means that it reduces the complexity of usage, because test logic does not need to deal with
        /// the async nature of validation rules.
        /// </para>
        /// </remarks>
        public static IConstraint FailingRuleResult => new RuleResultConstraint(RuleOutcome.Failed);

        /// <summary>
        /// Gets an NUnit constraint which asserts that the result is a passing result for a validation attempt.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This constraint actually accepts any of:
        /// </para>
        /// <list type="bullet">
        /// <item><description><see cref="CSF.Validation.ValidationResult"/></description></item>
        /// <item><description>A <see cref="System.Threading.Tasks.Task{T}"/> of <see cref="CSF.Validation.ValidationResult"/></description></item>
        /// </list>
        /// <para>
        /// This means that it reduces the complexity of usage, because test logic does not need to deal with
        /// the async nature of validation.
        /// </para>
        /// </remarks>
        public static IConstraint PassingValidationResult => new ValidationResultConstraint(true);

        /// <summary>
        /// Gets an NUnit constraint which asserts that the result is a failing validation result.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This constraint actually accepts any of:
        /// </para>
        /// <list type="bullet">
        /// <item><description><see cref="CSF.Validation.ValidationResult"/></description></item>
        /// <item><description>A <see cref="System.Threading.Tasks.Task{T}"/> of <see cref="CSF.Validation.ValidationResult"/></description></item>
        /// </list>
        /// <para>
        /// This means that it reduces the complexity of usage, because test logic does not need to deal with
        /// the async nature of validation.
        /// </para>
        /// </remarks>
        public static IConstraint FailingValidationResult => new ValidationResultConstraint(false);

        /// <summary>
        /// Gets an NUnit constraint which asserts that the result is an error validation result.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This constraint actually accepts any of:
        /// </para>
        /// <list type="bullet">
        /// <item><description><see cref="CSF.Validation.Rules.RuleResult"/></description></item>
        /// <item><description><see cref="CSF.Validation.Rules.RuleOutcome"/></description></item>
        /// <item><description>A <see cref="System.Threading.Tasks.Task{T}"/> of <see cref="CSF.Validation.Rules.RuleResult"/></description></item>
        /// <item><description>A <see cref="System.Threading.Tasks.Task{T}"/> of <see cref="CSF.Validation.Rules.RuleOutcome"/></description></item>
        /// </list>
        /// <para>
        /// This means that it reduces the complexity of usage, because test logic does not need to deal with
        /// the async nature of validation rules.
        /// </para>
        /// </remarks>
        public static IConstraint ErrorRuleResult => new RuleResultConstraint(RuleOutcome.Errored);
    }
}