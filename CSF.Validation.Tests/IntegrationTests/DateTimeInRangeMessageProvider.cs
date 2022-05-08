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
        public Task<string> GetFailureMessageAsync(DateTime value, ValidationRuleResult result, CancellationToken token = default)
        {
            var message = $"The date & time {value} is invalid. ";
            var ruleObject = (DateTimeInRange)result.ValidationLogic.RuleObject;

            if(ruleObject.Start.HasValue && ruleObject.End.HasValue)
                message = message + $"It must be between {ruleObject.Start} and {ruleObject.End} (inclusive).";
            else if(ruleObject.Start.HasValue)
                message = message + $"It must equal-to or later than {ruleObject.Start}.";
            else
                message = message + $"It must equal-to or before than {ruleObject.End}.";

            return Task.FromResult(message);
        }
    }
}