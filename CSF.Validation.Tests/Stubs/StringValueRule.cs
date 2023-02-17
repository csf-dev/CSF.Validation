using System;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Rules;

namespace CSF.Validation.Stubs
{
    public class StringValueRule : IRule<string, ValidatedObject>, IRule<string, object>
    {
        public ValueTask<RuleResult> GetResultAsync(string value, ValidatedObject validated, RuleContext context, CancellationToken token = default)
            => throw new NotImplementedException();

        public ValueTask<RuleResult> GetResultAsync(string value, object validated, RuleContext context, CancellationToken token = default)
            => throw new NotImplementedException();
    }
}