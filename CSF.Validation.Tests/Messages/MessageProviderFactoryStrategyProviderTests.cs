using System;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Rules;
using NUnit.Framework;

namespace CSF.Validation.Messages
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class MessageProviderFactoryStrategyProviderTests
    {
        [Test,AutoMoqData]
        public void GetMessageProviderFactoryShouldReturnADoubleGenericStrategyForAProviderTypeWhichMatchesADoubleGenericRule([AutofixtureServices] IServiceProvider serviceProvider,
                                                                                                                              MessageProviderFactoryStrategyProvider sut)
        {
            Assert.That(() => sut.GetMessageProviderFactory(new MessageProviderTypeInfo(typeof(TwoGenericProvider), default), typeof(IRule<string, int>)), Is.InstanceOf<DoubleGenericMessageProviderStrategy>());
        }

        [Test,AutoMoqData]
        public void GetMessageProviderFactoryShouldReturnASingleGenericStrategyForAProviderTypeWhichMatchesASingleGenericRule([AutofixtureServices] IServiceProvider serviceProvider,
                                                                                                                              MessageProviderFactoryStrategyProvider sut)
        {
            Assert.That(() => sut.GetMessageProviderFactory(new MessageProviderTypeInfo(typeof(OneGenericProvider), default), typeof(IRule<string>)), Is.InstanceOf<SingleGenericMessageProviderStrategy>());
        }

        [Test,AutoMoqData]
        public void GetMessageProviderFactoryShouldReturnASingleGenericStrategyForAProviderTypeWhichMatchesTheFirstTypeOfADoubleGenericRule([AutofixtureServices] IServiceProvider serviceProvider,
                                                                                                                                            MessageProviderFactoryStrategyProvider sut)
        {
            Assert.That(() => sut.GetMessageProviderFactory(new MessageProviderTypeInfo(typeof(OneGenericProvider), default), typeof(IRule<string, int>)), Is.InstanceOf<SingleGenericMessageProviderStrategy>());
        }

        [Test,AutoMoqData]
        public void GetMessageProviderFactoryShouldReturnANonGenericProviderIfSpecified([AutofixtureServices] IServiceProvider serviceProvider,
                                                                                        MessageProviderFactoryStrategyProvider sut)
        {
            Assert.That(() => sut.GetMessageProviderFactory(new MessageProviderTypeInfo(typeof(NonGenericProvider), default), typeof(IRule<string, int>)), Is.InstanceOf<NonGenericMessageProviderStrategy>());
        }

        [Test,AutoMoqData]
        public void GetMessageProviderFactoryShouldReturnNullForAProviderTypeWhichDoesNotMatchTheRuleInterface([AutofixtureServices] IServiceProvider serviceProvider,
                                                                                                               MessageProviderFactoryStrategyProvider sut)
        {
            Assert.That(() => sut.GetMessageProviderFactory(new MessageProviderTypeInfo(typeof(OneGenericProvider), default), typeof(IRule<bool>)), Is.Null);
        }

        [Test,AutoMoqData]
        public void GetMessageProviderFactoryShouldReturnADoubleGenericStrategyIfTheProviderTypeIsAmbiguousButMatchesADoubleGenericRule([AutofixtureServices] IServiceProvider serviceProvider,
                                                                                                                                        MessageProviderFactoryStrategyProvider sut)
        {
            Assert.That(() => sut.GetMessageProviderFactory(new MessageProviderTypeInfo(typeof(MultipleInterfaceProvider), default), typeof(IRule<string,int>)), Is.InstanceOf<DoubleGenericMessageProviderStrategy>());
        }

        [Test,AutoMoqData]
        public void GetMessageProviderFactoryShouldThrowIfRuleInterfaceIsInvalid([AutofixtureServices] IServiceProvider serviceProvider,
                                                                                 MessageProviderFactoryStrategyProvider sut)
        {
            Assert.That(() => sut.GetMessageProviderFactory(new MessageProviderTypeInfo(typeof(OneGenericProvider), default), typeof(bool)), Throws.ArgumentException);
        }

        public class TwoGenericProvider : IGetsFailureMessage<string, int>
        {
            public ValueTask<string> GetFailureMessageAsync(string value, int parentValue, ValidationRuleResult result, CancellationToken token = default)
            {
                throw new System.NotImplementedException();
            }
        }

        public class OneGenericProvider : IGetsFailureMessage<string>
        {
            public ValueTask<string> GetFailureMessageAsync(string value, ValidationRuleResult result, CancellationToken token = default)
            {
                throw new System.NotImplementedException();
            }
        }

        public class NonGenericProvider : IGetsFailureMessage
        {
            public ValueTask<string> GetFailureMessageAsync(ValidationRuleResult result, CancellationToken token = default)
            {
                throw new System.NotImplementedException();
            }
        }

        public class MultipleInterfaceProvider : IGetsFailureMessage<string, int>, IGetsFailureMessage<string>, IGetsFailureMessage, IGetsFailureMessage<int>
        {
            ValueTask<string> IGetsFailureMessage<string, int>.GetFailureMessageAsync(string value, int parentValue, ValidationRuleResult result, CancellationToken token)
            {
                throw new System.NotImplementedException();
            }

            ValueTask<string> IGetsFailureMessage<string>.GetFailureMessageAsync(string value, ValidationRuleResult result, CancellationToken token)
            {
                throw new System.NotImplementedException();
            }

            ValueTask<string> IGetsFailureMessage.GetFailureMessageAsync(ValidationRuleResult result, CancellationToken token)
            {
                throw new System.NotImplementedException();
            }

            ValueTask<string> IGetsFailureMessage<int>.GetFailureMessageAsync(int value, ValidationRuleResult result, CancellationToken token)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}