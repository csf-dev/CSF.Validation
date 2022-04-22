using System;
using CSF.Validation.Rules;
using CSF.Validation.ValidatorBuilding;

namespace CSF.Validation.IntegrationTests
{
    public class PersonValidatorBuilder : IBuildsValidator<Person>
    {
        public void ConfigureValidator(IConfiguresValidator<Person> config)
        {
            config.UseObjectIdentity(p => p.Name);

            config.AddRule<NotNull>();

            config.ForMember(x => x.Name, m =>
            {
                m.AddRule<NotNullOrEmpty>();
            });

            config.ForMember(x => x.Birthday, m =>
            {
                m.AddRule<DateTimeInRange>(r =>
                {
                    r.Name = "NotBornBefore1900";
                    r.ConfigureRule(c => c.Start = new DateTime(1900, 1, 1));
                });
            });

            config.ForMember(x => x.Pets, m => m.AddRule<NotNull>());

            config.ForMemberItems(x => x.Pets, m => m.AddRules<PetValidatorBuilder>());
        }
    }
}