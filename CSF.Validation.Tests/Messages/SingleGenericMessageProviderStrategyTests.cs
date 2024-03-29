using System;
using AutoFixture.NUnit3;
using CSF.Validation.Rules;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.Messages
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class SingleGenericMessageProviderStrategyTests
    {
        [Test,AutoMoqData]
        public void GetNonGenericFailureMessageProviderShouldCreateTheProviderFromAServiceProviderAndWrapIt([AutofixtureServices] IServiceProvider serviceProvider,
                                                                                                            [Frozen] IGetsFailureMessage<string> wrapped,
                                                                                                            SingleGenericMessageProviderStrategy sut,
                                                                                                            [RuleResult(ValidatedValue = "Foo")] ValidationRuleResult ruleResult)
        {
            var result = sut.GetNonGenericFailureMessageProvider(new MessageProviderTypeInfo(typeof(IGetsFailureMessage<string>), default), typeof(IRule<string>));
            result.GetFailureMessageAsync(ruleResult);
            Mock.Get(wrapped).Verify(x => x.GetFailureMessageAsync("Foo", ruleResult, default), Times.Once);
        }
    }
}