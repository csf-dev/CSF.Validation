using System.Threading.Tasks;
using NUnit.Framework.Constraints;

namespace CSF.Validation.Rules
{
    public class ValidationResultConstraint : NUnit.Framework.Constraints.Constraint
    {
        readonly RuleOutcome expectedOutcome;

        public override ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            if (actual is RuleResult result)
                return GetConstraintResult(result.Outcome, result);
            if (actual is RuleOutcome outcome)
                return GetConstraintResult(outcome, outcome);
            if (actual is Task<RuleResult> resultTask)
            {
                var theResult = resultTask.Result;
                return GetConstraintResult(theResult.Outcome, theResult);
            }
            if (actual is Task<RuleOutcome> outcomeTask)
            {
                var theOutcome = outcomeTask.Result;
                return GetConstraintResult(theOutcome, theOutcome);
            }

            return new ConstraintResult(this, actual, ConstraintStatus.Error);
        }

        ConstraintResult GetConstraintResult(RuleOutcome outcome, object actual)
            => new ConstraintResult(this, actual, outcome == expectedOutcome);

        public ValidationResultConstraint(RuleOutcome expectedOutcome)
        {
            this.expectedOutcome = expectedOutcome;
            Description = $"Validation result with outcome {expectedOutcome}";
        }
    }
}