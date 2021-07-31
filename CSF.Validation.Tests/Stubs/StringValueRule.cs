using System;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Rules;

namespace CSF.Validation.Stubs
{
    public class StringValueRule : IValueRule<string, ValidatedObject>
    {
        public Task<RuleResult> GetResultAsync(string value, ValidatedObject validated, ValueRuleContext context, CancellationToken token = default)
            => throw new NotImplementedException();
    }
}