using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Manifest;
using CSF.Validation.Resources;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A rule which ensures that the <see cref="ManifestItem.ValidatedType"/> of the <see cref="ManifestItem"/> which has been
    /// used as the <see cref="ValidationManifest.RootValue"/> is for the same (or a less-derived) type as
    /// the <see cref="ValidationManifest.ValidatedType"/> of that manifest.
    /// </summary>
    [Parallelizable]
    public class RootValueMustBeForSameTypeAsManifest : IRuleWithMessage<ValidationManifest>
    {
        /// <inheritdoc/>
        public ValueTask<RuleResult> GetResultAsync(ValidationManifest validated, RuleContext context, CancellationToken token = default)
        {
            if(validated?.ValidatedType is null) return PassAsync();
            if(validated?.RootValue?.ValidatedType is null) return PassAsync();
            return validated.RootValue.ValidatedType.IsAssignableFrom(validated.ValidatedType) ? PassAsync() : FailAsync();
        }

        /// <inheritdoc/>
        public ValueTask<string> GetFailureMessageAsync(ValidationManifest value, ValidationRuleResult result, CancellationToken token = default)
        {
            var message = string.Format(FailureMessages.GetFailureMessage("RootValueMustBeForSameTypeAsManifest"),
                                        nameof(ManifestItem.ValidatedType),
                                        nameof(ValidationManifest),
                                        nameof(ManifestItem),
                                        nameof(ValidationManifest.RootValue),
                                        value?.ValidatedType,
                                        value?.RootValue?.ValidatedType);
            return new ValueTask<string>(message);
        }
    }
}