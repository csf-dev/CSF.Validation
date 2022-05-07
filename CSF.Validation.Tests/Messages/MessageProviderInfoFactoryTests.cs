using System;
using System.Linq;
using AutoFixture.NUnit3;
using CSF.Validation.Rules;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.Messages
{
    [TestFixture,Parallelizable]
    public class MessageProviderInfoFactoryTests
    {
        [Test,AutoMoqData]
        public void GetMessageProviderInfoShouldReturnProvidersCreatedFromRegistryTypeInfoExcludingThoseForWhichThereIsNoFactory([Frozen] IRegistryOfMessageTypes typeRegistry,
                                                                                                                                 [Frozen] IGetsMessageProviderFactoryStrategy factoryStrategySelector,
                                                                                                                                 MessageProviderInfoFactory sut,
                                                                                                                                 int irrelevantPriority,
                                                                                                                                 [RuleResult] ValidationRuleResult ruleResult,
                                                                                                                                 IGetsNonGenericMessageProvider factory)
        {
            MessageProviderTypeInfo
                type1 = new MessageProviderTypeInfo(typeof(string), irrelevantPriority),
                type2 = new MessageProviderTypeInfo(typeof(int), irrelevantPriority),
                type3 = new MessageProviderTypeInfo(typeof(bool), irrelevantPriority);
            Mock.Get(typeRegistry).Setup(x => x.GetCandidateMessageProviderTypes(ruleResult)).Returns(new[] { type1, type2, type3 });
            Mock.Get(factoryStrategySelector).Setup(x => x.GetMessageProviderFactory(typeof(string), ruleResult.RuleInterface)).Returns(factory);
            Mock.Get(factoryStrategySelector).Setup(x => x.GetMessageProviderFactory(typeof(int), ruleResult.RuleInterface)).Returns(() => null);
            Mock.Get(factoryStrategySelector).Setup(x => x.GetMessageProviderFactory(typeof(bool), ruleResult.RuleInterface)).Returns(factory);

            var result = sut.GetMessageProviderInfo(ruleResult);

            Assert.That(result.Select(x => x.ProviderType), Is.EquivalentTo(new[] { typeof(string), typeof(bool) }));
        }

        [Test,AutoMoqData]
        public void GetMessageProviderInfoShouldCreateProviderUsingLazyResolutionFromTheFactory([Frozen] IRegistryOfMessageTypes typeRegistry,
                                                                                                [Frozen] IGetsMessageProviderFactoryStrategy factoryStrategySelector,
                                                                                                MessageProviderInfoFactory sut,
                                                                                                int irrelevantPriority,
                                                                                                [RuleResult] ValidationRuleResult ruleResult,
                                                                                                IGetsNonGenericMessageProvider factory,
                                                                                                IGetsFailureMessage provider)
        {
            MessageProviderTypeInfo
                type = new MessageProviderTypeInfo(typeof(string), irrelevantPriority);
            Mock.Get(typeRegistry).Setup(x => x.GetCandidateMessageProviderTypes(ruleResult)).Returns(new[] { type });
            Mock.Get(factoryStrategySelector).Setup(x => x.GetMessageProviderFactory(typeof(string), ruleResult.RuleInterface)).Returns(factory);
            Mock.Get(factory).Setup(x => x.GetNonGenericFailureMessageProvider(typeof(string), ruleResult.RuleInterface)).Returns(provider);

            var result = sut.GetMessageProviderInfo(ruleResult);

            Assert.Multiple(() =>
            {
                Mock.Get(factory).Verify(x => x.GetNonGenericFailureMessageProvider(It.IsAny<Type>(), It.IsAny<Type>()),
                                         Times.Never,
                                         "Because the provider hasn't been accessed, the factory should not have yet been used");
                Assert.That(result.Single().MessageProvider,
                            Is.SameAs(provider),
                            "The returned message provider is the same as the expected one.");
                Mock.Get(factory).Verify(x => x.GetNonGenericFailureMessageProvider(typeof(string), ruleResult.RuleInterface),
                                         Times.Once,
                                         "Now that the provider has been accessed, the factory should have been executed");
            });

        }
    }
}