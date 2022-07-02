using CSF.Validation.Rules;
using CSF.Validation.ValidatorBuilding;

namespace CSF.Validation.IntegrationTests
{
    public class CustomerValidatorBuilder : IBuildsValidator<Customer>
    {
        public void ConfigureValidator(IConfiguresValidator<Customer> config)
        {
            config.ForMember(x => x.Person, v =>
            {
                v.AddRules<PersonValidatorBuilder>();

                v.AddRule<MayNotHaveMoreThan5Pets>(r => {
                    r.AddDependency(d => d.RuleType<NotNull>().ForMember(nameof(Person.Pets)));
                });
            });

            config.AddRule<NotNull>();

            config.WhenValueIs<FrequentShopper>(c =>
            {
                c.AddRules<FrequentShopperPolymorphicBuilder>();
            });
        }
    }
}