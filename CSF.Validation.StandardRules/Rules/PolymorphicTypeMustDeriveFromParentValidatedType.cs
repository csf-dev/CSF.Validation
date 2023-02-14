using System;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Manifest;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// Validation rule which asserts that for any manifest item which <see cref="ManifestItem.IsPolymorphicType"/>,
    /// the <see cref="ManifestItem.ValidatedType"/> of the polymorphic type 
    /// </summary>
    [Parallelizable]
    public class PolymorphicTypeMustDeriveFromParentValidatedType : IRuleWithMessage<Type, ManifestItem>
    {
        /// <inheritdoc/>
        public Task<string> GetFailureMessageAsync(Type value, ManifestItem parentValue, ValidationRuleResult result, CancellationToken token = default)
        {
            var message = String.Format(Resources.FailureMessages.GetFailureMessage("PolymorphicTypeMustDeriveFromParentValidatedType"),
                                        nameof(ManifestItemTypes.PolymorphicType),
                                        nameof(ManifestItem.ValidatedType),
                                        nameof(ManifestItem.Parent),
                                        value,
                                        parentValue?.Parent?.ValidatedType);
            return Task.FromResult(message);
        }

        /// <inheritdoc/>
        public Task<RuleResult> GetResultAsync(Type value, ManifestItem parentValue, RuleContext context, CancellationToken token = default)
        {
            if (value is null
             || parentValue is null
             || !parentValue.IsPolymorphicType
             || parentValue.Parent?.ValidatedType is null) return PassAsync();
            
            return parentValue.Parent.ValidatedType.IsAssignableFrom(value) ? PassAsync() : FailAsync();
        }
    }
}