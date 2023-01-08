using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Manifest;
using CSF.Validation.Resources;
using CSF.Validation.Rules;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.ValidatorValidation
{
    /// <summary>
    /// A rule that ensures that a <see cref="ManifestValue"/> has a <see langword="null" /> <see cref="ManifestValue.MemberName"/>.
    /// Typically used for the root manifest value in a manifest.
    /// </summary>
    public class RootValueMustNotHaveAMemberName : IRuleWithMessage<ManifestValue>
    {
        /// <inheritdoc/>
        public Task<string> GetFailureMessageAsync(ManifestValue value, ValidationRuleResult result, CancellationToken token = default)
        {
            var message = string.Format(FailureMessages.GetFailureMessage("RootValueMustNotHaveAParent"),
                                        nameof(ManifestValue.MemberName),
                                        nameof(ManifestValue),
                                        nameof(ValidationManifest));
            return Task.FromResult(message);
        }

        /// <inheritdoc/>
        public Task<RuleResult> GetResultAsync(ManifestValue validated, RuleContext context, CancellationToken token = default)
            => validated?.MemberName is null ? PassAsync() : FailAsync();
    }
}