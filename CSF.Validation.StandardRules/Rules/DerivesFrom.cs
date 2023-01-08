using System;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Resources;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A rule that passes if the validated value is <see langword="null" /> or if it derives from a specified type, and fails if it does not.
    /// </summary>
    public class DerivesFrom : IRule<object>
    {
        /// <summary>
        /// Gets or sets the type that the validated value must derive from.
        /// </summary>
        public Type Type { get; set; }

        /// <inheritdoc/>
        public Task<RuleResult> GetResultAsync(object validated, RuleContext context, CancellationToken token = default)
        {
            if(Type is null) throw new InvalidOperationException(String.Format(ExceptionMessages.GetExceptionMessage("DerivesFrom")));
            if(validated is null) return PassAsync();
            return Type.IsAssignableFrom(validated.GetType()) ? PassAsync() : FailAsync();
        }
    }
}