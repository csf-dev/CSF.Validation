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
    public class AllChildrenMustBeValues : IRuleWithMessage<ManifestItem>
    {
        /// <inheritdoc/>
        public Task<string> GetFailureMessageAsync(ManifestItem value, ValidationRuleResult result, CancellationToken token = default)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<RuleResult> GetResultAsync(ManifestItem validated, RuleContext context, CancellationToken token = default)
        {
            if(validated?.Children is null) return PassAsync();
            return validated.Children.All(x => x.IsValue) ? PassAsync() : FailAsync();
        }
    }
}