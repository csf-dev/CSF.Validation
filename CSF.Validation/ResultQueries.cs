using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CSF.Specifications;

namespace CSF.Validation
{
    internal static class ResultQueries
    {
        internal static IQueryableValidationResult<TItem> ForMember<TValidated,TItem>(IQueryableValidationResult<TValidated> results, Expression<Func<TValidated, TItem>> memberExpression)
        {
            if (results is null)
                throw new ArgumentNullException(nameof(results));

            var member = Reflection.Reflect.Member(memberExpression);
            var childValue = results.ManifestValue.Children.FirstOrDefault(x => x.MemberName == member.Name);

            if(childValue is null)
            {
                var message = String.Format(Resources.ExceptionMessages.GetExceptionMessage("NoMatchingMemberInManifest"),
                                            typeof(TValidated).FullName,
                                            member.Name);
                throw new ArgumentException(message, nameof(memberExpression));
            }

            var filteredRuleResults = results.RuleResults.Where(new RuleResultIsForDescendentOfValue(childValue));
            return new SubsetOfValidationResults<TItem>(filteredRuleResults, childValue);
        }

        internal static IQueryableValidationResult<TItem> ForMatchingMemberItem<TValidated,TItem>(IQueryableValidationResult<TValidated> results,
                                                                                                  Expression<Func<TValidated, IEnumerable<TItem>>> memberExpression,
                                                                                                  TItem item)
        {
            if (results is null)
                throw new ArgumentNullException(nameof(results));

            var member = Reflection.Reflect.Member(memberExpression);
            var collectionValue = results.ManifestValue.Children.FirstOrDefault(x => x.MemberName == member.Name);

            if(collectionValue is null)
            {
                var message = String.Format(Resources.ExceptionMessages.GetExceptionMessage("NoMatchingMemberInManifest"),
                                            typeof(TValidated).FullName,
                                            member.Name);
                throw new ArgumentException(message, nameof(memberExpression));
            }
            if(collectionValue.CollectionItemValue is null)
            {
                var message = string.Format(Resources.ExceptionMessages.GetExceptionMessage("ValueMustBeACollection"),
                                            typeof(TValidated).FullName,
                                            member.Name,
                                            nameof(ForMatchingMemberItem));
                throw new ArgumentException(message, nameof(memberExpression));
            }

            var filteredRuleResults = results.RuleResults.Where(new RuleResultIsForDescendentOfValue(collectionValue.CollectionItemValue, item));
            return new SubsetOfValidationResults<TItem>(filteredRuleResults, collectionValue);
        }

        internal static IQueryableValidationResult<TValidated> ForOnlyTheSameValue<TValidated>(IQueryableValidationResult<TValidated> results)
        {
            if (results is null)
                throw new ArgumentNullException(nameof(results));

            var filteredRuleResults = results.RuleResults.Where(new RuleResultIsForDescendentOfValue(results.ManifestValue, false));
            return new SubsetOfValidationResults<TValidated>(filteredRuleResults, results.ManifestValue);
        }

        internal static IQueryableValidationResult<TValidated> WithoutSuccesses<TValidated>(IQueryableValidationResult<TValidated> results)
        {
            if (results is null)
                throw new ArgumentNullException(nameof(results));

            return new SubsetOfValidationResults<TValidated>(results.Where(x => x.Outcome != Rules.RuleOutcome.Passed), results.ManifestValue);
        }
    }
}