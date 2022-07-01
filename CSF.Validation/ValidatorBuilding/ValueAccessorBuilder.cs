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
        readonly IGetsValidatorBuilderContext builderContextFactory;
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
        public IConfiguresValueAccessor<TValidated, TValue> WhenValueIs<TDerived>(Action<IConfiguresValueAccessor<TValidated,TDerived>> derivedConfig)
            where TDerived : TValue
        {
            var derivedContext = builderContextFactory.GetPolymorphicContext(context, typeof(TDerived));
            var derivedBuilder = new ValueAccessorBuilder<TValidated, TDerived>(derivedContext,
                                                                                ruleBuilderFactory,
                                                                                validatorManifestFactory,
                                                                                builderContextFactory);
            if(!(derivedConfig is null))
                derivedConfig(derivedBuilder);
            ruleBuilders.Add(derivedBuilder);

            return this;
        }

        /// <inheritdoc/>
        public IManifestItem GetManifestValue()
        {
            var manifestValues = ruleBuilders.Select(x => x.GetManifestValue()).ToList();
            
            foreach(var manifestItem in manifestValues)
                HandleManifestItem(manifestItem);

            if(accessExceptionBehaviour.HasValue && context.ManifestValue is ManifestValue value)
                value.AccessorExceptionBehaviour = accessExceptionBehaviour.Value;

            return context.ManifestValue;
        }

        void HandleManifestItem(IManifestItem manifestItem)
        {
            if(manifestItem == context.ManifestValue) return;

            if (manifestItem is ManifestValue value
             && !(context.ManifestValue.Children.Contains(manifestItem)))
            {
                context.ManifestValue.Children.Add(value);
            }

            if (manifestItem is ManifestPolymorphicType poly
             && context.ManifestValue is IHasPolymorphicTypes hasPoly
             && !(hasPoly.PolymorphicTypes.Contains(poly)))
            {
                hasPoly.PolymorphicTypes.Add(poly);
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ValueAccessorBuilder{TValidated, TValue}"/>.
        /// </summary>
        /// <param name="context">The context which should be used for newly-added rule-builders.</param>
        /// <param name="ruleBuilderFactory">A factory for rule-builder instances.</param>
        /// <param name="validatorManifestFactory">A factory for validator manifest instances.</param>
        /// <param name="builderContextFactory">A factory for validator builder contexts.</param>
        public ValueAccessorBuilder(ValidatorBuilderContext context,
                                    IGetsRuleBuilder ruleBuilderFactory,
                                    IGetsValidatorManifest validatorManifestFactory,
                                    IGetsValidatorBuilderContext builderContextFactory)
        {
            this.validatorManifestFactory = validatorManifestFactory ?? throw new ArgumentNullException(nameof(validatorManifestFactory));
            this.builderContextFactory = builderContextFactory ?? throw new ArgumentNullException(nameof(builderContextFactory));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.ruleBuilderFactory = ruleBuilderFactory ?? throw new ArgumentNullException(nameof(ruleBuilderFactory));
        }
    }
}