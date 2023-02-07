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
    public class RootValueMustBeForSameTypeAsManifest : IRuleWithMessage<ManifestItem,ValidationManifest>
    {
        /// <inheritdoc/>
        public Task<RuleResult> GetResultAsync(ManifestItem value, ValidationManifest parentValue, RuleContext context, CancellationToken token = default)
        {
            if(parentValue?.ValidatedType is null) return PassAsync();
            if(value?.ValidatedType is null) return PassAsync();
            return value.ValidatedType.IsAssignableFrom(parentValue.ValidatedType) ? PassAsync() : FailAsync();
        }
        
        /// <inheritdoc/>
        public Task<string> GetFailureMessageAsync(ManifestItem value, ValidationManifest parentValue, ValidationRuleResult result, CancellationToken token = default)
        {
            var message = string.Format(FailureMessages.GetFailureMessage("RootValueMustBeForSameTypeAsManifest"),
                                        nameof(ManifestItem.ValidatedType),
                                        nameof(ValidationManifest),
                                        nameof(ManifestItem),
                                        nameof(ValidationManifest.RootValue),
                                        parentValue?.ValidatedType,
                                        value?.ValidatedType);
            return Task.FromResult(message);
        }
    }
}