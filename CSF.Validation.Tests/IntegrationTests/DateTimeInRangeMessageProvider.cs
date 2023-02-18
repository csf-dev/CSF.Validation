using System;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Messages;
using CSF.Validation.Rules;

namespace CSF.Validation.IntegrationTests
{
    [FailureMessageStrategy(RuleType = typeof(DateTimeInRange))]
    public class DateTimeInRangeMessageProvider : IGetsFailureMessage<DateTime>
    {
        public ValueTask<string> GetFailureMessageAsync(DateTime value, ValidationRuleResult result, CancellationToken token = default)
        {
            var message = $"The date {value.ToString("yyyy-MM-dd")} is invalid. ";
            var ruleObject = (DateTimeInRange)result.ValidationLogic.RuleObject;

            if(ruleObject.Start.HasValue && ruleObject.End.HasValue)
                message = message + $"It must be between {ruleObject.Start.Value.ToString("yyyy-MM-dd")} and {ruleObject.End.Value.ToString("yyyy-MM-dd")} (inclusive).";
            else if(ruleObject.Start.HasValue)
                message = message + $"It must equal-to or later than {ruleObject.Start.Value.ToString("yyyy-MM-dd")}.";
            else
                message = message + $"It must equal-to or before than {ruleObject.End.Value.ToString("yyyy-MM-dd")}.";

            return new ValueTask<string>(message);
        }
    }
}