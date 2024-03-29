using AutoFixture;

namespace CSF.Validation.RuleExecution
{
    public class ExecutableModelCustomisation : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            new Manifest.ManifestModelCustomization().Customize(fixture);

            fixture.Customize<ValidatedValue>(c => c.Without(x => x.ParentValue).Without(x => x.Rules).Without(x => x.CollectionItems));
            fixture.Customize<GetValueToBeValidatedResponse>(x => x.FromFactory((object obj) => new SuccessfulGetValueToBeValidatedResponse(obj)));
            fixture.Customize<ExecutableRule>(c => c.Without(x => x.Result));
            fixture.Customize<ExecutableRuleAndDependencies>(c => c.FromFactory((ExecutableRule rule) => new ExecutableRuleAndDependencies(rule)));
        }
    }
}