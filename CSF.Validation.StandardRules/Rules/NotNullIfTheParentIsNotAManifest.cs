using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Manifest;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A rule which asserts that a value is not <see langword="null" /> if the <see cref="ManifestItem.Parent"/>
    /// is not an instance of <see cref="ValidationManifest"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This rule is used to indicate mandatory (not-<see langword="null" />) properties upon any <see cref="ManifestItem"/>, when that
    /// manifest value is not the root value of a validation manifest.
    /// </para>
    /// </remarks>
    public class NotNullIfTheParentIsNotAManifest : IRuleWithMessage<object, ManifestItem>
    {
        /// <inheritdoc/>
        public Task<RuleResult> GetResultAsync(object value, ManifestItem parentValue, RuleContext context, CancellationToken token = default)
            => (parentValue?.Parent is ValidationManifest || !(value is null)) ? PassAsync() : FailAsync();

        /// <inheritdoc/>
        public Task<string> GetFailureMessageAsync(object value, ManifestItem parentValue, ValidationRuleResult result, CancellationToken token = default)
            => Task.FromResult(string.Format(Resources.FailureMessages.GetFailureMessage("NotNullIfTheParentIsNotAManifest"),
                                             nameof(ManifestItem),
                                             nameof(ValidationManifest)));
    }
}