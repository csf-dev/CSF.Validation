using System.Threading.Tasks;
using NUnit.Framework.Constraints;

namespace CSF.Validation
{
    public class ValidationResultConstraint : Constraint
    {
        readonly bool expectedPass;

        public override ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            if(actual is ValidationResult result)
                return new ValidationResultConstraintResult(this, result, result.Passed == expectedPass);
            if(actual is Task<ValidationResult> resultTask)
                return new ValidationResultConstraintResult(this, resultTask.Result, resultTask.Result.Passed == expectedPass);
            
            return new ConstraintResult(this, actual, ConstraintStatus.Error);
        }

        public ValidationResultConstraint(bool expectedPass)
        {
            this.expectedPass = expectedPass;
            Description = expectedPass ? "Passing validation result" : "Failing validation result";
        }

        class ValidationResultConstraintResult : ConstraintResult
        {
            public override void WriteActualValueTo(MessageWriter writer)
            {
                if(ActualValue is ValidationResult result)
                {
                    writer.WriteLine(result);
                    return;
                }

                base.WriteActualValueTo(writer);
            }

            public ValidationResultConstraintResult(Constraint constraint, object actual, bool outcome) : base(constraint, actual, outcome) {}
        }
    }
}