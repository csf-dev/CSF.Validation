using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Manifest;
using CSF.Validation.Rules;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.ValidatorValidation
{
    /// <summary>
    /// A rule which asserts that - where the <see cref="ManifestItem.CollectionItemValue"/> is not <see langword="null" />,
    /// it is configured to validate a type (via its own <see cref="ManifestItem.ValidatedType"/> property) that is compatible with
    /// the generic type for which the current <see cref="ManifestItem.ValidatedType"/> implements <see cref="IEnumerable{T}"/>.
    /// </summary>
    /// <example>
    /// <para>
    /// For example, if (from the validated manifest item) <c>item.CollectionItemValue.ValidatedType</c> is equal to <c>typeof(Pet)</c>
    /// and also <c>item.ValidatedType</c> derives from either <c>IEnumerable&lt;Cat&gt;</c> or <c>IEnumerable&lt;Pet&gt;</c> (where
    /// Cat is a subclass of Pet) then this would pass validation.
    /// </para>
    /// <para>
    /// On the other hand, if <c>item.CollectionItemValue.ValidatedType</c> is <c>typeof(Cat)</c>
    /// but <c>item.ValidatedType</c> derives from only <c>IEnumerable&lt;Pet&gt;</c> then this rule would fail validation.
    /// That's because if the collection may contain any "Pet", then it can't be validated by a <see cref="ManifestCollectionItem"/>
    /// which is only capable of validating "Cat" instances.
    /// </para>
    /// </example>
    public class CollectionItemValueMustValidateCompatibleTypeForValidatedType : IRule<ManifestItem>
    {
        /// <inheritdoc/>
        public Task<RuleResult> GetResultAsync(ManifestItem validated, RuleContext context, CancellationToken token = default)
        {
            if(validated?.CollectionItemValue?.ValidatedType is null || validated?.ValidatedType is null) return PassAsync();
            var requiredEnumerableType = typeof(IEnumerable<>).MakeGenericType(validated.CollectionItemValue.ValidatedType);
            return requiredEnumerableType.IsAssignableFrom(validated.ValidatedType) ? PassAsync() : FailAsync();
        }

        /// <inheritdoc/>
        public Task<string> GetFailureMessageAsync(ManifestItem value, ValidationRuleResult result, CancellationToken token = default)
        {
            var message = String.Format(Resources.FailureMessages.GetFailureMessage("CollectionItemValueMustValidateCompatibleTypeForValidatedType"),
                                        nameof(ManifestItem),
                                        nameof(ManifestItem.ValidatedType),
                                        nameof(IEnumerable<object>),
                                        nameof(ManifestItem.CollectionItemValue),
                                        value.ValidatedType,
                                        value.CollectionItemValue.ValidatedType);
            return Task.FromResult(message);
        }
    }
}