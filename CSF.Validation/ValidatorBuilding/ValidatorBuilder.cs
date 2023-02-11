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
    public class ValidatorBuilder<TValidated> : IConfiguresValidator<TValidated>, IHasValidationBuilderContext
    {
        readonly ValidatorBuilderContext context;
        readonly IGetsValidatorBuilderContext ruleContextFactory;
        readonly IGetsRuleBuilder ruleBuilderFactory;
        readonly IGetsValueAccessorBuilder valueBuilderFactory;
        readonly IGetsValidatorBuilderContextFromBuilder validatorManifestFactory;

        /// <inheritdoc/>
        public ValidatorBuilderContext Context => context;

        /// <inheritdoc/>
        public IConfiguresValidator<TValidated> UseObjectIdentity(Func<TValidated, object> identityAccessor)
        {
            Context.AssertNotRecursive();
            Context.ManifestValue.IdentityAccessor = o => identityAccessor((TValidated) o);
            return this;
        }

        /// <inheritdoc/>
        public IConfiguresValidator<TValidated> AddRule<TRule>(Action<IConfiguresRule<TRule>> ruleDefinition = default) where TRule : IRule<TValidated>
        {
            Context.AssertNotRecursive();
            var builder = ruleBuilderFactory.GetRuleBuilder(Context, ruleDefinition);
            context.ConfigurationCallbacks.Add(builder);
            return this;
        }

        /// <inheritdoc/>
        public IConfiguresValidator<TValidated> AddRules<TBuilder>() where TBuilder : IBuildsValidator<TValidated>
        {
            Context.AssertNotRecursive();
            var importContext = validatorManifestFactory.GetValidatorBuilderContext(typeof(TBuilder), Context);
            Context.Contexts.Add(importContext);
            return this;
        }

        /// <inheritdoc/>
        public IConfiguresValidator<TValidated> AddBaseRules<TBase, TBuilder>() where TBuilder : IBuildsValidator<TBase>
        {
            Context.AssertNotRecursive();
            if(!typeof(TBase).IsAssignableFrom(typeof(TValidated)))
                throw new InvalidCastException(String.Format(Resources.ExceptionMessages.GetExceptionMessage("ValidatedTypeMustDeriveFromBaseType"),
                                                             typeof(TValidated).FullName,
                                                             typeof(TBase).FullName,
                                                             nameof(AddBaseRules)));
            var importContext = validatorManifestFactory.GetValidatorBuilderContext(typeof(TBuilder), Context);
            Context.Contexts.Add(importContext);
            return this;
        }

        /// <inheritdoc/>
        public IConfiguresValidator<TValidated> ForMember<TValue>(Expression<Func<TValidated, TValue>> memberAccessor, Action<IConfiguresValueAccessor<TValidated, TValue>> valueConfig)
        {
            Context.AssertNotRecursive();
            var ruleContext = ruleContextFactory.GetContextForMember(memberAccessor, Context);
            valueBuilderFactory.GetValueAccessorBuilder<TValidated, TValue>(ruleContext, valueConfig);
            Context.Contexts.Add(ruleContext);
            return this;
        }

        /// <inheritdoc/>
        public IConfiguresValidator<TValidated> ForMemberItems<TValue>(Expression<Func<TValidated, IEnumerable<TValue>>> memberAccessor, Action<IConfiguresValueAccessor<TValidated, TValue>> valueConfig)
        {
            Context.AssertNotRecursive();
            var memberContext = ruleContextFactory.GetContextForMember<TValidated,IEnumerable<TValue>>(memberAccessor, Context);
            Context.Contexts.Add(memberContext);
            var ruleContext = ruleContextFactory.GetContextForCollection(memberContext, typeof(TValue));
            memberContext.Contexts.Add(ruleContext);
            valueBuilderFactory.GetValueAccessorBuilder<TValidated, TValue>(ruleContext, valueConfig);

            return this;
        }

        /// <inheritdoc/>
        public IConfiguresValidator<TValidated> ForValue<TValue>(Func<TValidated, TValue> valueAccessor, Action<IConfiguresValueAccessor<TValidated, TValue>> valueConfig)
        {
            Context.AssertNotRecursive();
            var ruleContext = ruleContextFactory.GetContextForValue(valueAccessor, Context);
            valueBuilderFactory.GetValueAccessorBuilder<TValidated, TValue>(ruleContext, valueConfig);
            Context.Contexts.Add(ruleContext);
            return this;
        }

        /// <inheritdoc/>
        public IConfiguresValidator<TValidated> ForValues<TValue>(Func<TValidated, IEnumerable<TValue>> valuesAccessor, Action<IConfiguresValueAccessor<TValidated, TValue>> valueConfig)
        {
            Context.AssertNotRecursive();
            var valueContext = ruleContextFactory.GetContextForValue<TValidated,IEnumerable<TValue>>(valuesAccessor, Context);
            Context.Contexts.Add(valueContext);
            var ruleContext = ruleContextFactory.GetContextForCollection(valueContext, typeof(TValue));
            valueContext.Contexts.Add(ruleContext);
            valueBuilderFactory.GetValueAccessorBuilder<TValidated, TValue>(ruleContext, valueConfig);

            return this;
        }
        
        /// <inheritdoc/>
        public IConfiguresValidator<TValidated> WhenValueIs<TDerived>(Action<IConfiguresValidator<TDerived>> derivedConfig)
            where TDerived : TValidated
        {
            Context.AssertNotRecursive();
            var polymorphicContext = ruleContextFactory.GetPolymorphicContext(Context, typeof(TDerived));
            Context.Contexts.Add(polymorphicContext);

            if(!(derivedConfig is null))
            {
                var polymorphicBuilder = new ValidatorBuilder<TDerived>(ruleContextFactory,
                                                                        ruleBuilderFactory,
                                                                        valueBuilderFactory,
                                                                        validatorManifestFactory,
                                                                        polymorphicContext);
                derivedConfig(polymorphicBuilder);
            }

            return this;
        }

        /// <inheritdoc/>
        public IConfiguresValidator<TValidated> ValidateAsAncestor(int depth)
        {
            if(depth < 1)
                throw new ArgumentOutOfRangeException(nameof(depth));
            var recursiveAncestor = Context.ManifestValue.GetAncestor(depth);
            Context.MakeRecursive(recursiveAncestor);
            return this;
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
                                IGetsValidatorBuilderContextFromBuilder validatorManifestFactory,
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