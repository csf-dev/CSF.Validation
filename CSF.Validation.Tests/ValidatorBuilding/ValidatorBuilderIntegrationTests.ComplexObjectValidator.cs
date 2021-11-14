using CSF.Validation.Stubs;

namespace CSF.Validation.ValidatorBuilding
{
    public partial class ValidatorBuilderIntegrationTests
    {
        public class ComplexObjectValidator : IBuildsValidator<ComplexObject>
        {
            public void ConfigureValidator(IConfiguresValidator<ComplexObject> config)
            {
                config.UseObjectIdentity(x => x.Identity);

                config.AddRule<ObjectRule>();

                config.ForMember(x => x.Associated, m => m.AddRules<ComplexChildValidator>());

                config.ForMemberItems(x => x.Children, m => m.AddRules<ComplexChildValidator>());

                config.ForMember(x => x.StringProperty, m =>
                {
                    m.AddValueRule<StringValueRule>();
                    m.AddRule<ObjectRule>();
                });
            }
        }
    }
}