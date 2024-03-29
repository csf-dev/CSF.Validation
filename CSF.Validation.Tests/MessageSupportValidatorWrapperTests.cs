using Moq;
using NUnit.Framework;

namespace CSF.Validation
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class MessageSupportValidatorWrapperTests
    {
        [Test,AutoMoqData]
        public void GetValidatorWithMessageSupportNonGenericShouldReturnInstanceOfMessageAdapter(MessageSupportValidatorWrapper sut)
        {
            var validator = new Mock<IValidator<string>>();
            validator.As<IValidator>().SetupGet(x => x.ValidatedType).Returns(typeof(string));
            Assert.That(() => sut.GetValidatorWithMessageSupport(validator.As<IValidator>().Object), Is.InstanceOf<MessageEnrichingValidatorDecorator<string>>());
        }
        [Test,AutoMoqData]
        public void GetValidatorWithMessageSupportShouldReturnInstanceOfMessageAdapter(MessageSupportValidatorWrapper sut, IValidator<string> validator)
        {
            Assert.That(() => sut.GetValidatorWithMessageSupport(validator), Is.InstanceOf<MessageEnrichingValidatorDecorator<string>>());
        }
    }
}