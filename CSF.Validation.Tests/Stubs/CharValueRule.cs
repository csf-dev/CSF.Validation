using System;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Rules;

namespace CSF.Validation.Stubs
{
    public class CharValueRule : IRule<char, ValidatedObject>
    {
        public ValueTask<RuleResult> GetResultAsync(char value, ValidatedObject validated, RuleContext context, CancellationToken token = default)
            => throw new NotImplementedException();
    }
}