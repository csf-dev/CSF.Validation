using System;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Rules;

namespace CSF.Validation.Stubs
{
    public class StringValueRule : IValueRule<string, ValidatedObject>, IValueRule<string, object>
    {
        public Task<RuleResult> GetResultAsync(string value, ValidatedObject validated, RuleContext context, CancellationToken token = default)
            => throw new NotImplementedException();

        public Task<RuleResult> GetResultAsync(string value, object validated, RuleContext context, CancellationToken token = default)
            => throw new NotImplementedException();
    }
}