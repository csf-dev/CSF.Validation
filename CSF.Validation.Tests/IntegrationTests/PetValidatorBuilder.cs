using CSF.Validation.Rules;
using CSF.Validation.ValidatorBuilding;

namespace CSF.Validation.IntegrationTests
{
    public class PetValidatorBuilder : IBuildsValidator<Pet>
    {
        public void ConfigureValidator(IConfiguresValidator<Pet> config)
        {
            config.AddRule<NotNull>();
            config.AddRule<CantBeOwnedByUnderageChildren>();

            config.ForMember(x => x.Name, m =>
            {
                m.AddRule<NotNullOrEmpty>();
            });

            config.ForMember(x => x.NumberOfLegs, m =>
            {
                m.AddRule<IntegerInRange>(r =>
                {
                    r.ConfigureRule(c =>
                    {
                        // Can't have negative legs and millipedes have max 750 legs
                        c.Min = 0;
                        c.Max = 750;
                    });
                });
            });
        }
    }
}