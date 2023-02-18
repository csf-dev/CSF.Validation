using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Messages;

namespace CSF.Validation
{
    public class ThirdSampleProvider : IGetsFailureMessage<string,object>
    {
        public ValueTask<string> GetFailureMessageAsync(string val1, object val2, ValidationRuleResult result, CancellationToken token = default)
            => throw new System.NotImplementedException();
    }
}