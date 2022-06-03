using System.Linq;
using AutoFixture.NUnit3;
using CSF.Validation.Rules;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.Messages
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class CriteriaApplyingMessageProviderInfoDecoratorTests
    {
        [Test,AutoMoqData]
        public void GetMessageProviderInfoShouldExcludeProvidersWhichDoNotMatchCriteria([Frozen] IGetsMessageProviderInfo wrapped,
                                                                                        [Frozen] IGetsNonGenericMessageCriteria criteriaFactory,
                                                                                        CriteriaApplyingMessageProviderInfoDecorator sut,
                                                                                        TestingMessageProviderInfo info1,
                                                                                        TestingMessageProviderInfo info2,
                                                                                        TestingMessageProviderInfo info3,
                                                                                        [RuleResult] ValidationRuleResult ruleResult,
                                                                                        IHasFailureMessageUsageCriteria criteria1,
                                                                                        IHasFailureMessageUsageCriteria criteria2,
                                                                                        IHasFailureMessageUsageCriteria criteria3)
        {
            Mock.Get(wrapped).Setup(x => x.GetMessageProviderInfo(ruleResult)).Returns(new[] { info1, info2, info3 });
            Mock.Get(criteriaFactory).Setup(x => x.GetNonGenericMessageCriteria(info1, ruleResult.RuleInterface)).Returns(criteria1);
            Mock.Get(criteriaFactory).Setup(x => x.GetNonGenericMessageCriteria(info2, ruleResult.RuleInterface)).Returns(criteria2);
            Mock.Get(criteriaFactory).Setup(x => x.GetNonGenericMessageCriteria(info3, ruleResult.RuleInterface)).Returns(criteria3);
            Mock.Get(criteria1).Setup(x => x.CanGetFailureMessage(ruleResult)).Returns(true);
            Mock.Get(criteria2).Setup(x => x.CanGetFailureMessage(ruleResult)).Returns(false);
            Mock.Get(criteria3).Setup(x => x.CanGetFailureMessage(ruleResult)).Returns(true);

            Assert.That(() => sut.GetMessageProviderInfo(ruleResult).Select(x => x.MessageProvider),
                        Is.EquivalentTo(new [] { info1.MessageProvider, info3.MessageProvider }));
        }

        [Test,AutoMoqData]
        public void GetMessageProviderInfoShouldIncreasePriorityBy10ForMatchingProvider([Frozen] IGetsMessageProviderInfo wrapped,
                                                                                        [Frozen] IGetsNonGenericMessageCriteria criteriaFactory,
                                                                                        CriteriaApplyingMessageProviderInfoDecorator sut,
                                                                                        TestingMessageProviderInfo info,
                                                                                        [RuleResult] ValidationRuleResult ruleResult,
                                                                                        IHasFailureMessageUsageCriteria criteria)
        {
            Mock.Get(wrapped).Setup(x => x.GetMessageProviderInfo(ruleResult)).Returns(new[] { info });
            Mock.Get(criteriaFactory).Setup(x => x.GetNonGenericMessageCriteria(info, ruleResult.RuleInterface)).Returns(criteria);
            Mock.Get(criteria).Setup(x => x.CanGetFailureMessage(ruleResult)).Returns(true);

            Assert.That(() => sut.GetMessageProviderInfo(ruleResult).Single().Priority,
                        Is.EqualTo(info.Priority + 10));
        }
    }
}