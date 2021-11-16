using System;
using System.Collections.Generic;
using System.Linq;
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
        /// <exception cref="ValidatorBuildingException">If the <see cref="ManifestRule.RuleConfiguration"/> action is not null and throws an exception.</exception>
        public IValidationLogic GetValidationLogic(ManifestRule ruleDefinition)
        {
            if (ruleDefinition is null)
                throw new ArgumentNullException(nameof(ruleDefinition));

            var ruleLogic = GetRuleLogic(ruleDefinition);
            return WrapRuleLogicWithAdapter(ruleDefinition, ruleLogic);
        }

        /// <summary>
        /// Gets the underlying rule logic.
        /// This is returned as <see cref="object"/> because we cannot be sure as to which rule interface is being used.
        /// </summary>
        /// <param name="ruleDefinition">The rule definition</param>
        /// <returns>The rule logic</returns>
        /// <seealso cref="IRule{TValue, TValidated}"/>
        /// <seealso cref="IRule{TValidated}"/>
        /// <exception cref="ValidatorBuildingException">If the <see cref="ManifestRule.RuleConfiguration"/> action is not null and throws an exception.</exception>
        object GetRuleLogic(ManifestRule ruleDefinition)
        {
            var rule = ruleResolver.ResolveRule(ruleDefinition.Identifier.RuleType);
            ConfigureRule(rule, ruleDefinition);
            return rule;
        }

        /// <summary>
        /// Where <see cref="ManifestRule.RuleConfiguration"/> is not null, this method applies that configuration
        /// to the <paramref name="rule"/> instance.
        /// </summary>
        /// <param name="rule">The rule object to be configured.</param>
        /// <param name="ruleDefinition">The rule definition.</param>
        /// <exception cref="ValidatorBuildingException">If the configuration action throws an exception.</exception>
        static void ConfigureRule(object rule, ManifestRule ruleDefinition)
        {
            if(ruleDefinition.RuleConfiguration is null) return;

            try
            {
                ruleDefinition.RuleConfiguration(rule);
            }
            catch(Exception e)
            {
                var message = String.Format(Resources.ExceptionMessages.GetExceptionMessage("UnexpectedExceptionConfiguringRule"), ruleDefinition.Identifier);
                throw new ValidatorBuildingException(message, e);
            }
        }

        /// <summary>
        /// Wraps the <paramref name="ruleLogic"/> object with an adapter that allows it to be used via
        /// the interface <see cref="IValidationLogic"/>.
        /// </summary>
        /// <param name="ruleDefinition">The manifest rule definition.</param>
        /// <param name="ruleLogic">The rule logic object.</param>
        /// <returns>The rule logic object wrapped with an adapter allowing it to be executed via the interface <see cref="IValidationLogic"/>.</returns>
        static IValidationLogic WrapRuleLogicWithAdapter(ManifestRule ruleDefinition, object ruleLogic)
        {
            var ruleInterface = GetBestRuleInterface(ruleDefinition);

            var ruleAdapter = ruleInterface.GetGenericTypeDefinition() == typeof(IRule<>)
                ? GetRuleAdapter(ruleInterface.GenericTypeArguments[0], ruleLogic)
                : GetValueRuleAdapter(ruleInterface.GenericTypeArguments[0], ruleInterface.GenericTypeArguments[1], ruleLogic);
            
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

        /// <summary>
        /// Selects the rule-logic interface with the best/closest match to the rule type and the way in which it is
        /// to be used by the validator.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If the validation rule has a parent validated type - the <seealso cref="ManifestValue.Parent"/> property
        /// of the <see cref="ManifestRule.ManifestValue"/> is not <see langword="null"/> - and the rule type implements
        /// <see cref="IRule{TValue, TValidated}"/> of the appropriate generic types the type of the value rule
        /// interface will be returned.
        /// </para>
        /// <para>
        /// A second attempt will be made if the rule has no parent validated type, or if it does not implement the <see cref="IRule{TValue, TValidated}"/>
        /// interface with appropriare generic types.  If it implements <see cref="IRule{TValidated}"/> with the
        /// appropriate generic type then the type of that interface will be returned.
        /// </para>
        /// <para>
        /// If neither mechanism above succeeds in getting a rule interface then an exception will be raised.
        /// </para>
        /// </remarks>
        /// <param name="ruleDefinition">The rule definition.</param>
        /// <returns>The interface-type which most closely matches the rule and its usage.</returns>
        /// <seealso cref="IRule{TValue, TValidated}"/>
        /// <seealso cref="IRule{TValidated}"/>
        /// <exception cref="ValidatorBuildingException">If the rule class does not implement either <see cref="IRule{TValue, TValidated}"/>
        /// or <see cref="IRule{TValidated}"/> with appropriate generic types.</exception>
        static Type GetBestRuleInterface(ManifestRule ruleDefinition)
        {
            var ruleType = ruleDefinition.Identifier.RuleType;
            var validatedType = GetValidatedType(ruleDefinition.ManifestValue);
            var parentValidatedType = GetValidatedType(ruleDefinition.ManifestValue.Parent);

            var valueRuleInterface = TryGetValueRuleInterface(ruleType, validatedType, parentValidatedType);
            if(valueRuleInterface != null) return valueRuleInterface;

            var ruleInterface = TryGetRuleInterface(ruleType, validatedType);
            if(ruleInterface != null) return ruleInterface;

            var messageTemplate = (parentValidatedType != null) ? "RuleTypeMustImplementAppropriateRuleOrValueRuleInterface" : "RuleTypeMustImplementAppropriateRuleInterface";
            var message = String.Format(GetExceptionMessage(messageTemplate), ruleType, validatedType, parentValidatedType, nameof(IRule<object>), nameof(IRule<object,object>));
            throw new ValidatorBuildingException(message);
        }

        static Type GetValidatedType(ManifestValue manifestValue)
        {
            if(manifestValue is null) return null;
            var type = manifestValue.ValidatedType;

            if(!manifestValue.EnumerateItems) return type;

            var itemType = (from @interface in type.GetTypeInfo().ImplementedInterfaces.Union(new [] { type })
                            where
                                @interface.GetTypeInfo().IsGenericType
                             && @interface.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                            select @interface.GenericTypeArguments[0])
                .FirstOrDefault();

            if(!(itemType is null)) return itemType;

            var message = String.Format(GetExceptionMessage("EnumerateItemsIsTrueButValueIsNotEnumerable"),
                                        nameof(ManifestValue.EnumerateItems),
                                        type,
                                        nameof(IEnumerable<object>));
            throw new ValidatorBuildingException(message);
        }

        static Type TryGetValueRuleInterface(Type ruleType, Type validatedType, Type parentValidatedType)
        {
            if(parentValidatedType == null) return null;

            var interfaceType = typeof(IRule<,>).MakeGenericType(validatedType, parentValidatedType);
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