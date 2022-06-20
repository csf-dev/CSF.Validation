using CSF.Validation.Rules;
using CSF.Validation.ValidatorBuilding;

namespace CSF.Validation.IntegrationTests
{
    public class FrequentShopperPolymorphicBuilder : IBuildsValidator<FrequentShopper>
    {
        public void ConfigureValidator(IConfiguresValidator<FrequentShopper> config)
        {
            config.ForMember(x => x.LoyaltyPoints, m =>
            {
                m.AddRule<IntegerInRange>(c =>
                {
                    c.ConfigureRule(r => r.Min = 0);
                });
            });
        }
    }
}