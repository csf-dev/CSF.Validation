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
    public class NullIfNotRecursive : IRuleWithMessage<object, ManifestItem>
    {
        /// <inheritdoc/>
        public Task<string> GetFailureMessageAsync(object value, ManifestItem parentValue, ValidationRuleResult result, CancellationToken token = default)
        {
            // TODO: Write this impl
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<RuleResult> GetResultAsync(object value, ManifestItem parentValue, RuleContext context, CancellationToken token = default)
        {
            if(parentValue?.IsRecursive != false) return PassAsync();
            return ReferenceEquals(value, null) ? PassAsync() : FailAsync();
        }
    }
}