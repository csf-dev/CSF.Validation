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
    [Parallelizable]
    public class NullIfTheParentIsAManifest : IRuleWithMessage<object, ManifestItem>
    {
        /// <inheritdoc/>
        public ValueTask<RuleResult> GetResultAsync(object value, ManifestItem parentValue, RuleContext context, CancellationToken token = default)
            => (!(parentValue?.Parent is ValidationManifest) || value is null) ? PassAsync() : FailAsync();

        /// <inheritdoc/>
        public ValueTask<string> GetFailureMessageAsync(object value, ManifestItem parentValue, ValidationRuleResult result, CancellationToken token = default)
            => new ValueTask<string>(string.Format(Resources.FailureMessages.GetFailureMessage("NullIfTheParentIsAManifest"),
                                             nameof(ManifestItem.Parent),
                                             nameof(ValidationManifest)));
    }
}