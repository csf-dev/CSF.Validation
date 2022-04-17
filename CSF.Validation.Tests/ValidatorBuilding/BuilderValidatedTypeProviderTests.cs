using CSF.Validation.Stubs;
using NUnit.Framework;

namespace CSF.Validation.ValidatorBuilding
{
    [TestFixture,Parallelizable,Ignore("Temporarily broken, to be restored")]
    public class BuilderValidatedTypeProviderTests
    {
        [Test,AutoMoqData]
        public void GetValidatedTypeShouldReturnTheCorrectGenericTypeForABuilderType(BuilderValidatedTypeProvider sut)
        {
            Assert.That(() => sut.GetValidatedType(typeof(ValidatedObjectValidator)), Is.EqualTo(typeof(ValidatedObject)));
        }

        [Test,AutoMoqData]
        public void GetValidatedTypeShouldThrowIfBuilderTypeIsNull(BuilderValidatedTypeProvider sut)
        {
            Assert.That(() => sut.GetValidatedType(null), Throws.ArgumentNullException);
        }

        [Test,AutoMoqData]
        public void GetValidatedTypeShouldThrowIfBuilderTypeDoesNotImplementBuilderInterface(BuilderValidatedTypeProvider sut)
        {
            Assert.That(() => sut.GetValidatedType(typeof(object)), Throws.ArgumentException);
        }
        
        [Test,AutoMoqData]
        public void GetValidatedTypeShouldThrowIfBuilderTypeBuildsAmbiguousValidator(BuilderValidatedTypeProvider sut)
        {
            Assert.That(() => sut.GetValidatedType(typeof(MultiTypeValidator)), Throws.ArgumentException);
        }
        
        class MultiTypeValidator : IBuildsValidator<string>, IBuildsValidator<int>
        {
            public void ConfigureValidator(IConfiguresValidator<string> config)
                => throw new System.NotImplementedException();

            public void ConfigureValidator(IConfiguresValidator<int> config)
                => throw new System.NotImplementedException();
        }
    }
}