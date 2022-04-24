using System;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Manifest;
using CSF.Validation.ManifestModel;
using CSF.Validation.ValidatorBuilding;
using Moq;
using NUnit.Framework;

namespace CSF.Validation
{
    [TestFixture,Parallelizable]
    public class ValidatorFactoryExtensionsTests
    {
        #region GetValidator

        [Test,AutoMoqData]
        public void GetValidatorShouldReturnCorrectGenericValidatorForBuilder(IGetsValidator factory,
                                                                              StubValidator validator)
        {
            Mock.Get(factory)
                .Setup(x => x.GetValidator(typeof(ValidatedObjectBuilder)))
                .Returns(validator);

            Assert.That(() => factory.GetValidator<ValidatedObject>(typeof(ValidatedObjectBuilder)), Is.SameAs(validator));
        }

        [Test,AutoMoqData]
        public void GetValidatorShouldThrowIfBuilderTypeIsNotCorrectForValidatedType(IGetsValidator factory)
        {
            Assert.That(() => factory.GetValidator<ValidatedObject>(typeof(DifferentObjectBuilder)), Throws.ArgumentException);
        }

        [Test,AutoMoqData]
        public void GetValidatorShouldReturnCorrectGenericValidatorForManifest(IGetsValidator factory,
                                                                               StubValidator validator,
                                                                               [ManifestModel] ValidationManifest manifest)
        {
            Mock.Get(factory)
                .Setup(x => x.GetValidator(manifest))
                .Returns(validator);
            manifest.ValidatedType = typeof(ValidatedObject);

            Assert.That(() => factory.GetValidator<ValidatedObject>(manifest), Is.SameAs(validator));
        }

        [Test,AutoMoqData]
        public void GetValidatorShouldReturnCorrectGenericValidatorForModel(IGetsValidator factory,
                                                                               StubValidator validator,
                                                                               [ManifestModel] Value model)
        {
            Mock.Get(factory)
                .Setup(x => x.GetValidator(model, typeof(ValidatedObject)))
                .Returns(validator);

            Assert.That(() => factory.GetValidator<ValidatedObject>(model), Is.SameAs(validator));
        }
        
        [Test,AutoMoqData]
        public void GetValidatorShouldThrowIfManifestTypeIsNotCorrectForValidatedType(IGetsValidator factory,
                                                                                      [ManifestModel] ValidationManifest manifest)
        {
            manifest.ValidatedType = typeof(string);
            Assert.That(() => factory.GetValidator<ValidatedObject>(manifest), Throws.ArgumentException);
        }

        #endregion

        #region GetValidatorWithMessageSupport

        [Test,AutoMoqData]
        public void GetValidatorWithMessageSupportShouldReturnCorrectGenericValidatorForBuilder(IGetsValidator factory,
                                                                              MessageValidator validator)
        {
            Mock.Get(factory)
                .Setup(x => x.GetValidatorWithMessageSupport(typeof(ValidatedObjectBuilder)))
                .Returns(validator);

            Assert.That(() => factory.GetValidatorWithMessageSupport<ValidatedObject>(typeof(ValidatedObjectBuilder)), Is.SameAs(validator));
        }

        [Test,AutoMoqData]
        public void GetValidatorWithMessageSupportShouldThrowIfBuilderTypeIsNotCorrectForValidatedType(IGetsValidator factory)
        {
            Assert.That(() => factory.GetValidatorWithMessageSupport<ValidatedObject>(typeof(DifferentObjectBuilder)), Throws.ArgumentException);
        }

        [Test,AutoMoqData]
        public void GetValidatorWithMessageSupportShouldReturnCorrectGenericValidatorForManifest(IGetsValidator factory,
                                                                               MessageValidator validator,
                                                                               [ManifestModel] ValidationManifest manifest)
        {
            Mock.Get(factory)
                .Setup(x => x.GetValidatorWithMessageSupport(manifest))
                .Returns(validator);
            manifest.ValidatedType = typeof(ValidatedObject);

            Assert.That(() => factory.GetValidatorWithMessageSupport<ValidatedObject>(manifest), Is.SameAs(validator));
        }

        [Test,AutoMoqData]
        public void GetValidatorWithMessageSupportShouldReturnCorrectGenericValidatorForModel(IGetsValidator factory,
                                                                               MessageValidator validator,
                                                                               [ManifestModel] Value model)
        {
            Mock.Get(factory)
                .Setup(x => x.GetValidatorWithMessageSupport(model, typeof(ValidatedObject)))
                .Returns(validator);

            Assert.That(() => factory.GetValidatorWithMessageSupport<ValidatedObject>(model), Is.SameAs(validator));
        }
        
        [Test,AutoMoqData]
        public void GetValidatorWithMessageSupportShouldThrowIfManifestTypeIsNotCorrectForValidatedType(IGetsValidator factory,
                                                                                      [ManifestModel] ValidationManifest manifest)
        {
            manifest.ValidatedType = typeof(string);
            Assert.That(() => factory.GetValidatorWithMessageSupport<ValidatedObject>(manifest), Throws.ArgumentException);
        }

        #endregion

        #region Stubs

        public class ValidatedObject {}

        public class ValidatedObjectBuilder : IBuildsValidator<ValidatedObject>
        {
            public void ConfigureValidator(IConfiguresValidator<ValidatedObject> config)
                => throw new System.NotImplementedException();
        }

        public class DifferentObjectBuilder : IBuildsValidator<string>
        {
            public void ConfigureValidator(IConfiguresValidator<string> config)
                => throw new System.NotImplementedException();
        }

        public class StubValidator : IValidator, IValidator<ValidatedObject>
        {
            public Type ValidatedType => typeof(ValidatedObject);

            public Task<ValidationResult> ValidateAsync(object validatedObject, ValidationOptions options = null, CancellationToken cancellationToken = default)
                => throw new System.NotImplementedException();

            public Task<ValidationResult> ValidateAsync(ValidatedObject validatedObject, ValidationOptions options = null, CancellationToken cancellationToken = default)
                => throw new System.NotImplementedException();
        }

        public class MessageValidator : IValidatorWithMessages, IValidatorWithMessages<ValidatedObject>
        {
            public Type ValidatedType => typeof(ValidatedObject);

            public Task<ValidationResultWithMessages> ValidateAsync(object validatedObject, ValidationOptions options = null, CancellationToken cancellationToken = default)
                => throw new System.NotImplementedException();

            public Task<ValidationResultWithMessages> ValidateAsync(ValidatedObject validatedObject, ValidationOptions options = null, CancellationToken cancellationToken = default)
                => throw new System.NotImplementedException();
        }

        #endregion
    }
}