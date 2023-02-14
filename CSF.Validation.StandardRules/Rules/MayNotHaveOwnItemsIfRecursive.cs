using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Manifest;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A validation rule which asserts that - if the manifest item type is recursive - then the parent manifest item may not
    /// have any of its "own" collection item, children, polymorphic types or rules.
    /// </summary>
    [Parallelizable]
    public class MayNotHaveOwnItemsIfRecursive : IRuleWithMessage<ManifestItem>
    {
        const string
            ownCollectionItemKey = nameof(ManifestItem.OwnCollectionItemValue),
            ownChildrenKey = nameof(ManifestItem.OwnChildren),
            ownPolymorphicTypesKey = nameof(ManifestItem.OwnPolymorphicTypes),
            ownRulesKey = nameof(ManifestItem.OwnRules);

        /// <inheritdoc/>
        public Task<string> GetFailureMessageAsync(ManifestItem value, ValidationRuleResult result, CancellationToken token = default)
        {
            var message = String.Format(Resources.FailureMessages.GetFailureMessage("MayNotHaveOwnItemsIfRecursive"),
                                        nameof(ManifestItemTypes.Recursive),
                                        nameof(ManifestItem.OwnCollectionItemValue),
                                        result.Data.TryGetValue(ownCollectionItemKey, out object val1) && val1 is bool boolVal ? boolVal.ToString() : "<unknown>",
                                        nameof(ManifestItem.OwnChildren),
                                        result.Data.TryGetValue(ownChildrenKey, out object val2) && val2 is int intVal1 ? intVal1.ToString() : "<unknown>",
                                        nameof(ManifestItem.OwnPolymorphicTypes),
                                        result.Data.TryGetValue(ownPolymorphicTypesKey, out object val3) && val3 is int intVal2 ? intVal2.ToString() : "<unknown>",
                                        nameof(ManifestItem.OwnRules),
                                        result.Data.TryGetValue(ownRulesKey, out object val4) && val4 is int intVal3 ? intVal3.ToString() : "<unknown>");
            return Task.FromResult(message);
        }

        /// <inheritdoc/>
        public Task<RuleResult> GetResultAsync(ManifestItem validated, RuleContext context, CancellationToken token = default)
        {
            if(validated is null || !validated.IsRecursive) return PassAsync();

            var hasOwnCollectionItem = validated.OwnCollectionItemValue != null;
            var ownChildrenCount = validated.OwnChildren.Count;
            var ownPolymorphicTypesCount = validated.OwnPolymorphicTypes.Count;
            var ownRulesCount = validated.OwnRules.Count;

            var data = new Dictionary<string, object> {
                {ownCollectionItemKey, hasOwnCollectionItem},
                {ownChildrenKey, ownChildrenCount},
                {ownPolymorphicTypesKey, ownPolymorphicTypesCount},
                {ownRulesKey, ownRulesCount},
            };

            var ok = !hasOwnCollectionItem
                  && ownChildrenCount == 0
                  && ownPolymorphicTypesCount == 0
                  && ownRulesCount == 0;
            
            return ok ? PassAsync(data) : FailAsync(data);
        }
    }
}