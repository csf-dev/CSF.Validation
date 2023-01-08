using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Manifest;
using CSF.Validation.Resources;
using CSF.Validation.Rules;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.ValidatorValidation
{
    /// <summary>
    /// A rule which ensures that the <see cref="ManifestItem.ValidatedType"/> of the <see cref="ValidationManifest.RootValue"/>
    /// is for the same (or a less-derived) type as the <see cref="ValidationManifest.ValidatedType"/>.
    /// </summary>
    public class RootValueMustBeForSameTypeAsManifest : IRuleWithMessage<ValidationManifest>
    {
        /// <inheritdoc/>
        public Task<string> GetFailureMessageAsync(ValidationManifest value, ValidationRuleResult result, CancellationToken token = default)
        {
            var message = string.Format(FailureMessages.GetFailureMessage("RootValueMustBeForSameTypeAsManifest"),
                                        nameof(ManifestItem.ValidatedType),
                                        nameof(ValidationManifest.RootValue),
                                        $"{nameof(ValidationManifest)}.{nameof(ValidationManifest.ValidatedType)}",
                                        $"{nameof(ValidationManifest)}.{nameof(ValidationManifest.RootValue)}.{nameof(ManifestItem.ValidatedType)}",
                                        value?.RootValue?.ValidatedType?.AssemblyQualifiedName,
                                        $"{nameof(ValidationManifest)}.{nameof(ValidationManifest.ValidatedType)}",
                                        value?.ValidatedType?.AssemblyQualifiedName);
            return Task.FromResult(message);
        }

        /// <inheritdoc/>
        public Task<RuleResult> GetResultAsync(ValidationManifest validated, RuleContext context, CancellationToken token = default)
        {
            if(validated?.ValidatedType is null) return PassAsync();
            if(validated?.RootValue?.ValidatedType is null) return PassAsync();
            return validated.ValidatedType.IsAssignableFrom(validated.RootValue.ValidatedType) ? PassAsync() : FailAsync();
        }
    }
}