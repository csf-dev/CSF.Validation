using System;
using System.Collections.Generic;
using System.Linq;
using CSF.Validation.Manifest;
using CSF.Validation.Rules;

namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// A builder which is used to configure how a member or other value of an object should be validated.
    /// </summary>
    /// <typeparam name="TValidated">The type of the overall object being validated.</typeparam>
    /// <typeparam name="TValue">The type of this specific value being validated.</typeparam>
    public class ValueAccessorBuilder<TValidated, TValue> : IBuildsValueAccessor<TValidated, TValue>
    {
        readonly ValidatorBuilderContext context;
        readonly IGetsRuleBuilder ruleBuilderFactory;
        readonly IGetsValidatorManifest validatorManifestFactory;
        readonly ICollection<IGetsManifestValue> ruleBuilders = new HashSet<IGetsManifestValue>();
        ValueAccessExceptionBehaviour? accessExceptionBehaviour;

        /// <inheritdoc/>
        public IConfiguresValueAccessor<TValidated, TValue> AddRuleWithParent<TRule>(Action<IConfiguresRule<TRule>> ruleDefinition = null) where TRule : IRule<TValue, TValidated>
            => AddRulePrivate<TRule>(ruleDefinition);

        /// <inheritdoc/>
        public IConfiguresValueAccessor<TValidated, TValue> AddRule<TRule>(Action<IConfiguresRule<TRule>> ruleDefinition = default) where TRule : IRule<TValue>
            => AddRulePrivate<TRule>(ruleDefinition);

        IConfiguresValueAccessor<TValidated, TValue> AddRulePrivate<TRule>(Action<IConfiguresRule<TRule>> ruleDefinition)
        {
            var ruleBuilder = ruleBuilderFactory.GetRuleBuilder(context, ruleDefinition);
            ruleBuilders.Add(ruleBuilder);
            return this;
        }

        /// <inheritdoc/>
        public IConfiguresValueAccessor<TValidated, TValue> AddRules<TBuilder>() where TBuilder : IBuildsValidator<TValue>
        {
            var importedRules = validatorManifestFactory.GetValidatorManifest(typeof(TBuilder), context);
            ruleBuilders.Add(importedRules);
            return this;
        }

        /// <inheritdoc/>
        public IConfiguresValueAccessor<TValidated, TValue> AccessorExceptionBehaviour(ValueAccessExceptionBehaviour? behaviour)
        {
            accessExceptionBehaviour = behaviour;
            return this;
        }

        /// <inheritdoc/>
        public ManifestValueBase GetManifestValue()
        {
            var manifestValues = ruleBuilders.Select(x => x.GetManifestValue()).ToList();
            
            foreach(var manifestValue in manifestValues)
            {
                if(manifestValue == context.ManifestValue) continue;
                if(!(manifestValue is ManifestValue val)) continue;
                context.ManifestValue.Children.Add(val);
            }

            if(accessExceptionBehaviour.HasValue && context.ManifestValue is ManifestValue value)
                value.AccessorExceptionBehaviour = accessExceptionBehaviour.Value;

            return context.ManifestValue;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ValueAccessorBuilder{TValidated, TValue}"/>.
        /// </summary>
        /// <param name="context">The context which should be used for newly-added rule-builders.</param>
        /// <param name="ruleBuilderFactory">A factory for rule-builder instances.</param>
        /// <param name="validatorManifestFactory">A factory for validator manifest instances.</param>
        public ValueAccessorBuilder(ValidatorBuilderContext context, IGetsRuleBuilder ruleBuilderFactory, IGetsValidatorManifest validatorManifestFactory)
        {
            this.validatorManifestFactory = validatorManifestFactory ?? throw new ArgumentNullException(nameof(validatorManifestFactory));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.ruleBuilderFactory = ruleBuilderFactory ?? throw new ArgumentNullException(nameof(ruleBuilderFactory));
        }
    }
}