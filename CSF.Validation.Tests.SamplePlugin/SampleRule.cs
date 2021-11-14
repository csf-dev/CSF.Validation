using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Rules;

namespace CSF.Validation
{
    public class SampleRule : IRule<object>
    {
        public Task<RuleResult> GetResultAsync(object validated, RuleContext context, CancellationToken token = default)
        {
            throw new System.NotImplementedException();
        }
    }
}