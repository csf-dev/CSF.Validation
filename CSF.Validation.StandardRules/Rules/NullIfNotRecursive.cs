using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Manifest;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A rule which asserts that a value is null if the parent <see cref="ManifestItem"/> is not a recursive one.
    /// </summary>
    [Parallelizable]
    public class NullIfNotRecursive : IRuleWithMessage<object, ManifestItem>
    {
        /// <inheritdoc/>
        public ValueTask<string> GetFailureMessageAsync(object value, ManifestItem parentValue, ValidationRuleResult result, CancellationToken token = default)
            => new ValueTask<string>(Resources.FailureMessages.GetFailureMessage("NullIfNotRecursive"));

        /// <inheritdoc/>
        public ValueTask<RuleResult> GetResultAsync(object value, ManifestItem parentValue, RuleContext context, CancellationToken token = default)
        {
            if(parentValue?.IsRecursive != false) return PassAsync();
            return ReferenceEquals(value, null) ? PassAsync() : FailAsync();
        }
    }
}