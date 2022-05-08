using System;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Rules;
using NUnit.Framework;

namespace CSF.Validation.Messages
{
    [TestFixture,Parallelizable]
    public class MessageProviderFactoryStrategyProviderTests
    {
        [Test,AutoMoqData]
        public void GetMessageProviderFactoryShouldReturnADoubleGenericStrategyForAProviderTypeWhichMatchesADoubleGenericRule([AutofixtureServices] IServiceProvider serviceProvider,
                                                                                                                              MessageProviderFactoryStrategyProvider sut)
        {
            Assert.That(() => sut.GetMessageProviderFactory(typeof(TwoGenericProvider), typeof(IRule<string, int>)), Is.InstanceOf<DoubleGenericMessageProviderStrategy>());
        }

        [Test,AutoMoqData]
        public void GetMessageProviderFactoryShouldReturnASingleGenericStrategyForAProviderTypeWhichMatchesASingleGenericRule([AutofixtureServices] IServiceProvider serviceProvider,
                                                                                                                              MessageProviderFactoryStrategyProvider sut)
        {
            Assert.That(() => sut.GetMessageProviderFactory(typeof(OneGenericProvider), typeof(IRule<string>)), Is.InstanceOf<SingleGenericMessageProviderStrategy>());
        }

        [Test,AutoMoqData]
        public void GetMessageProviderFactoryShouldReturnASingleGenericStrategyForAProviderTypeWhichMatchesTheFirstTypeOfADoubleGenericRule([AutofixtureServices] IServiceProvider serviceProvider,
                                                                                                                                            MessageProviderFactoryStrategyProvider sut)
        {
            Assert.That(() => sut.GetMessageProviderFactory(typeof(OneGenericProvider), typeof(IRule<string, int>)), Is.InstanceOf<SingleGenericMessageProviderStrategy>());
        }

        [Test,AutoMoqData]
        public void GetMessageProviderFactoryShouldReturnANonGenericProviderIfSpecified([AutofixtureServices] IServiceProvider serviceProvider,
                                                                                        MessageProviderFactoryStrategyProvider sut)
        {
            Assert.That(() => sut.GetMessageProviderFactory(typeof(NonGenericProvider), typeof(IRule<string, int>)), Is.InstanceOf<NonGenericMessageProviderStrategy>());
        }

        [Test,AutoMoqData]
        public void GetMessageProviderFactoryShouldReturnNullForAProviderTypeWhichDoesNotMatchTheRuleInterface([AutofixtureServices] IServiceProvider serviceProvider,
                                                                                                               MessageProviderFactoryStrategyProvider sut)
        {
            Assert.That(() => sut.GetMessageProviderFactory(typeof(OneGenericProvider), typeof(IRule<bool>)), Is.Null);
        }

        [Test,AutoMoqData]
        public void GetMessageProviderFactoryShouldReturnADoubleGenericStrategyIfTheProviderTypeIsAmbiguousButMatchesADoubleGenericRule([AutofixtureServices] IServiceProvider serviceProvider,
                                                                                                                                        MessageProviderFactoryStrategyProvider sut)
        {
            Assert.That(() => sut.GetMessageProviderFactory(typeof(MultipleInterfaceProvider), typeof(IRule<string,int>)), Is.InstanceOf<DoubleGenericMessageProviderStrategy>());
        }

        [Test,AutoMoqData]
        public void GetMessageProviderFactoryShouldThrowIfRuleInterfaceIsInvalid([AutofixtureServices] IServiceProvider serviceProvider,
                                                                                 MessageProviderFactoryStrategyProvider sut)
        {
            Assert.That(() => sut.GetMessageProviderFactory(typeof(OneGenericProvider), typeof(bool)), Throws.ArgumentException);
        }

        public class TwoGenericProvider : IGetsFailureMessage<string, int>
        {
            public Task<string> GetFailureMessageAsync(string value, int parentValue, ValidationRuleResult result, CancellationToken token = default)
            {
                throw new System.NotImplementedException();
            }
        }

        public class OneGenericProvider : IGetsFailureMessage<string>
        {
            public Task<string> GetFailureMessageAsync(string value, ValidationRuleResult result, CancellationToken token = default)
            {
                throw new System.NotImplementedException();
            }
        }

        public class NonGenericProvider : IGetsFailureMessage
        {
            public Task<string> GetFailureMessageAsync(ValidationRuleResult result, CancellationToken token = default)
            {
                throw new System.NotImplementedException();
            }
        }

        public class MultipleInterfaceProvider : IGetsFailureMessage<string, int>, IGetsFailureMessage<string>, IGetsFailureMessage, IGetsFailureMessage<int>
        {
            Task<string> IGetsFailureMessage<string, int>.GetFailureMessageAsync(string value, int parentValue, ValidationRuleResult result, CancellationToken token)
            {
                throw new System.NotImplementedException();
            }

            Task<string> IGetsFailureMessage<string>.GetFailureMessageAsync(string value, ValidationRuleResult result, CancellationToken token)
            {
                throw new System.NotImplementedException();
            }

            Task<string> IGetsFailureMessage.GetFailureMessageAsync(ValidationRuleResult result, CancellationToken token)
            {
                throw new System.NotImplementedException();
            }

            Task<string> IGetsFailureMessage<int>.GetFailureMessageAsync(int value, ValidationRuleResult result, CancellationToken token)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}