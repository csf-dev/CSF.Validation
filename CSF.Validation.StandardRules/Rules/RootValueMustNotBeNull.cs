using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Manifest;
using CSF.Validation.Resources;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A rule which ensures that the <see cref="ValidationManifest.RootValue"/> is not <see langword="null" />.
    /// </summary>
    [Parallelizable]
    public class RootValueMustNotBeNull : IRuleWithMessage<ValidationManifest>
    {
        /// <inheritdoc/>
        public ValueTask<RuleResult> GetResultAsync(ValidationManifest validated, RuleContext context, CancellationToken token = default)
        {
            if (validated is null) return PassAsync();
            return validated?.RootValue is null ? FailAsync() : PassAsync();
        }

        /// <inheritdoc/>
        public ValueTask<string> GetFailureMessageAsync(ValidationManifest value, ValidationRuleResult result, CancellationToken token = default)
        {
            var message = string.Format(FailureMessages.GetFailureMessage("RootValueMustNotBeNull"),
                                        nameof(ValidationManifest));
            return new ValueTask<string>(message);
        }
    }
}