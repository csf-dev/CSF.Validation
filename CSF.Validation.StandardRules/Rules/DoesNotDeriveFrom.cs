using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Resources;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A rule that passes if the validated value is <see langword="null" /> or if it does not derive from a specified type, and fails if it does.
    /// </summary>
    public class DoesNotDeriveFrom : IRuleWithMessage<object>
    {
        /// <summary>
        /// Gets or sets the type that the validated value must derive from.
        /// </summary>
        public Type Type { get; set; }

        /// <inheritdoc/>
        public Task<RuleResult> GetResultAsync(object validated, RuleContext context, CancellationToken token = default)
        {
            if(Type is null) throw new InvalidOperationException(String.Format(ExceptionMessages.GetExceptionMessage("DoesNotDeriveFrom")));
            if(validated is null) return PassAsync();
            var actualType = validated.GetType();
            var data = new Dictionary<string, object> { { DerivesFrom.ActualTypeKey, actualType } };
            return Type.IsAssignableFrom(actualType) ? FailAsync(data) : PassAsync(data);
        }

        /// <inheritdoc/>
        public Task<string> GetFailureMessageAsync(object value, ValidationRuleResult result, CancellationToken token = default)
            => Task.FromResult(GetFailureMessage(value, result, Type));

        static internal string GetFailureMessage(object value, ValidationRuleResult result, Type unwantedType)
        {
            var actualType = result.Data.TryGetValue(DerivesFrom.ActualTypeKey, out var typ) ? typ as Type : null;
            return String.Format(Resources.FailureMessages.GetFailureMessage("DoesNotDeriveFrom"),
                                 unwantedType,
                                 actualType?.ToString() ?? "<unknown>");
        }
    }
}