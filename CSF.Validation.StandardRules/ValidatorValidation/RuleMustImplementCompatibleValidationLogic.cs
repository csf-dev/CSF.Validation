using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Manifest;
using CSF.Validation.Rules;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.ValidatorValidation
{
    /// <summary>
    /// A rule which asserts that the rule's concrete logic type, <see cref="RuleIdentifierBase.RuleType"/>, implements
    /// an interface which is compatible with the corresponding manifest item's <see cref="ManifestItem.ValidatedType"/>.
    /// </summary>
    public class RuleMustImplementCompatibleValidationLogic : IRuleWithMessage<ManifestRule>
    {
        /// <inheritdoc/>
        public Task<RuleResult> GetResultAsync(ManifestRule validated, RuleContext context, CancellationToken token = default)
        {
            var candidates = GetCandidateInterfaces(validated);
            if(!candidates.Any()) return PassAsync();

            return candidates.Any(x => x.IsAssignableFrom(validated.Identifier.RuleType)) ? PassAsync() : FailAsync();
        }

        static Type[] GetCandidateInterfaces(ManifestRule rule)
        {
            if(rule?.ManifestValue?.ValidatedType is null) return Type.EmptyTypes;

            var singleGenericInterface = typeof(IRule<>).MakeGenericType(rule.ManifestValue.ValidatedType);
            if(rule.ManifestValue.Parent?.ValidatedType is null) return new[] { singleGenericInterface };

            var doubleGenericInterface = typeof(IRule<,>).MakeGenericType(rule.ManifestValue.Parent.ValidatedType);
            return new[] { singleGenericInterface, doubleGenericInterface };
        }

        /// <inheritdoc/>
        public Task<string> GetFailureMessageAsync(ManifestRule value, ValidationRuleResult result, CancellationToken token = default)
            => Task.FromResult(GetFailureMessage(GetCandidateInterfaces(value), value.Identifier.RuleType));

        static string GetFailureMessage(Type[] candidateInterfaces, Type logicType)
        {
            switch(candidateInterfaces.Length)
            {
            case 1:
                return String.Format(Resources.FailureMessages.GetFailureMessage("RuleMustImplementCompatibleValidationLogicOneInterface"),
                                     nameof(ManifestRule.Identifier),
                                     nameof(RuleIdentifierBase.RuleType),
                                     nameof(IRule<object>),
                                     candidateInterfaces[0].GenericTypeArguments.First(),
                                     logicType);
            case 2:
                return String.Format(Resources.FailureMessages.GetFailureMessage("RuleMustImplementCompatibleValidationLogicTwoInterfaces"),
                                     nameof(ManifestRule.Identifier),
                                     nameof(RuleIdentifierBase.RuleType),
                                     nameof(IRule<object>),
                                     candidateInterfaces[0].GenericTypeArguments[0],
                                     candidateInterfaces[1].GenericTypeArguments[1],
                                     logicType);
            default:
                throw new ArgumentException(Resources.ExceptionMessages.GetExceptionMessage("WrongNumberOfCandidateInterfaces"), nameof(candidateInterfaces));
            }
        }
    }
}