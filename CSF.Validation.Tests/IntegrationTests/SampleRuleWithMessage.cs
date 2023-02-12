using System;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Rules;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.IntegrationTests
{
    public class SampleRuleWithMessage<TValidated> : IRuleWithMessage<TValidated>
    {
        public Guid GuidProperty { get; set; }

        public Task<string> GetFailureMessageAsync(TValidated value, ValidationRuleResult result, CancellationToken token = default)
            => Task.FromResult(GuidProperty.ToString());

        public Task<RuleResult> GetResultAsync(TValidated validated, RuleContext context, CancellationToken token = default)
            => FailAsync();
    }
}