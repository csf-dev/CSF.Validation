using System;
using AutoFixture.NUnit3;
using CSF.Validation.Messages;
using NUnit.Framework;

namespace CSF.Validation.Messages
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class NonGenericMessageProviderStrategyTests
    {
        [Test,AutoMoqData]
        public void GetNonGenericFailureMessageProviderShouldReturnTheProviderFromAServiceProvider([AutofixtureServices] IServiceProvider serviceProvider,
                                                                                                   [Frozen] IGetsFailureMessage provider,
                                                                                                   NonGenericMessageProviderStrategy sut,
                                                                                                   Type ruleInterface)
        {
            Assert.That(() => sut.GetNonGenericFailureMessageProvider(new MessageProviderTypeInfo(typeof(IGetsFailureMessage), default), ruleInterface), Is.SameAs(provider));
        }
    }
}