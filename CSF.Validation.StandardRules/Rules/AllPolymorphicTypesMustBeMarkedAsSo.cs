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
    public class AllPolymorphicTypesMustBeMarkedAsSo : IRuleWithMessage<ManifestItem>
    {
        /// <inheritdoc/>
        public Task<string> GetFailureMessageAsync(ManifestItem value, ValidationRuleResult result, CancellationToken token = default)
        {
            // TODO: Write this impl
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<RuleResult> GetResultAsync(ManifestItem validated, RuleContext context, CancellationToken token = default)
        {
            if(validated?.PolymorphicTypes is null) return PassAsync();
            return validated.PolymorphicTypes.All(x => x.IsPolymorphicType) ? PassAsync() : FailAsync();
        }
    }
}