using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CSF.Validation.Manifest;
using CSF.Validation.Rules;

namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// A builder which is used to configure how an object should be validated.
    /// </summary>
    /// <typeparam name="TValidated">The type of the object being validated.</typeparam>
    public class ValidatorBuilder<TValidated> : IValidatorBuilder<TValidated>
    {
        readonly ValidatorBuilderContext context;
        readonly IGetsValidatorBuilderContext ruleContextFactory;
        readonly IGetsRuleBuilder ruleBuilderFactory;
        readonly IGetsValueAccessorBuilder valueBuilderFactory;
        readonly IGetsValidatorManifest validatorManifestFactory;
        readonly ICollection<IGetsManifestValue> ruleBuilders = new HashSet<IGetsManifestValue>();

        /// <inheritdoc/>
        public IConfiguresValidator<TValidated> UseObjectIdentity(Func<TValidated, object> identityAccessor)
        {
            context.ManifestValue.IdentityAccessor = o => identityAccessor((TValidated) o);
            return this;
        }

        /// <inheritdoc/>
        public IConfiguresValidator<TValidated> AddRule<TRule>(Action<IConfiguresRule<TRule>> ruleDefinition = default) where TRule : IRule<TValidated>
        {
            var ruleBuilder = ruleBuilderFactory.GetRuleBuilder(context, ruleDefinition);
            ruleBuilders.Add(ruleBuilder);
            return this;
        }

        /// <inheritdoc/>
        public IConfiguresValidator<TValidated> AddRules<TBuilder>() where TBuilder : IBuildsValidator<TValidated>
        {
            var importedRules = validatorManifestFactory.GetValidatorManifest(typeof(TBuilder), context);
            ruleBuilders.Add(importedRules);
            return this;
        }

        /// <inheritdoc/>
        public IConfiguresValidator<TValidated> ForMember<TValue>(Expression<Func<TValidated, TValue>> memberAccessor, Action<IConfiguresValueAccessor<TValidated, TValue>> valueConfig)
        {
            var ruleContext = ruleContextFactory.GetContextForMember(memberAccessor, context);
            AddValueValidation(valueConfig, ruleContext);
            return this;
        }

        /// <inheritdoc/>
        public IConfiguresValidator<TValidated> ForMemberItems<TValue>(Expression<Func<TValidated, IEnumerable<TValue>>> memberAccessor, Action<IConfiguresValueAccessor<TValidated, TValue>> valueConfig)
        {
            var memberContext = ruleContextFactory.GetContextForMember<TValidated,IEnumerable<TValue>>(memberAccessor, context);
            var ruleContext = ruleContextFactory.GetContextForMember<TValidated, TValue>(null, memberContext, true);
            AddValueValidation(valueConfig, ruleContext);
            return this;
        }

        /// <inheritdoc/>
        public IConfiguresValidator<TValidated> ForValue<TValue>(Func<TValidated, TValue> valueAccessor, Action<IConfiguresValueAccessor<TValidated, TValue>> valueConfig)
        {
            var ruleContext = ruleContextFactory.GetContextForValue(valueAccessor, context);
            AddValueValidation(valueConfig, ruleContext);
            return this;
        }

        /// <inheritdoc/>
        public IConfiguresValidator<TValidated> ForValues<TValue>(Func<TValidated, IEnumerable<TValue>> valuesAccessor, Action<IConfiguresValueAccessor<TValidated, TValue>> valueConfig)
        {
            var valueContext = ruleContextFactory.GetContextForValue<TValidated,IEnumerable<TValue>>(valuesAccessor, context);
            var ruleContext = ruleContextFactory.GetContextForValue<TValidated, TValue>(null, valueContext, true);
            AddValueValidation(valueConfig, ruleContext);
            return this;
        }
        
        /// <inheritdoc/>
        public IConfiguresValidator<TValidated> WhenValueIs<TDerived>(Action<IConfiguresValidator<TDerived>> derivedConfig)
            where TDerived : TValidated
        {
            var derivedContext = ruleContextFactory.GetPolymorphicContext(context, typeof(TDerived));
            var derivedBuilder = new ValidatorBuilder<TDerived>(ruleContextFactory,
                                                                ruleBuilderFactory,
                                                                valueBuilderFactory,
                                                                validatorManifestFactory,
                                                                derivedContext);
            if(!(derivedConfig is null))
                derivedConfig(derivedBuilder);
            ruleBuilders.Add(derivedBuilder);

            return this;
        }

        /// <inheritdoc/>
        public ManifestValueBase GetManifestValue()
        {
            var manifestValues = ruleBuilders.Select(x => x.GetManifestValue()).ToList();
            
            foreach(var manifestItem in manifestValues)
                HandleManifestItem(manifestItem);

            return context.ManifestValue;
        }

        void HandleManifestItem(ManifestValueBase manifestItem)
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

        /// <inheritdoc/>
        public ValidationManifest GetManifest()
        {
            return new ValidationManifest
            {
                RootValue = GetManifestValue() as ManifestValue,
                ValidatedType = typeof(TValidated),
            };
        }

        void AddValueValidation<TValue>(Action<IConfiguresValueAccessor<TValidated, TValue>> valueConfig, ValidatorBuilderContext context)
        {
            var valueBuilder = valueBuilderFactory.GetValueAccessorBuilder<TValidated, TValue>(context, valueConfig);
            ruleBuilders.Add(valueBuilder);
        }

        /// <summary>
        /// Initialises a new instance of <see cref="ValidatorBuilder{TValidated}"/>.
        /// </summary>
        /// <param name="ruleContextFactory">A factory for rule contexts.</param>
        /// <param name="ruleBuilderFactory">A factory for rule builders.</param>
        /// <param name="valueBuilderFactory">A factory for validator builders.</param>
        /// <param name="validatorManifestFactory">A factory for validator manifests.</param>
        /// <param name="context">
        /// A validator builder context for this validator builder to use.
        /// </param>
        public ValidatorBuilder(IGetsValidatorBuilderContext ruleContextFactory,
                                IGetsRuleBuilder ruleBuilderFactory,
                                IGetsValueAccessorBuilder valueBuilderFactory,
                                IGetsValidatorManifest validatorManifestFactory,
                                ValidatorBuilderContext context)
        {
            this.ruleContextFactory = ruleContextFactory ?? throw new ArgumentNullException(nameof(ruleContextFactory));
            this.ruleBuilderFactory = ruleBuilderFactory ?? throw new ArgumentNullException(nameof(ruleBuilderFactory));
            this.valueBuilderFactory = valueBuilderFactory ?? throw new ArgumentNullException(nameof(valueBuilderFactory));
            this.validatorManifestFactory = validatorManifestFactory ?? throw new ArgumentNullException(nameof(validatorManifestFactory));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }
    }
}