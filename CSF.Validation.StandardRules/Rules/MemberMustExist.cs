using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Manifest;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A rule which asserts that the value indicates a member which exists upon the <see cref="ManifestItem.ValidatedType"/> of the <see cref="ManifestValue"/>.
    /// </summary>
    public class MemberMustExist : IRuleWithMessage<string, ManifestValue>
    {
        /// <inheritdoc/>
        public Task<RuleResult> GetResultAsync(string value, ManifestValue parentValue, RuleContext context, CancellationToken token = default)
        {
            if(value is null || parentValue?.ValidatedType is null) return PassAsync();
            return parentValue.ValidatedType.GetMember(value).Any() ? PassAsync() : FailAsync();
        }

        /// <inheritdoc/>
        public Task<string> GetFailureMessageAsync(string value, ManifestValue parentValue, ValidationRuleResult result, CancellationToken token = default)
        {
            var message = string.Format(Resources.FailureMessages.GetFailureMessage("MemberMustExist"),
                                        nameof(ManifestItem.ValidatedType),
                                        parentValue.ValidatedType,
                                        value,
                                        nameof(ManifestValue.MemberName),
                                        nameof(ManifestValue));
            return Task.FromResult(message);
        }
    }
}