using CSF.Validation.ValidatorBuilding;

namespace CSF.Validation.IntegrationTests
{
    public class ParallelValidatorBuilder : IBuildsValidator<Person>
    {
        public void ConfigureValidator(IConfiguresValidator<Person> config)
        {
            // With 4 levels of parallelisation (set via a threadpool), this validator
            // should take very close to 1 second to completely execute.
            // Rules 1-4 should occur in parallel taking approx 200ms between all of them.
            // Rules 5 & 6 take 500ms & 300ms respectively and should take approx 800ms

            config.AddRule<ParallelisableRule>(c =>
            {
                c.ConfigureRule(r => r.MillisecondsDelay = 200);
                c.Name = "Rule 1";
            });
            config.AddRule<ParallelisableRule>(c =>
            {
                c.ConfigureRule(r => r.MillisecondsDelay = 50);
                c.Name = "Rule 2";
            });
            config.AddRule<ParallelisableRule>(c =>
            {
                c.ConfigureRule(r => r.MillisecondsDelay = 200);
                c.Name = "Rule 3";
            });
            config.AddRule<ParallelisableRule>(c =>
            {
                c.ConfigureRule(r => r.MillisecondsDelay = 50);
                c.Name = "Rule 4";
            });
            config.AddRule<NonParallelisableRule>(c =>
            {
                c.ConfigureRule(r => r.MillisecondsDelay = 500);
                c.Name = "Rule 5";
            });
            config.AddRule<NonParallelisableRule>(c =>
            {
                c.ConfigureRule(r => r.MillisecondsDelay = 300);
                c.Name = "Rule 6";
            });
        }
    }
}