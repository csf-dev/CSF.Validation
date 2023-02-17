using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Rules;

namespace CSF.Validation.Stubs
{
    public class IntegerRule : IRule<int>
    {
        public ValueTask<RuleResult> GetResultAsync(int validated, RuleContext context, CancellationToken token = default)
            => throw new System.NotImplementedException();
    }
}