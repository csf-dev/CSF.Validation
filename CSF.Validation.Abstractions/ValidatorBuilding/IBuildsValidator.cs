namespace CSF.Validation.ValidatorBuilding
{
    public interface IBuildsValidator<TValidated>
    {
        void BuildValidator(IConfiguresValidator<TValidated> validator);
    }
}