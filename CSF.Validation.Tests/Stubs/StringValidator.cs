using System;
using CSF.Validation.ValidatorBuilding;

namespace CSF.Validation.Stubs
{
    public class StringValidator : IBuildsValidator<string>
    {
        public void ConfigureValidator(IConfiguresValidator<string> config)
            => throw new NotImplementedException();
    }
}