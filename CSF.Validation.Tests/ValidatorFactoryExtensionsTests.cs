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
    [TestFixture, NUnit.Framework.Parallelizable]
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

            public Task<IQueryableValidationResult<ValidatedObject>> ValidateAsync(ValidatedObject validatedObject, ValidationOptions options = null, CancellationToken cancellationToken = default)
                => throw new System.NotImplementedException();
        }

        #endregion
    }
}