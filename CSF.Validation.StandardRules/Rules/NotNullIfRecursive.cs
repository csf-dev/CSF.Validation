using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Manifest;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A rule which asserts that a value is not null if the parent <see cref="ManifestItem"/> is a recursive one.
    /// </summary>
    public class NotNullIfRecursive : IRuleWithMessage<ManifestItem, ManifestItem>
    {
        /// <inheritdoc/>
        public Task<string> GetFailureMessageAsync(ManifestItem value, ManifestItem parentValue, ValidationRuleResult result, CancellationToken token = default)
            => Task.FromResult(Resources.FailureMessages.GetFailureMessage("NotNullIfRecursive"));

        /// <inheritdoc/>
        public Task<RuleResult> GetResultAsync(ManifestItem value, ManifestItem parentValue, RuleContext context, CancellationToken token = default)
        {
            if(parentValue?.IsRecursive != true) return PassAsync();
            return ReferenceEquals(value, null) ? FailAsync() : PassAsync();
        }
    }
}