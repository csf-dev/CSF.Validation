using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Messages;

namespace CSF.Validation
{
    public class SampleProvider : IGetsFailureMessage
    {
        public ValueTask<string> GetFailureMessageAsync(ValidationRuleResult result, CancellationToken token = default)
            => throw new System.NotImplementedException();
    }
}