using System.Reflection;
using CSF.Validation.IntegrationTests;
using NUnit.Framework;

namespace CSF.Validation.Bootstrap
{
    [TestFixture,Parallelizable]
    public class MessageProviderRegistrationBuilderTests
    {
        [Test,AutoMoqData]
        public void AddMessageProvidersInAssemblyShouldAddSomeKnownMessageProviders(MessageProviderRegistrationBuilder sut)
        {
            sut.AddMessageProvidersInAssembly(Assembly.GetExecutingAssembly());
            Assert.That(sut.MessageProviderTypes, Is.SupersetOf(new[] { typeof(CantBeOwnedByUnderageChildrenMessageProvider), typeof(DateTimeInRangeMessageProvider) }));
        }
    }
}