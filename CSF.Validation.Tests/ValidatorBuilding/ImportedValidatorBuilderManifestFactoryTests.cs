using System;
using AutoFixture.NUnit3;
using CSF.Validation.Bootstrap;
using CSF.Validation.Manifest;
using CSF.Validation.Stubs;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.ValidatorBuilding
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class ImportedValidatorBuilderManifestFactoryTests
    {
        [Test,AutoMoqData]
        public void GetValidatorManifestShouldReturnAValidatorBuilder([Frozen] IServiceProvider serviceProvider,
                                                                      ImportedValidatorBuilderManifestFactory sut,
                                                                      IGetsValidatorBuilderContext ruleContextFactory,
                                                                      IGetsRuleBuilder ruleBuilderFactory,
                                                                      IGetsValueAccessorBuilder valueBuilderFactory,
                                                                      IGetsValidatorBuilderContextFromBuilder validatorManifestFactory,
                                                                      IResolvesServices resolver,
                                                                      [ManifestModel] ValidatorBuilderContext context)
        {
            Mock.Get(serviceProvider).Setup(x => x.GetService(typeof(IGetsValidatorBuilderContext))).Returns(ruleContextFactory);
            Mock.Get(serviceProvider).Setup(x => x.GetService(typeof(IGetsRuleBuilder))).Returns(ruleBuilderFactory);
            Mock.Get(serviceProvider).Setup(x => x.GetService(typeof(IGetsValueAccessorBuilder))).Returns(valueBuilderFactory);
            Mock.Get(serviceProvider).Setup(x => x.GetService(typeof(IGetsValidatorBuilderContextFromBuilder))).Returns(validatorManifestFactory);
            Mock.Get(serviceProvider).Setup(x => x.GetService(typeof(IResolvesServices))).Returns(resolver);
            Mock.Get(resolver).Setup(x => x.ResolveService<object>(typeof(GenericValidatorDefinition<object>))).Returns(() => new GenericValidatorDefinition<object>());

            var result = sut.GetValidatorBuilderContext(typeof(GenericValidatorDefinition<object>), context);

            Assert.That(result, Is.InstanceOf<ValidatorBuilderContext>());
        }

        [Test,AutoMoqData]
        public void GetValidatorManifestShouldExecuteConfigureValidatorFromDefinitionUponBuilder([Frozen] IServiceProvider serviceProvider,
                                                                                                 ImportedValidatorBuilderManifestFactory sut,
                                                                                                 IGetsValidatorBuilderContext ruleContextFactory,
                                                                                                 IGetsRuleBuilder ruleBuilderFactory,
                                                                                                 IGetsValueAccessorBuilder valueBuilderFactory,
                                                                                                 IGetsValidatorBuilderContextFromBuilder validatorManifestFactory,
                                                                                                 IResolvesServices resolver,
                                                                                                 [ManifestModel] ValidatorBuilderContext context,
                                                                                                 ValidatedObject obj)
        {
            Mock.Get(serviceProvider).Setup(x => x.GetService(typeof(IGetsValidatorBuilderContext))).Returns(ruleContextFactory);
            Mock.Get(serviceProvider).Setup(x => x.GetService(typeof(IGetsRuleBuilder))).Returns(ruleBuilderFactory);
            Mock.Get(serviceProvider).Setup(x => x.GetService(typeof(IGetsValueAccessorBuilder))).Returns(valueBuilderFactory);
            Mock.Get(serviceProvider).Setup(x => x.GetService(typeof(IGetsValidatorBuilderContextFromBuilder))).Returns(validatorManifestFactory);
            Mock.Get(serviceProvider).Setup(x => x.GetService(typeof(IResolvesServices))).Returns(resolver);
            Mock.Get(resolver).Setup(x => x.ResolveService<object>(typeof(ValidatedObjectDefinition))).Returns(() => new ValidatedObjectDefinition());

            sut.GetValidatorBuilderContext(typeof(ValidatedObjectDefinition), context);

            Assert.That(() => context.ManifestValue.IdentityAccessor(obj),
                        Is.EqualTo(obj.Identity),
                        $@"Because the definition sets up the context to use the {nameof(ValidatedObject.Identity)} property
for identity, asserting that they are equal proves that the configuration function was executed.");
        }

        [Test,AutoMoqData]
        public void GetValidatorManifestShouldThrowForAnInvalidDefinitionWithNoInterface(ImportedValidatorBuilderManifestFactory sut,
                                                                                         [ManifestModel] ValidatorBuilderContext context)
        {
            Assert.That(() => sut.GetValidatorBuilderContext(typeof(InvalidDefinitionWithNoInterface), context),
                        Throws.ArgumentException.And.Message.StartWith("The validation definition type must implement IBuildsValidator<T>."));
        }

        [Test,AutoMoqData]
        public void GetValidatorManifestShouldThrowForAnInvalidDefinitionWithTwoInterfaces(ImportedValidatorBuilderManifestFactory sut, [ManifestModel] ValidatorBuilderContext context)
        {
            Assert.That(() => sut.GetValidatorBuilderContext(typeof(InvalidDefinitionWithTwoInterfaces), context),
                        Throws.ArgumentException.And.Message.StartWith("The validation definition type must implement IBuildsValidator<T> a maximum of once, for one generic validated type."));
        }
        [Test,AutoMoqData]
        public void GetValidatorManifestWithRuleContextShouldReturnABuilderContext([Frozen] IServiceProvider serviceProvider,
                                                                                     ImportedValidatorBuilderManifestFactory sut,
                                                                                     IGetsValidatorBuilderContext ruleContextFactory,
                                                                                     IGetsRuleBuilder ruleBuilderFactory,
                                                                                     IGetsValueAccessorBuilder valueBuilderFactory,
                                                                                     IGetsValidatorBuilderContextFromBuilder validatorManifestFactory,
                                                                                     IResolvesServices resolver,
                                                                                     [ManifestModel] ValidatorBuilderContext context)
        {
            Mock.Get(serviceProvider).Setup(x => x.GetService(typeof(IGetsValidatorBuilderContext))).Returns(ruleContextFactory);
            Mock.Get(serviceProvider).Setup(x => x.GetService(typeof(IGetsRuleBuilder))).Returns(ruleBuilderFactory);
            Mock.Get(serviceProvider).Setup(x => x.GetService(typeof(IGetsValueAccessorBuilder))).Returns(valueBuilderFactory);
            Mock.Get(serviceProvider).Setup(x => x.GetService(typeof(IGetsValidatorBuilderContextFromBuilder))).Returns(validatorManifestFactory);
            Mock.Get(serviceProvider).Setup(x => x.GetService(typeof(IResolvesServices))).Returns(resolver);
            Mock.Get(resolver).Setup(x => x.ResolveService<object>(typeof(GenericValidatorDefinition<object>))).Returns(() => new GenericValidatorDefinition<object>());

            var result = sut.GetValidatorBuilderContext(typeof(GenericValidatorDefinition<object>), context);

            Assert.That(result?.ManifestValue.ValidatedType, Is.EqualTo(typeof(object)), "Result is builder context for correct type");
        }

        public class GenericValidatorDefinition<T> : IBuildsValidator<T>
        {
            public IConfiguresValidator<T> LastConfig { get; private set; }

            public void ConfigureValidator(IConfiguresValidator<T> config) => LastConfig = config;
        }

        public class ValidatedObjectDefinition : IBuildsValidator<ValidatedObject>
        {
            public void ConfigureValidator(IConfiguresValidator<ValidatedObject> config) => config.UseObjectIdentity(o => o.Identity);
        }

        public class InvalidDefinitionWithNoInterface {}

        public class InvalidDefinitionWithTwoInterfaces : IBuildsValidator<string>, IBuildsValidator<int>
        {
            public void ConfigureValidator(IConfiguresValidator<string> config) => throw new NotImplementedException();

            public void ConfigureValidator(IConfiguresValidator<int> config) => throw new NotImplementedException();
        }
    }
}