using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Messages;

namespace CSF.Validation.IntegrationTests
{
    [FailureMessageStrategy(RuleType = typeof(CantBeOwnedByUnderageChildren))]
    public class CantBeOwnedByUnderageChildrenMessageProvider : IGetsFailureMessage<Pet>
    {
        public ValueTask<string> GetFailureMessageAsync(Pet value, ValidationRuleResult result, CancellationToken token = default)
        {
            return new ValueTask<string>($"The pet cannot be owned by a child under {value.MinimumAgeToOwn} years old, " +
                                   $"but the child is only {result.Data[CantBeOwnedByUnderageChildren.ActualAgeKey]} years old.");
        }
    }
}