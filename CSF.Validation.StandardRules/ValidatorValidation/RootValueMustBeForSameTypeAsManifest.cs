using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Manifest;
using CSF.Validation.Resources;
using CSF.Validation.Rules;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.ValidatorValidation
{
    /// <summary>
    /// A rule which ensures that the <see cref="ManifestItem.ValidatedType"/> of the <see cref="ManifestValue"/> which has been
    /// used as the <see cref="ValidationManifest.RootValue"/> is for the same (or a less-derived) type as
    /// the <see cref="ValidationManifest.ValidatedType"/> of that manifest.
    /// </summary>
    public class RootValueMustBeForSameTypeAsManifest : IRuleWithMessage<ManifestValue,ValidationManifest>
    {
        /// <inheritdoc/>
        public Task<RuleResult> GetResultAsync(ManifestValue value, ValidationManifest parentValue, RuleContext context, CancellationToken token = default)
        {
            if(parentValue?.ValidatedType is null) return PassAsync();
            if(value?.ValidatedType is null) return PassAsync();
            return value.ValidatedType.IsAssignableFrom(parentValue.RootValue.ValidatedType) ? PassAsync() : FailAsync();
        }
        
        /// <inheritdoc/>
        public Task<string> GetFailureMessageAsync(ManifestValue value, ValidationManifest parentValue, ValidationRuleResult result, CancellationToken token = default)
        {
            var message = string.Format(FailureMessages.GetFailureMessage("RootValueMustBeForSameTypeAsManifest"),
                                        nameof(ManifestItem.ValidatedType),
                                        nameof(ValidationManifest.RootValue),
                                        $"{nameof(ValidationManifest)}.{nameof(ValidationManifest.ValidatedType)}",
                                        $"{nameof(ValidationManifest)}.{nameof(ValidationManifest.RootValue)}.{nameof(ManifestItem.ValidatedType)}",
                                        value?.ValidatedType?.AssemblyQualifiedName,
                                        $"{nameof(ValidationManifest)}.{nameof(ValidationManifest.ValidatedType)}",
                                        value?.ValidatedType?.AssemblyQualifiedName);
            return Task.FromResult(message);
        }
    }
}