using Moq;
using NUnit.Framework;

namespace CSF.Validation
{
    [TestFixture,Parallelizable]
    public class ExceptionThrowingValidatorWrapperTests
    {
        [Test,AutoMoqData]
        public void WrapValidatorNonGenericShouldReturnInstanceOfGenericWrapper(ExceptionThrowingValidatorWrapper sut)
        {
            var validator = new Mock<IValidator<string>>();
            validator.As<IValidator>().SetupGet(x => x.ValidatedType).Returns(typeof(string));
            Assert.That(() => sut.WrapValidator(validator.As<IValidator>().Object), Is.InstanceOf<ThrowingBehaviourValidatorDecorator<string>>());
        }

        [Test,AutoMoqData]
        public void WrapValidatorGenericShouldReturnInstanceOfGenericWrapper(ExceptionThrowingValidatorWrapper sut, IValidator<string> validator)
        {
            Assert.That(() => sut.WrapValidator(validator), Is.InstanceOf<ThrowingBehaviourValidatorDecorator<string>>());
        }
    }
}