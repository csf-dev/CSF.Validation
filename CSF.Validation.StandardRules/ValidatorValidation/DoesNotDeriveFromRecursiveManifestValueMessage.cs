using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Manifest;
using CSF.Validation.Messages;
using CSF.Validation.Resources;
using CSF.Validation.Rules;

namespace CSF.Validation.ValidatorValidation
{
    /// <summary>
    /// A validation failure message provider for the rule which ensures that the <see cref="ValidationManifest.RootValue"/>
    /// of a <see cref="ValidationManifest"/> does not derive from <see cref="RecursiveManifestValue"/>.
    /// </summary>
    [FailureMessageStrategy(RuleType = typeof(DoesNotDeriveFrom<RecursiveManifestValue>),
                            MemberName = nameof(ValidationManifest.RootValue),
                            ValidatedType = typeof(ManifestValue),
                            ParentValidatedType = typeof(ValidationManifest),
                            RuleName = ValidationManifestValidatorBuilder.RootValueOfManifestMustNotBeRecursive)]
    public class DoesNotDeriveFromRecursiveManifestValueMessage : IGetsFailureMessage<ManifestValue>
    {
        /// <inheritdoc/>
        public Task<string> GetFailureMessageAsync(ManifestValue value, ValidationRuleResult result, CancellationToken token = default)
        {
            var message = string.Format(FailureMessages.GetFailureMessage("DoesNotDeriveFromRecursiveManifestValueMessage"),
                                        nameof(ValidationManifest.RootValue),
                                        nameof(ValidationManifest),
                                        nameof(RecursiveManifestValue));
            return Task.FromResult(message);
        }
    }
}