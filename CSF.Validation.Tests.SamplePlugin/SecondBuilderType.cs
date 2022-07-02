using CSF.Validation.ValidatorBuilding;

namespace CSF.Validation
{
    public class SecondBuilderType : IBuildsValidator<string>
    {
        public void ConfigureValidator(IConfiguresValidator<string> config)
        {
            throw new System.NotImplementedException();
        }
    }
}