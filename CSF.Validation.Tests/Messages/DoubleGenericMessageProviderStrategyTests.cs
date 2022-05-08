using System;
using AutoFixture.NUnit3;
using CSF.Validation.Rules;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.Messages
{
    [TestFixture,Parallelizable]
    public class DoubleGenericMessageProviderStrategyTests
    {
        [Test,AutoMoqData]
        public void GetNonGenericFailureMessageProviderShouldCreateTheProviderFromAServiceProviderAndWrapIt([AutofixtureServices] IServiceProvider serviceProvider,
                                                                                                            [Frozen] IGetsFailureMessage<string,int> wrapped,
                                                                                                            DoubleGenericMessageProviderStrategy sut,
                                                                                                            [RuleResult(ValidatedValue = "Foo", ParentValue = 5)] ValidationRuleResult ruleResult)
        {
            var result = sut.GetNonGenericFailureMessageProvider(typeof(IGetsFailureMessage<string,int>), typeof(IRule<string,int>));
            result.GetFailureMessageAsync(ruleResult);
            Mock.Get(wrapped).Verify(x => x.GetFailureMessageAsync("Foo", 5, ruleResult, default), Times.Once);
        }
    }
}