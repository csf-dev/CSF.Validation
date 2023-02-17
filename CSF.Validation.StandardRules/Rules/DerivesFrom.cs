using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Resources;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A rule that passes if the validated value is <see langword="null" /> or if it derives from a specified type, and fails if it does not.
    /// </summary>
    [Parallelizable]
    public class DerivesFrom : IRuleWithMessage<object>
    {
        internal const string ActualTypeKey = "Actual type";

        /// <summary>
        /// Gets or sets the type that the validated value must derive from.
        /// </summary>
        public Type Type { get; set; }

        /// <inheritdoc/>
        public ValueTask<RuleResult> GetResultAsync(object validated, RuleContext context, CancellationToken token = default)
        {
            if(Type is null) throw new InvalidOperationException(String.Format(ExceptionMessages.GetExceptionMessage("DerivesFrom")));
            if(validated is null) return PassAsync();
            var actualType = validated.GetType();
            var data = new Dictionary<string, object> { { ActualTypeKey, actualType } };
            return Type.IsAssignableFrom(actualType) ? PassAsync(data) : FailAsync(data);
        }

        /// <inheritdoc/>
        public ValueTask<string> GetFailureMessageAsync(object value, ValidationRuleResult result, CancellationToken token = default)
            => new ValueTask<string>(GetFailureMessage(value, result, Type));

        static internal string GetFailureMessage(object value, ValidationRuleResult result, Type requiredType)
        {
            var actualType = result.Data.TryGetValue(ActualTypeKey, out var typ) ? typ as Type : null;
            return String.Format(Resources.FailureMessages.GetFailureMessage("DerivesFrom"),
                                 requiredType,
                                 actualType?.ToString() ?? "<unknown>");
        }
    }
}