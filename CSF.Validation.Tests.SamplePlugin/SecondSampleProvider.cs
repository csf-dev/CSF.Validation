using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Messages;

namespace CSF.Validation
{
    public class SecondSampleProvider : IGetsFailureMessage<string>
    {
        public ValueTask<string> GetFailureMessageAsync(string val, ValidationRuleResult result, CancellationToken token = default)
            => throw new System.NotImplementedException();
    }
}