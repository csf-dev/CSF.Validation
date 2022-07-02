using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using CSF.Validation.Rules;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.Messages
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class RuleWithMessageCandidateTypeDecoratorTests
    {
        [Test,AutoMoqData]
        public void GetCandidateMessageProviderTypesShouldAlwaysReturnWrappedResults([Frozen] IGetsCandidateMessageTypes wrapped,
                                                                                     RuleWithMessageCandidateTypeDecorator sut,
                                                                                     MessageProviderTypeInfo typeInfo1,
                                                                                     MessageProviderTypeInfo typeInfo2,
                                                                                     MessageProviderTypeInfo typeInfo3,
                                                                                     [RuleResult] ValidationRuleResult result)
        {
            Mock.Get(wrapped).Setup(x => x.GetCandidateMessageProviderTypes(result)).Returns(new[] { typeInfo1, typeInfo2, typeInfo3 });
            Assert.That(() => sut.GetCandidateMessageProviderTypes(result), Is.SupersetOf(new[] { typeInfo1, typeInfo2, typeInfo3 }));
        }

        [Test,AutoMoqData]
        public void GetCandidateMessageProviderTypeShouldAddAResultForADoubleGenericRuleWithMessageWhenItMatches([Frozen] IGetsCandidateMessageTypes wrapped,
                                                                                                                 RuleWithMessageCandidateTypeDecorator sut,
                                                                                                                 [RuleResult(RuleInterface = typeof(IRule<string,int>))] ValidationRuleResult result)
        {
            Mock.Get(wrapped).Setup(x => x.GetCandidateMessageProviderTypes(result)).Returns(Array.Empty<MessageProviderTypeInfo>());
            Mock.Get(result.ValidationLogic).SetupGet(x => x.RuleObject).Returns(new DoubleGenericRuleWithMessage());
            Assert.That(() => sut.GetCandidateMessageProviderTypes(result), Has.One.InstanceOf<InstanceMessageProviderInfo>());
        }

        [Test,AutoMoqData]
        public void GetCandidateMessageProviderTypeShouldAddAResultForASingleGenericRuleWithMessageWhenItMatches([Frozen] IGetsCandidateMessageTypes wrapped,
                                                                                                                 RuleWithMessageCandidateTypeDecorator sut,
                                                                                                                 [RuleResult(RuleInterface = typeof(IRule<string>))] ValidationRuleResult result)
        {
            Mock.Get(wrapped).Setup(x => x.GetCandidateMessageProviderTypes(result)).Returns(Array.Empty<MessageProviderTypeInfo>());
            Mock.Get(result.ValidationLogic).SetupGet(x => x.RuleObject).Returns(new SingleGenericRuleWithMessage());
            Assert.That(() => sut.GetCandidateMessageProviderTypes(result), Has.One.InstanceOf<InstanceMessageProviderInfo>());
        }

        [Test,AutoMoqData]
        public void GetCandidateMessageProviderTypeShouldNotAddAResultIfTheRuleObjectDoesNotMatchTheRuleWithMessageInterface([Frozen] IGetsCandidateMessageTypes wrapped,
                                                                                                                             RuleWithMessageCandidateTypeDecorator sut,
                                                                                                                             [RuleResult(RuleInterface = typeof(IRule<string>))] ValidationRuleResult result)
        {
            Mock.Get(wrapped).Setup(x => x.GetCandidateMessageProviderTypes(result)).Returns(Array.Empty<MessageProviderTypeInfo>());
            Mock.Get(result.ValidationLogic).SetupGet(x => x.RuleObject).Returns(new DoubleGenericRuleWithMessage());
            Assert.That(() => sut.GetCandidateMessageProviderTypes(result), Is.Empty);
        }

        class DoubleGenericRuleWithMessage : IRuleWithMessage<string,int>
        {
            public Task<string> GetFailureMessageAsync(string value, int parentValue, ValidationRuleResult result, CancellationToken token = default)
                => throw new System.NotImplementedException();

            public Task<RuleResult> GetResultAsync(string validated, int parentValue, RuleContext context, CancellationToken token = default)
                => throw new System.NotImplementedException();
        }

        class SingleGenericRuleWithMessage : IRuleWithMessage<string>
        {
            public Task<string> GetFailureMessageAsync(string value, ValidationRuleResult result, CancellationToken token = default)
                => throw new System.NotImplementedException();

            public Task<RuleResult> GetResultAsync(string validated, RuleContext context, CancellationToken token = default)
                => throw new System.NotImplementedException();
        }

    }
}