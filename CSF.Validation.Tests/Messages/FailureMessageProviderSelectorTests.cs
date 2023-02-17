using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using CSF.Validation.Rules;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.Messages
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class FailureMessageProviderSelectorTests
    {
        [Test,AutoMoqData]
        public void GetProviderShouldReturnTheProviderWithTheHighestPriorityValue([Frozen] IGetsMessageProviderInfoFactory providerFactoryFactory,
                                                                                  FailureMessageProviderSelector sut,
                                                                                  IGetsMessageProviderInfo providerFactory,
                                                                                  SampleProvider provider1,
                                                                                  SampleProvider provider2,
                                                                                  SampleProvider provider3,
                                                                                  [RuleResult] ValidationRuleResult ruleResult)
        {
            var info1 = new TestingMessageProviderInfo(provider1, 4);
            var info2 = new TestingMessageProviderInfo(provider2, 10);
            var info3 = new TestingMessageProviderInfo(provider3, 3);
            Mock.Get(providerFactoryFactory).Setup(x => x.GetProviderInfoFactory()).Returns(providerFactory);
            Mock.Get(providerFactory).Setup(x => x.GetMessageProviderInfo(ruleResult)).Returns(new[] { info1, info2, info3 });

            Assert.That(() => sut.GetProvider(ruleResult), Is.SameAs(provider2));
        }

        public class SampleProvider : IGetsFailureMessage
        {
            ValueTask<string> IGetsFailureMessage.GetFailureMessageAsync(ValidationRuleResult result, CancellationToken token)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}