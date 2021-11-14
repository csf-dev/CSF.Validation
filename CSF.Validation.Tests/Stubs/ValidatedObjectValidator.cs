using CSF.Validation.ValidatorBuilding;

namespace CSF.Validation.Stubs
{
    public class ValidatedObjectValidator : IBuildsValidator<ValidatedObject>
    {
        public void ConfigureValidator(IConfiguresValidator<ValidatedObject> config)
            => throw new System.NotImplementedException();
    }
}