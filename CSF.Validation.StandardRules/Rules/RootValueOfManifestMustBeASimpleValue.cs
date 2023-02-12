using System;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Manifest;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A rule which verifies that the root manifest item within a validation manifest has an
    /// item type equal to <see cref="ManifestItemTypes.Value"/>.
    /// </summary>
    [Parallelizable]
    public class RootValueOfManifestMustBeASimpleValue : IRuleWithMessage<ValidationManifest>
    {
        /// <inheritdoc/>
        public Task<string> GetFailureMessageAsync(ValidationManifest value, ValidationRuleResult result, CancellationToken token = default)
            => Task.FromResult(String.Format(Resources.FailureMessages.GetFailureMessage("RootValueOfManifestMustBeASimpleValue"),
                                             nameof(ValidationManifest),
                                             nameof(ValidationManifest.RootValue),
                                             nameof(ManifestItemTypes.Value)));

        /// <inheritdoc/>
        public Task<RuleResult> GetResultAsync(ValidationManifest validated, RuleContext context, CancellationToken token = default)
        {
            if(validated?.RootValue is null) return PassAsync();
            return validated.RootValue.ItemType == ManifestItemTypes.Value ? PassAsync() : FailAsync();
        }
    }
}