using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.Manifest
{
    [TestFixture,Parallelizable]
    public class RecursiveManifestValueTests
    {
        [Test,AutoMoqData]
        public void ToStringShouldReturnAStringWithTheTypeAndMemberName([Frozen] IManifestItem item)
        {
            Mock.Get(item).SetupGet(x => x.ValidatedType).Returns(typeof(string));

            var sut = new RecursiveManifestValue(item) { MemberName = "Foo" };

            Assert.That(() => sut.ToString(), Is.EqualTo("[RecursiveManifestValue: Type = String, MemberName = Foo]"));
        }
    }
}