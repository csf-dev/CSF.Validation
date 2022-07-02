using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Rules;

namespace CSF.Validation.Stubs
{
    public class RuleWithConstructorDependency : IRule<object>
    {
        public Task<RuleResult> GetResultAsync(object validated, RuleContext context, CancellationToken token = default)
            => throw new System.NotImplementedException();

        public RuleWithConstructorDependency(string dependency) {}
    }
}