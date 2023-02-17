using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Rules;

namespace CSF.Validation
{
    public class SampleRuleWithParent : IRule<string,object>
    {
        public ValueTask<RuleResult> GetResultAsync(string validated, object parentValue, RuleContext context, CancellationToken token = default)
        {
            throw new System.NotImplementedException();
        }
    }
}