using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Manifest;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A rule which asserts that a value is <see langword="null" /> if the <see cref="ManifestItem.Parent"/>
    /// is an instance of <see cref="ValidationManifest"/>.
    /// </summary>
    public class NullIfTheParentIsAManifest : IRuleWithMessage<object, ManifestValue>
    {
        /// <inheritdoc/>
        public Task<RuleResult> GetResultAsync(object value, ManifestValue parentValue, RuleContext context, CancellationToken token = default)
            => (!(parentValue?.Parent is ValidationManifest) || value is null) ? PassAsync() : FailAsync();

        /// <inheritdoc/>
        public Task<string> GetFailureMessageAsync(object value, ManifestValue parentValue, ValidationRuleResult result, CancellationToken token = default)
            => Task.FromResult(string.Format(Resources.FailureMessages.GetFailureMessage("NullIfTheParentIsAManifest"),
                                             nameof(ManifestItem.Parent),
                                             nameof(ValidationManifest)));
    }
}