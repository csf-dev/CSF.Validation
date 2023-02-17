using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Manifest;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A rule that asserts all members of the <see cref="ManifestItem.Children"/> collection have the value type <see cref="ManifestItemTypes.Value"/>.
    /// </summary>
    [Parallelizable]
    public class AllChildrenMustBeValues : IRuleWithMessage<ManifestItem>
    {
        /// <inheritdoc/>
        public ValueTask<string> GetFailureMessageAsync(ManifestItem value, ValidationRuleResult result, CancellationToken token = default)
            => new ValueTask<string>(String.Format(Resources.FailureMessages.GetFailureMessage("AllChildrenMustBeValues"),
                                             nameof(ManifestItem),
                                             nameof(ManifestItem.Children),
                                             nameof(ManifestItemTypes.Value)));

        /// <inheritdoc/>
        public ValueTask<RuleResult> GetResultAsync(ManifestItem validated, RuleContext context, CancellationToken token = default)
        {
            if(validated?.Children is null) return PassAsync();
            return validated.Children.All(x => x.IsValue) ? PassAsync() : FailAsync();
        }
    }
}