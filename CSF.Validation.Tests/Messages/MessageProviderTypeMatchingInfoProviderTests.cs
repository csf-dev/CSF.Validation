using System.Linq;
using NUnit.Framework;

namespace CSF.Validation.Messages
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class MessageProviderTypeMatchingInfoProviderTests
    {
        [Test,AutoMoqData]
        public void GetMatchingInfoShouldReturnAUniversalMatchForATypeWithNoAttributes(MessageProviderTypeMatchingInfoProvider sut)
        {
            Assert.Multiple(() =>
            {
                Assert.That(() => sut.GetMatchingInfo(typeof(NoAttributesRuleClass)), Has.Length.EqualTo(1), "Correct number of results");
                Assert.That(() => sut.GetMatchingInfo(typeof(NoAttributesRuleClass)), Has.None.InstanceOf<FailureMessageStrategyAttribute>(), "No strategy results");
            });
        }

        [Test,AutoMoqData]
        public void GetMatchingInfoShouldReturnAllAttributesForATypeWithSome(MessageProviderTypeMatchingInfoProvider sut)
        {
            Assert.That(() => sut.GetMatchingInfo(typeof(ThreeAttributesRuleClass)).OfType<FailureMessageStrategyAttribute>().Select(x => x.RuleName),
                        Is.EqualTo(new [] { "Foo", "Bar", "Baz" }));
        }

        class NoAttributesRuleClass {}

        [FailureMessageStrategy(RuleName = "Foo")]
        [FailureMessageStrategy(RuleName = "Bar")]
        [FailureMessageStrategy(RuleName = "Baz")]
        class ThreeAttributesRuleClass {}
    }
}