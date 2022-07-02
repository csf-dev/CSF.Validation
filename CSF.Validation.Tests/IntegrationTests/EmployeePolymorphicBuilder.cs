using CSF.Validation.Rules;
using CSF.Validation.ValidatorBuilding;

namespace CSF.Validation.IntegrationTests
{
    public class EmployeePolymorphicBuilder : IBuildsValidator<Employee>
    {
        public void ConfigureValidator(IConfiguresValidator<Employee> config)
        {
            config.ForMember(x => x.PayrollNumber, m =>
            {
                m.AddRule<IntegerInRange>(c =>
                {
                    c.ConfigureRule(r =>
                    {
                        r.Min = 1;
                        r.Max = 9999;
                    });
                });
            });
        }
    }
}