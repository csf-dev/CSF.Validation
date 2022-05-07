using System;
using AutoFixture.NUnit3;
using CSF.Validation.Messages;
using NUnit.Framework;

namespace CSF.Validation.Tests.Messages
{
    [TestFixture,Parallelizable]
    public class NonGenericMessageProviderStrategyTests
    {
        [Test,AutoMoqData]
        public void GetNonGenericFailureMessageProviderShouldReturnTheProviderFromAServiceProvider([AutofixtureServices] IServiceProvider serviceProvider,
                                                                                                   [Frozen] IGetsFailureMessage provider,
                                                                                                   NonGenericMessageProviderStrategy sut,
                                                                                                   Type ruleInterface)
        {
            Assert.That(() => sut.GetNonGenericFailureMessageProvider(typeof(IGetsFailureMessage), ruleInterface), Is.SameAs(provider));
        }
    }
}