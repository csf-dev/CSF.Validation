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
    public class ValueAccessorBuilder<TValidated, TValue> : IConfiguresValueAccessor<TValidated, TValue>, IHasValidationBuilderContext
    {
        readonly ValidatorBuilderContext context;
        readonly IGetsRuleBuilder ruleBuilderFactory;
        readonly IGetsValidatorBuilderContextFromBuilder validatorManifestFactory;
        readonly IGetsValidatorBuilderContext builderContextFactory;

        /// <inheritdoc/>
        public ValidatorBuilderContext Context => context;

        /// <inheritdoc/>
        public IConfiguresValueAccessor<TValidated, TValue> AddRuleWithParent<TRule>(Action<IConfiguresRule<TRule>> ruleDefinition = null) where TRule : IRule<TValue, TValidated>
            => AddRulePrivate<TRule>(ruleDefinition);

        /// <inheritdoc/>
        public IConfiguresValueAccessor<TValidated, TValue> AddRule<TRule>(Action<IConfiguresRule<TRule>> ruleDefinition = default) where TRule : IRule<TValue>
            => AddRulePrivate<TRule>(ruleDefinition);

        IConfiguresValueAccessor<TValidated, TValue> AddRulePrivate<TRule>(Action<IConfiguresRule<TRule>> ruleDefinition)
        {
            var builder = ruleBuilderFactory.GetRuleBuilder(context, ruleDefinition);
            context.ConfigurationCallbacks.Add(builder);
            return this;
        }

        /// <inheritdoc/>
        public IConfiguresValueAccessor<TValidated, TValue> AddRules<TBuilder>() where TBuilder : IBuildsValidator<TValue>
        {
            validatorManifestFactory.GetValidatorBuilderContext(typeof(TBuilder), context);
            return this;
        }

        /// <inheritdoc/>
        public IConfiguresValueAccessor<TValidated, TValue> AccessorExceptionBehaviour(ValueAccessExceptionBehaviour? behaviour)
        {
            context.ManifestValue.AccessorExceptionBehaviour = behaviour;
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
            Context.Contexts.Add(derivedContext);

            return this;
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
                                    IGetsValidatorBuilderContextFromBuilder validatorManifestFactory,
                                    IGetsValidatorBuilderContext builderContextFactory)
        {
            this.validatorManifestFactory = validatorManifestFactory ?? throw new ArgumentNullException(nameof(validatorManifestFactory));
            this.builderContextFactory = builderContextFactory ?? throw new ArgumentNullException(nameof(builderContextFactory));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.ruleBuilderFactory = ruleBuilderFactory ?? throw new ArgumentNullException(nameof(ruleBuilderFactory));
        }
    }
}