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

        public ValueTask<string> GetFailureMessageAsync(TValidated value, ValidationRuleResult result, CancellationToken token = default)
            => new ValueTask<string>(GuidProperty.ToString());

        public ValueTask<RuleResult> GetResultAsync(TValidated validated, RuleContext context, CancellationToken token = default)
            => FailAsync();
    }
}