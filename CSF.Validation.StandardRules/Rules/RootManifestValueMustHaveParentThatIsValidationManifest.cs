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
    [Parallelizable]
    public class RootManifestValueMustHaveParentThatIsValidationManifest : IRuleWithMessage<ValidationManifest>
    {
        /// <inheritdoc/>
        public ValueTask<RuleResult> GetResultAsync(ValidationManifest validated, RuleContext context, CancellationToken token = default)
        {
            if(validated?.RootValue?.Parent is null) return PassAsync();
            return ReferenceEquals(validated.RootValue.Parent, validated) ? PassAsync() : FailAsync();
        }

        /// <inheritdoc/>
        public ValueTask<string> GetFailureMessageAsync(ValidationManifest value, ValidationRuleResult result, CancellationToken token = default)
        {
            var message = String.Format(Resources.FailureMessages.GetFailureMessage("ParentMustDeriveFromValidationManifest"),
                                        nameof(ManifestItem.Parent),
                                        nameof(ManifestItem));
            return new ValueTask<string>(message);
        }
    }
}