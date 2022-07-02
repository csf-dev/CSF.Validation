using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Rules;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.IntegrationTests
{
    public class NonParallelisableRule : IRule<object>
    {
        public int MillisecondsDelay { get; set; } = 100;

        public async Task<RuleResult> GetResultAsync(object validated, RuleContext context, CancellationToken token = default)
        {
            await Task.Delay(MillisecondsDelay, token);
            return Pass();
        }
    }
}