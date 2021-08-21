using System;
using System.Reflection;
using CSF.Validation.Manifest;
using static CSF.Validation.Resources.ExceptionMessages;

namespace CSF.Validation.Rules
{
    /// <summary>
    /// A factory service which creates instances of <see cref="IValidationLogic"/>
    /// from <see cref="ManifestRule"/> definitions.
    /// </summary>
    public class ValidationLogicFactory : IGetsValidationLogic
    {
        readonly IResolvesRule ruleResolver;

        /// <summary>
        /// Gets a validation logic instance for the specified manifest rule definition.
        /// </summary>
        /// <param name="ruleDefinition">A manifest rule definition.</param>
        /// <returns>A validation logic instance by which the rule's logic may be executed in a generalised manner.</returns>
        public IValidationLogic GetValidationLogic(ManifestRule ruleDefinition)
        {
            if (ruleDefinition is null)
                throw new ArgumentNullException(nameof(ruleDefinition));

            var ruleInterface = GetBestRuleInterface(ruleDefinition);
            var rule = ruleResolver.ResolveRule(ruleDefinition.Identifier.RuleType);

            var ruleAdapter = ruleInterface.GetGenericTypeDefinition() == typeof(IRule<>)
                ? GetRuleAdapter(ruleInterface.GenericTypeArguments[0], rule)
                : GetValueRuleAdapter(ruleInterface.GenericTypeArguments[0], ruleInterface.GenericTypeArguments[1], rule);
            ruleAdapter = WrapWithExceptionHandlingDecorator(ruleAdapter);

            return ruleAdapter;
        }

        static IValidationLogic GetRuleAdapter(Type validatedType, object rule)
        {
            var adapterType = typeof(RuleAdapter<>).MakeGenericType(validatedType);
            return (IValidationLogic)Activator.CreateInstance(adapterType, new[] { rule });
        }

        static IValidationLogic GetValueRuleAdapter(Type valueType, Type validatedType, object rule)
        {
            var adapterType = typeof(ValueRuleAdapter<,>).MakeGenericType(valueType, validatedType);
            return (IValidationLogic)Activator.CreateInstance(adapterType, new[] { rule });
        }

        static IValidationLogic WrapWithExceptionHandlingDecorator(IValidationLogic logic)
            => new ExceptionHandlingRuleLogicDecorator(logic);

        static Type GetBestRuleInterface(ManifestRule ruleDefinition)
        {
            var ruleType = ruleDefinition.Identifier.RuleType;
            var validatedType = ruleDefinition.ManifestValue.ValidatedType;
            var parentValidatedType = ruleDefinition.ManifestValue.Parent?.ValidatedType;

            var valueRuleInterface = TryGetValueRuleInterface(ruleType, validatedType, parentValidatedType);
            if(valueRuleInterface != null) return valueRuleInterface;

            var ruleInterface = TryGetRuleInterface(ruleType, validatedType);
            if(ruleInterface != null) return ruleInterface;

            var messageTemplate = (parentValidatedType != null) ? "RuleTypeMustImplementAppropriateRuleOrValueRuleInterface" : "RuleTypeMustImplementAppropriateRuleInterface";
            var message = String.Format(GetExceptionMessage(messageTemplate), ruleType, validatedType, parentValidatedType);
            throw new ValidatorBuildingException(message);
        }

        static Type TryGetValueRuleInterface(Type ruleType, Type validatedType, Type parentValidatedType)
        {
            if(parentValidatedType == null) return null;

            var interfaceType = typeof(IValueRule<,>).MakeGenericType(validatedType, parentValidatedType);
            return interfaceType.GetTypeInfo().IsAssignableFrom(ruleType.GetTypeInfo()) ? interfaceType : null;
        }

        static Type TryGetRuleInterface(Type ruleType, Type validatedType)
        {
            var interfaceType = typeof(IRule<>).MakeGenericType(validatedType);
            return interfaceType.GetTypeInfo().IsAssignableFrom(ruleType.GetTypeInfo()) ? interfaceType : null;
        }
 
        /// <summary>
        /// Initialises a new instance of <see cref="ValidationLogicFactory"/>.
        /// </summary>
        /// <param name="ruleResolver">A rule-resolving service.</param>
        public ValidationLogicFactory(IResolvesRule ruleResolver)
        {
            this.ruleResolver = ruleResolver ?? throw new ArgumentNullException(nameof(ruleResolver));
        }
   }
}