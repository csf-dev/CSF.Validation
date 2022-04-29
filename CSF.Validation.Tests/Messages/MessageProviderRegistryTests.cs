using System;
using System.Linq;
using AutoFixture.NUnit3;
using CSF.Validation.Rules;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.Messages
{
    [TestFixture,Parallelizable]
    public class MessageProviderRegistryTests
    {
        [Test,AutoMoqData]
        public void RegisterMessageProviderTypesShouldThrowIfTypesAreNull(MessageProviderRegistry sut)
        {
            Assert.That(() => sut.RegisterMessageProviderTypes(null), Throws.ArgumentNullException);
        }

        [Test,AutoMoqData]
        public void RegisterMessageProviderTypesShouldThrowIfDuplicateTypesAreAdded([Frozen] IGetsRuleMatchingInfoForMessageProviderType matchingInfoProvider,
                                                                                    MessageProviderRegistry sut,
                                                                                    Type type)
        {
            Mock.Get(matchingInfoProvider).Setup(x => x.GetMatchingInfo(type)).Returns(Enumerable.Empty<IGetsMessageProviderTypeMatchingInfoForRule>());
            sut.RegisterMessageProviderTypes(type);
            Assert.That(() => sut.RegisterMessageProviderTypes(type), Throws.InvalidOperationException);
        }

        [Test,AutoMoqData]
        public void GetCandidateMessageProviderTypesShouldReturnCorrectInfoForARule([Frozen] IGetsRuleMatchingInfoForMessageProviderType matchingInfoProvider,
                                                                                    MessageProviderRegistry sut,
                                                                                    IGetsMessageProviderTypeMatchingInfoForRule info1,
                                                                                    IGetsMessageProviderTypeMatchingInfoForRule info2,
                                                                                    IGetsMessageProviderTypeMatchingInfoForRule info3,
                                                                                    IGetsMessageProviderTypeMatchingInfoForRule info4,
                                                                                    IGetsMessageProviderTypeMatchingInfoForRule info5,
                                                                                    [RuleResult] ValidationRuleResult ruleResult)
        {
            // These have to be three different types, to avoid a no-dupes exception
            var providerType1 = typeof(object);
            var providerType2 = typeof(string);
            var providerType3 = typeof(int);
            Mock.Get(matchingInfoProvider).Setup(x => x.GetMatchingInfo(providerType1)).Returns(new[] { info1 });
            Mock.Get(matchingInfoProvider).Setup(x => x.GetMatchingInfo(providerType2)).Returns(new[] { info2, info3 });
            Mock.Get(matchingInfoProvider).Setup(x => x.GetMatchingInfo(providerType3)).Returns(new[] { info4, info5 });
            Mock.Get(info1).Setup(x => x.IsMatch(ruleResult)).Returns(true);
            Mock.Get(info1).Setup(x => x.GetPriority()).Returns(1);
            Mock.Get(info2).Setup(x => x.IsMatch(ruleResult)).Returns(false);
            Mock.Get(info2).Setup(x => x.GetPriority()).Returns(2);
            Mock.Get(info3).Setup(x => x.IsMatch(ruleResult)).Returns(false);
            Mock.Get(info3).Setup(x => x.GetPriority()).Returns(3);
            Mock.Get(info4).Setup(x => x.IsMatch(ruleResult)).Returns(true);
            Mock.Get(info4).Setup(x => x.GetPriority()).Returns(4);
            Mock.Get(info5).Setup(x => x.IsMatch(ruleResult)).Returns(true);
            Mock.Get(info5).Setup(x => x.GetPriority()).Returns(5);

            sut.RegisterMessageProviderTypes(providerType1, providerType2, providerType3);

            var expected = new[] {
                new MessageProviderTypeAndPriority(providerType1, 1),
                new MessageProviderTypeAndPriority(providerType3, 5),
            };
            Assert.That(() => sut.GetCandidateMessageProviderTypes(ruleResult), Is.EquivalentTo(expected));
        }
    }
}