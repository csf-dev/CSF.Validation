using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CSF.Specifications;
using CSF.Validation.Manifest;

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
            return new SubsetOfValidationResults<TItem>(filteredRuleResults, collectionValue.CollectionItemValue);
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

        internal static IQueryableValidationResult<TDerived> PolymorphicAs<TValidated,TDerived>(IQueryableValidationResult<TValidated> results)
            where TDerived : TValidated
        {
            if (results is null)
                throw new ArgumentNullException(nameof(results));

            if(results.ManifestValue is ManifestPolymorphicType)
            {
                var message = String.Format(Resources.ExceptionMessages.GetExceptionMessage("MustNotBeAPolymorphicType"),
                                            typeof(ManifestPolymorphicType).Name);
                throw new ArgumentException(message, nameof(results));
            }

            var polymorphicManifest = results.ManifestValue.PolymorphicTypes.FirstOrDefault(x => x.ValidatedType == typeof(TDerived));
            if(polymorphicManifest is null)
            {
                var message = String.Format(Resources.ExceptionMessages.GetExceptionMessage("MustHaveMatchingPolymorphicManifest"),
                                            results.ManifestValue.ValidatedType.FullName,
                                            typeof(TDerived).FullName);
                throw new ArgumentException(message, nameof(results));
            }

            return new SubsetOfValidationResults<TDerived>(results.RuleResults, polymorphicManifest);
        }

        internal static SerializableValidationResult ToSerializableValidationResult(IQueryableValidationResult result)
        {
            return new SerializableValidationResult
            {
                RuleResults = result.RuleResults
                    .Select(ruleResult =>
                    {
                        return new SerializableValidationRuleResult
                        {
                            MemberName = ruleResult.Identifier?.MemberName,
                            ObjectIdentityString = ruleResult.Identifier?.ObjectIdentity?.ToString(),
                            RuleTypeName = ruleResult.Identifier?.RuleType.AssemblyQualifiedName,
                            RuleName = ruleResult.Identifier?.RuleName,
                            ValidatedTypeName = ruleResult.Identifier?.ValidatedType.AssemblyQualifiedName,
                            Outcome = ruleResult.Outcome,
                            Message = ruleResult.Message,
                            ExceptionString = ruleResult.Exception?.ToString(),
                        };
                    })
                    .ToArray(),
            };
        }
    }
}