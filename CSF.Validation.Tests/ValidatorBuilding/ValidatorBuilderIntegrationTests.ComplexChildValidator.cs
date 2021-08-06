using CSF.Validation.Stubs;

namespace CSF.Validation.ValidatorBuilding
{
    public partial class ValidatorBuilderIntegrationTests
    {
        public class ComplexChildValidator : IBuildsValidator<ComplexObject>
        {
            public void ConfigureValidator(IConfiguresValidator<ComplexObject> config)
            {
                config.UseObjectIdentity(x => x.Identity);

                config.AddRule<ObjectRule>();

                config.ForMember(x => x.StringProperty, m => m.AddValueRule<StringValueRule>());

                config.ForValue(x => x.IntegerMethod(), m => m.AddRule<IntegerRule>());
            }
        }
    }
}