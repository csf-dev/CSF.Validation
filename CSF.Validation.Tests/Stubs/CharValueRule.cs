using System;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Rules;

namespace CSF.Validation.Stubs
{
    public class CharValueRule : IValueRule<char, ValidatedObject>
    {
        public Task<RuleResult> GetResultAsync(char value, ValidatedObject validated, ValueRuleContext context, CancellationToken token = default)
            => throw new NotImplementedException();
    }
}