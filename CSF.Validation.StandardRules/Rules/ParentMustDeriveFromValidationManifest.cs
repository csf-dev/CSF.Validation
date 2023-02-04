using System;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Manifest;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A validation rule which asserts that the <see cref="ManifestItem.Parent"/> is an instance of
    /// <see cref="ValidationManifest"/>.
    /// </summary>
    public class ParentMustDeriveFromValidationManifest : IRuleWithMessage<ManifestValue>
    {
        /// <inheritdoc/>
        public Task<RuleResult> GetResultAsync(ManifestValue validated, RuleContext context, CancellationToken token = default)
        {
            if(validated?.Parent is null) return PassAsync();
            return validated.Parent is ValidationManifest ? PassAsync() : FailAsync();
        }

        /// <inheritdoc/>
        public Task<string> GetFailureMessageAsync(ManifestValue value, ValidationRuleResult result, CancellationToken token = default)
        {
            var message = String.Format(Resources.FailureMessages.GetFailureMessage("ParentMustDeriveFromValidationManifest"),
                                        nameof(ManifestItem.Parent),
                                        nameof(ManifestValue),
                                        nameof(ValidationManifest));
            return Task.FromResult(message);
        }
    }
}