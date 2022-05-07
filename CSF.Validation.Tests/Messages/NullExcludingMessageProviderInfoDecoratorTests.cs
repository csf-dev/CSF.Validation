using AutoFixture.NUnit3;
using CSF.Validation.Rules;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.Messages
{
    [TestFixture,Parallelizable]
    public class NullExcludingMessageProviderInfoDecoratorTests
    {
        [Test,AutoMoqData]
        public void GetMessageProviderInfoShouldExcludeProviderInfosWhereTheProviderIsNull([Frozen] IGetsMessageProviderInfo wrapped,
                                                                                           NullExcludingMessageProviderInfoDecorator sut,
                                                                                           MessageProviderInfo provider1,
                                                                                           MessageProviderInfo provider2,
                                                                                           MessageProviderInfo provider3,
                                                                                           IGetsFailureMessage providerService,
                                                                                           [RuleResult] ValidationRuleResult ruleResult)
        {
            Mock.Get(provider1).SetupGet(x => x.MessageProvider).Returns(providerService);
            Mock.Get(provider2).SetupGet(x => x.MessageProvider).Returns(() => null);
            Mock.Get(provider3).SetupGet(x => x.MessageProvider).Returns(providerService);
            Mock.Get(wrapped).Setup(x => x.GetMessageProviderInfo(ruleResult)).Returns(() => new[] { provider1, provider2, provider3 });
            Assert.That(() => sut.GetMessageProviderInfo(ruleResult), Is.EquivalentTo(new[] { provider1, provider3 }));
        }
    }
}