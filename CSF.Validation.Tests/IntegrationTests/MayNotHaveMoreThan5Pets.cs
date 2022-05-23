using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Rules;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.IntegrationTests
{
    public class MayNotHaveMoreThan5Pets : IRuleWithMessage<Person>
    {
        public Task<string> GetFailureMessageAsync(Person value, ValidationRuleResult result, CancellationToken token = default)
            => Task.FromResult("Nobody may have more than 5 pets.");

        public Task<RuleResult> GetResultAsync(Person validated, RuleContext context, CancellationToken token = default)
        {
            if(validated?.Pets is null) return PassAsync();
            return ResultAsync(validated.Pets.Count <= 5);
        }
    }
}