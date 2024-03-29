using System;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Manifest;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A rule which asserts that the <see cref="ManifestItem.CollectionItemValue"/> property is either <see langword="null" />
    /// or contains a manifest item with the type <see cref="ManifestItemTypes.CollectionItem"/>.
    /// </summary>
    [Parallelizable]
    public class CollectionItemValueMustBeCollectionItem : IRuleWithMessage<ManifestItem>
    {
        /// <inheritdoc/>
        public ValueTask<string> GetFailureMessageAsync(ManifestItem value, ValidationRuleResult result, CancellationToken token = default)
            => new ValueTask<string>(String.Format(Resources.FailureMessages.GetFailureMessage("CollectionItemValueMustBeCollectionItem"),
                                             nameof(ManifestItem),
                                             nameof(ManifestItem.CollectionItemValue),
                                             nameof(ManifestItemTypes.CollectionItem)));

        /// <inheritdoc/>
        public ValueTask<RuleResult> GetResultAsync(ManifestItem validated, RuleContext context, CancellationToken token = default)
        {
            if(validated?.CollectionItemValue is null) return PassAsync();
            return validated.CollectionItemValue.IsCollectionItem ? PassAsync() : FailAsync();
        }
    }
}