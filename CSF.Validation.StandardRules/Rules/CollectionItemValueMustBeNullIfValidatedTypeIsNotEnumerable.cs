using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Manifest;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A rule which asserts that - if the <see cref="ManifestItem.ValidatedType"/> of a <see cref="ManifestItem"/> is
    /// not a type that derives from <see cref="IEnumerable{T}"/> then the <see cref="ManifestItem.CollectionItemValue"/>
    /// of that manifest item must be null.
    /// </summary>
    [Parallelizable]
    public class CollectionItemValueMustBeNullIfValidatedTypeIsNotEnumerable : IRuleWithMessage<ManifestItem>
    {
        /// <inheritdoc/>
        public ValueTask<RuleResult> GetResultAsync(ManifestItem validated, RuleContext context, CancellationToken token = default)
        {
            if(validated?.CollectionItemValue is null || validated?.ValidatedType is null) return PassAsync();
            return typeof(IEnumerable<object>).IsAssignableFrom(validated.ValidatedType) ? PassAsync() : FailAsync();
        }

        /// <inheritdoc/>
        public ValueTask<string> GetFailureMessageAsync(ManifestItem value, ValidationRuleResult result, CancellationToken token = default)
        {
            var message = String.Format(Resources.FailureMessages.GetFailureMessage("CollectionItemValueMustBeNullIfValidatedTypeIsNotEnumerable"),
                                        nameof(ManifestItem.ValidatedType),
                                        nameof(ManifestItem),
                                        nameof(IEnumerable<object>),
                                        nameof(Object),
                                        nameof(ManifestItem.CollectionItemValue));
            return new ValueTask<string>(message);
        }
    }
}