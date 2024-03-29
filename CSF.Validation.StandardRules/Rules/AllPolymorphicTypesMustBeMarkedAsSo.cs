using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Manifest;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A rule that asserts all members of the <see cref="ManifestItem.PolymorphicTypes"/> collection have the value
    /// type <see cref="ManifestItemTypes.PolymorphicType"/>.
    /// </summary>
    [Parallelizable]
    public class AllPolymorphicTypesMustBeMarkedAsSo : IRuleWithMessage<ManifestItem>
    {
        /// <inheritdoc/>
        public ValueTask<string> GetFailureMessageAsync(ManifestItem value, ValidationRuleResult result, CancellationToken token = default)
            => new ValueTask<string>(String.Format(Resources.FailureMessages.GetFailureMessage("AllPolymorphicTypesMustBeMarkedAsSo"),
                                             nameof(ManifestItem),
                                             nameof(ManifestItem.PolymorphicTypes),
                                             nameof(ManifestItemTypes.PolymorphicType)));

        /// <inheritdoc/>
        public ValueTask<RuleResult> GetResultAsync(ManifestItem validated, RuleContext context, CancellationToken token = default)
        {
            if(validated?.PolymorphicTypes is null) return PassAsync();
            return validated.PolymorphicTypes.All(x => x.IsPolymorphicType) ? PassAsync() : FailAsync();
        }
    }
}